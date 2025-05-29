using Hotel360InteractiveServer.Data;
using Hotel360InteractiveServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Hotel360InteractiveServer.Controller
{
    [Authorize(Roles = "Refeições, Admin")]
    public class RefeicoesController
    {
        private const string BaseSql = @"
SELECT  
    R.Codigo                  AS CodigoReserva,
    R.LinhaReserva            AS LinhaReserva,
    R.DataCriacao             AS DataCriacao,
    R.CheckIn                 AS DataCheckIn,
    R.HoraChegada             AS HoraChegada,
    R.CheckOut                AS DataCheckOut,
    R.nrnoites                AS NumeroNoites,
    R.HoraSaida               AS HoraPartida,
    RA.DataInicio             AS DataInicioAlojamento,
    RA.DataFim                AS DataFimAlojamento,
    RA.Alojamento             AS CodigoAlojamento,
    RA.Categoria              AS CategoriaAlojamento,
    RV.Categoria              AS CategoriaPreco,
    R.NrADultos               AS NumeroAdultos,
    R.NrCriancas              AS NumeroCriancas,
    R.NrBercos                AS NumeroBercos,
    R.NrQuartos               AS NumeroAlojamentos,
    R.PrecoTotal              AS Preco,
    R.TipoReserva             AS CodigoEstadoReserva,
    CAST(R.Observacoes AS VARCHAR(255)) AS Observacoes,
    CAST(R.Voucher    AS VARCHAR(30))  AS Voucher,
    R.Funcionario             AS Utilizador,
    T.Codigo                  AS CodigoHospede,
    ISNULL(T.Nome,  isnull(REH.nome,''))    AS NomeHospede,
    ISNULL(T.Apelido, isnull(REH.apelido,'')) AS ApelidoHospede,
    ISNULL(T.Pais,'')         AS CodigoPais,
    PS.Nome                   AS Pais,
    P.Codigo                  AS CodigoPackage,
    P.Descricao               AS DescricaoPackage,
    ISNULL(RF.Data,R.CheckIn) AS DataRefeicao,
    SUM(CASE WHEN (RF.ExtraMeal <> 1 OR RF.PequenoAlmoco = 0)
             THEN ISNULL(RF.PequenoAlmoco,0)
             ELSE ISNULL(RF.BreakFastExtraNumber, 0)
        END)                    AS PequenoAlmoco,
    SUM(CASE WHEN (RF.ExtraMeal <> 1 OR RF.Almoco = 0)
             THEN ISNULL(RF.Almoco,0)
             ELSE ISNULL(RF.LunchExtraNumber, 0)
        END)                    AS Almoco,
    SUM(CASE WHEN (RF.ExtraMeal <> 1 OR RF.Jantar = 0)
             THEN ISNULL(RF.Jantar,0)
             ELSE ISNULL(RF.DinnerExtraNumber, 0)
        END)                    AS Jantar,
    MAX(ISNULL(RF.Observations,'')) AS Observations,
    E.Codigo           AS CodigoEntidade,
    ISNULL(T2.Nome,'') AS NomeEntidade,
    ISNULL(T2.Apelido,'') AS ApelidoEntidade,
    ISNULL(E.Tipo,0)  AS TipoEntidade,
    R.Tipodoc         AS TipoDocFP,
    R.Serie           AS SerieFP,
    R.Numdoc          AS NumDocFP,
    0                 AS ValorPendente
FROM whotreservas R
INNER JOIN whotreservasalojamentos RA
    ON R.Unidade = RA.Unidade
   AND R.Codigo  = RA.codigoreserva
   AND R.LinhaReserva = RA.LinhaReserva
INNER JOIN whotReservasRefeicoes RF
    ON R.Unidade     = RF.Unidade
   AND R.Codigo      = RF.CodigoReserva
   AND R.LinhaReserva= RF.LinhaReserva
INNER JOIN whotreservasvaloresextra RV
    ON RF.Unidade       = RV.Unidade
   AND RF.CodigoReserva = RV.codigoreserva
   AND RF.LinhaReserva  = RV.LinhaReserva
   AND RV.tipo = 2
   AND RV.Datainicio <= RF.Data
   AND (RV.DataFim > RF.Data OR (RV.DataFim = R.Checkout AND RV.DataFim = RF.Data))
LEFT JOIN whotpackages P
    ON RV.Unidade = P.Unidade
   AND RV.Codigo  = P.Codigo
LEFT JOIN whotreservasentidades REH
    ON R.unidade = REH.unidade
   AND R.codigo  = REH.codigoreserva
   AND R.linhareserva = REH.linhareserva
   AND REH.IsHospedePrincipal = 1
LEFT JOIN wgcterceiros T
    ON R.CodigoHospede = T.Codigo
LEFT JOIN wgcPaises PS
    ON T.Pais = PS.Codigo
LEFT JOIN whotreservasentidades E
    ON R.Unidade     = E.Unidade
   AND R.Codigo      = E.CodigoReserva
   AND R.LinhaReserva = E.LinhaReserva
   AND E.IsEntidadePrincipal = 1
LEFT JOIN wgcterceiros T2
    ON E.Codigo = T2.Codigo
WHERE ISNULL(E.Tipo,0) NOT IN (1,2)
  AND R.Dividida = 0
  AND RF.Data BETWEEN @DataDe AND @DataAte
  AND RA.DataInicio <= @DataAte
  AND RA.DataFim    >= @DataDe
  AND R.TipoReserva IN ('RSV','CKI','CKO')
  AND (RF.PequenoAlmoco = 1 OR RF.Almoco = 1 OR RF.Jantar = 1)
GROUP BY 
    R.Unidade, R.Codigo, R.LinhaReserva, R.DataCriacao, R.CheckIn, R.HoraChegada,
    R.CheckOut, R.nrnoites, R.HoraSaida, RA.DataInicio, RA.DataFim, RA.Alojamento,
    RA.Categoria, RV.Categoria, R.NrADultos, R.NrCriancas, R.NrBercos, R.NrQuartos,
    R.PrecoTotal, R.TipoReserva, CAST(R.Observacoes AS VARCHAR(255)),
    CAST(R.Voucher AS VARCHAR(30)), R.Funcionario, T.Codigo,
    ISNULL(T.Nome, isnull(REH.nome,'')), ISNULL(T.Apelido, isnull(REH.apelido,'')),
    ISNULL(T.Pais,''), PS.Nome, P.Codigo, P.Descricao, ISNULL(RF.Data,R.CheckIn),
    E.Codigo, ISNULL(T2.Nome,''), ISNULL(T2.Apelido,''), ISNULL(E.Tipo,0),
    R.Tipodoc, R.Serie, R.Numdoc, RF.Data, R.Quarto
ORDER BY RF.Data, R.Quarto, R.Codigo, R.LinhaReserva, RA.DataInicio;";

        public static async Task<IEnumerable<Refeicao>> GetRefeicoesData(ApplicationDbContext dbContext, DateTime dataDe, DateTime dataAte)
        {
            try
            {
                var parameters = new
                {
                    DataDe = dataDe.Date,
                    DataAte = dataAte.Date
                };

                return await dbContext.QueryAsync<Refeicao>(BaseSql, parameters);

            }
            catch (Exception ex)
            {
                Logs.Erro("Refeições não foram carregadas!", ex);
                return new List<Refeicao>();
            }
        }
        public static async Task<DateTime> GetDia(ApplicationDbContext dbContext)
        {
            string sql = "SELECT datahotel FROM whotparametros";
            var result = await dbContext.QueryAsync<DateTime>(sql);
            return result.FirstOrDefault();
        }
        public static async Task SetConfirmacaoAsync(ApplicationDbContext dbContext, Refeicao refeicao)
        {
            string codigoRefeicao = $"{refeicao.CodigoReserva}_{refeicao.LinhaReserva}";

            tipoRefeicao tipoRefeicao;
            if (refeicao.PequenoAlmoco)
                tipoRefeicao = tipoRefeicao.PequenoAlmoco;
            else if (refeicao.Almoco)
                tipoRefeicao = tipoRefeicao.Almoco;
            else if (refeicao.Jantar)
                tipoRefeicao = tipoRefeicao.Jantar;
            else
                throw new InvalidOperationException("Tipo de refeição não especificado.");

            string sql = @"
                MERGE INTO ConfirmRefeicao AS Target
                USING (SELECT @CodigoRefeicao AS CodigoRefeicao, @DataRefeicao AS DataRefeicao, @TipoRefeicao AS TipoRefeicao) AS Source
                ON Target.CodigoRefeicao = Source.CodigoRefeicao AND Target.DataRefeicao = Source.DataRefeicao AND Target.TipoRefeicao = Source.TipoRefeicao
                WHEN MATCHED THEN
                    UPDATE SET Confirmed = @Confirmed
                WHEN NOT MATCHED THEN
                    INSERT (CodigoRefeicao, DataRefeicao, TipoRefeicao, Confirmed)
                    VALUES (@CodigoRefeicao, @DataRefeicao, @TipoRefeicao, @Confirmed);";

            await dbContext.ExecuteAsync(sql, new
            {
                CodigoRefeicao = codigoRefeicao,
                DataRefeicao = refeicao.DataRefeicao.Date,
                TipoRefeicao = tipoRefeicao.ToString(),
                Confirmed = refeicao.Confirmado
            });
        }
        public static async Task<IEnumerable<ConfirmRefeicao>> GetConfirmacoesAsync(ApplicationDbContext dbContext, DateTime from, DateTime to)
        {
            string sql = @"
                SELECT CodigoRefeicao, DataRefeicao, TipoRefeicao, Confirmed
                FROM ConfirmRefeicao
                WHERE DataRefeicao BETWEEN @FromDate AND @ToDate";

            return await dbContext.QueryAsync<ConfirmRefeicao>(sql, new
            {
                FromDate = from.Date,
                ToDate = to.Date
            });
        }
    }
}
