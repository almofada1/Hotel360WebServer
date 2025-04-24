using System.Data;
using Hotel360InteractiveServer.Data;
using Hotel360InteractiveServer.Models;

namespace Hotel360InteractiveServer.Controller
{
    class CheckInsController
    {
        public static async Task<List<Wgcpaises>> GetPaises(ApplicationDbContext dbContext)
        {
            try
            {
                string sql = @"SELECT codigo, nome FROM wgcpaises ORDER BY codigo ASC";
                var result = await dbContext.QueryAsync<Wgcpaises>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                Logs.Erro("CarregaPaises", ex);
                return new List<Wgcpaises>();
            }
        }

        public static async Task<bool> UpdateTerceiro(ApplicationDbContext dbContext, Terceiro t)
        {
            try
            {
                string sql = @"UPDATE WGCTERCEIROS 
                        SET morada1 = @Morada, CODPOSTAL = @CodPostal, LOCALPOSTAL = @LocalPostal, PAIS = @Pais, TELEFONE = @Telefone,
                            TELEMOVEL = @Telemovel, SEXO = @Sexo, NCONTRIB = @NContrib, EMAIL = @Email
                        WHERE CODIGO = @Codigo";
                var parameters = new
                {
                    t.morada,
                    t.codPostal,
                    t.localPostal,
                    t.pais,
                    t.telefone,
                    t.telemovel,
                    t.sexo,
                    t.nContrib,
                    t.email,
                    t.Codigo
                };

                int rowsAffected = await dbContext.ExecuteAsync(sql, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Logs.Erro("Erro: UpdateTerceiro : " + ex.Message);
                return false;
            }
        }

        public static async Task<Terceiro> GetTerceiroAsync(ApplicationDbContext dbContext, string codigo)
        {
            try
            {
                string sql = @"SELECT NOME + ' ' + APELIDO AS NOME, codigo, MORADA1 as morada, CODPOSTAL, LOCALPOSTAL, PAIS, TELEFONE, TELEMOVEL, SEXO, NCONTRIB, EMAIL
                       FROM wgcterceiros
                       WHERE codigo = @Codigo";

                var result = await dbContext.QueryAsync<Terceiro>(sql, new { Codigo = codigo });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception
                Logs.Erro("GetTerceiroAsync", ex);
                return null;
            }
        }

        public static async Task<List<CheckIn>> CarregaCheckinsDeDataAsync(ApplicationDbContext dbContext)
        {
            try
            {
                string sql = @"
            SELECT  
                R.Codigo AS CodigoReserva, 
                R.LinhaReserva AS LinhaReserva, 
                R.DataCriacao AS DataCriacao, 
                R.CheckIn AS DataCheckIn, 
                R.HoraChegada AS HoraChegada, 
                R.CheckOut AS DataCheckOut, 
                R.nrnoites AS NumeroNoites, 
                R.HoraSaida AS HoraPartida, 
                RA.Alojamento AS CodigoAlojamento, 
                RA.Categoria AS CategoriaAlojamento, 
                RV.Categoria AS CategoriaPreco, 
                R.NrADultos AS NumeroAdultos, 
                R.NrCriancas AS NumeroCriancas, 
                R.NrBercos AS NumeroBercos, 
                R.NrQuartos AS NumeroAlojamentos, 
                R.PrecoTotal AS Preco, 
                R.TipoReserva AS CodigoEstadoReserva, 
                R.Observacoes AS Observacoes, 
                ISNULL(T.Nome, ISNULL(REH.nome, '')) AS NomeHospede, 
                ISNULL(T.Apelido, ISNULL(REH.apelido, '')) AS ApelidoHospede, 
                ISNULL(T.Codigo, '') AS CodigoHospede,
                ISNULL(T.Pais, '') AS CodigoPais, 
                P.Codigo AS CodigoPackage, 
                P.Descricao as DescricaoPackage, 
                ISNULL(T2.Nome, '') as NomeEntidade, 
                ISNULL(T2.Apelido, '') as ApelidoEntidade,
                ISNULL(T2.Codigo, '') AS CodigoEntidade
            FROM whotreservas AS R  
            INNER JOIN whotreservasalojamentos AS RA 
                ON R.Unidade = RA.Unidade 
                AND R.Codigo = RA.codigoreserva 
                AND R.LinhaReserva = RA.LinhaReserva 
            INNER JOIN whotreservasvaloresextra AS RV 
                ON RA.Unidade = RV.Unidade 
                AND RA.CodigoReserva = RV.codigoreserva 
                AND RA.LinhaReserva = RV.LinhaReserva 
                AND RV.tipo = 2 
                AND RV.Datainicio <= RA.Datainicio 
                AND (RV.DataFim > RA.Datainicio OR (RV.DataInicio = RV.DataFim AND RV.DataFim = RA.Datainicio))  
            LEFT JOIN whotpackages AS P 
                ON RV.Unidade = P.Unidade 
                AND RV.Codigo = P.Codigo  
            LEFT JOIN whotreservasentidades AS REH 
                ON R.unidade = REH.unidade 
                AND R.codigo = REH.codigoreserva 
                AND R.linhareserva = REH.linhareserva 
                AND REH.IsHospedePrincipal = 1  
            LEFT JOIN wgcterceiros AS T 
                ON R.CodigoHospede = T.Codigo 
            LEFT JOIN wgcPaises AS PS 
                ON T.Pais = PS.Codigo 
            LEFT JOIN whotreservasentidades AS E 
                ON R.Unidade = E.Unidade 
                AND R.Codigo = E.CodigoReserva 
                AND R.LinhaReserva = E.LinhaReserva 
                AND E.IsEntidadePrincipal = 1  
            LEFT JOIN wgcterceiros AS T2 
                ON E.Codigo = T2.Codigo 
            WHERE ISNULL(e.Tipo, 0) <> 1 
                AND ISNULL(e.Tipo, 0) <> 2 
                AND R.CheckIn = (select datahotel from whotparametros)
                AND R.TipoReserva IN ('RSV', 'ECF', 'LTE', 'OVB', 'CKI') 
                AND R.Quarto IS NOT NULL 
                AND R.Quarto <> ''
            ORDER BY R.Quarto, R.Codigo, R.LinhaReserva, RA.DataInicio";

                var checkInsResult = await dbContext.QueryAsync<CheckIn>(sql);

                var groupedCheckIns = checkInsResult
                                      .GroupBy(o => o.CodigoReserva)
                                      .Select(g => g.FirstOrDefault())
                                      .ToList();

                return groupedCheckIns;
            }
            catch (Exception ex)
            {
                Logs.Erro("CarregaCheckinsDeHoje", ex);
                return new List<CheckIn>(); // Return an empty list in case of error
            }
        }

        public static async Task<DateTime> GetDia(ApplicationDbContext dbContext)
        {
            string sql = "SELECT datahotel FROM whotparametros";
            var result = await dbContext.QueryAsync<DateTime>(sql);
            return result.FirstOrDefault();
        }

        public static async Task<Ocupado> VerificaOcupadosQuartoAsync(ApplicationDbContext dbContext, CheckIn checkIn)
        {
            try
            {
                string sql = @"
                    SELECT codigo, linhareserva, quarto
                    FROM whotreservas
                    WHERE whotreservas.unidade = 'whotel'
                    AND quarto = @CodigoAlojamento
                    AND tiporeserva = 'CKI'
                    AND NOT (codigo = @CodigoReserva AND linhareserva = @LinhaReserva)";

                var parameters = new
                {
                    checkIn.CodigoAlojamento,
                    checkIn.CodigoReserva,
                    checkIn.LinhaReserva
                };

                var result = await dbContext.QueryAsync<Ocupado>(sql, parameters);
                return result.FirstOrDefault() ?? new Ocupado();
            }
            catch (Exception ex)
            {
                Logs.Erro("VerificaOcupadosQuarto", ex);
                return new Ocupado();
            }
        }


        public static async Task<bool> EfetuarCheckinAsync(ApplicationDbContext dbContext, CheckIn cki)
        {
            try
            {
                string sql = @"
                    UPDATE whotreservas 
                    SET tiporeserva = 'CKI' 
                    WHERE codigo = @CodigoReserva";

                var parameters = new { CodigoReserva = cki.CodigoReserva };
                int rowsAffected = await dbContext.ExecuteAsync(sql, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Logs.Erro("Erro: EfetuarCheckin : " + ex.Message);
                return false;
            }
        }

    }
}
