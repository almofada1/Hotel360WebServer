using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using Hotel360InteractiveServer.Controller;
using Hotel360InteractiveServer.Models;

namespace Hotel360InteractiveServer.ControllerRefeicao
{
    class Refeicoes
    {
        public static List<Refeicao> GetRefeicoesData(DateTime data)
        {
            try
            {
               List<Refeicao> ListaRefeicoes = new List<Refeicao>();
                using (var connection = new SqlConnection(Sessao.SQLServerConnectionString))
                {
                    connection.Open();

                    string sql = string.Format(@"SELECT  R.Codigo AS CodigoReserva, R.LinhaReserva AS LinhaReserva, R.DataCriacao AS DataCriacao, R.CheckIn AS DataCheckIn, R.HoraChegada AS HoraChegada, R.CheckOut AS DataCheckOut, R.nrnoites AS NumeroNoites, R.HoraSaida AS HoraPartida, RA.DataInicio AS DataInicioAlojamento, RA.DataFim AS DataFimAlojamento, RA.Alojamento AS CodigoAlojamento, RA.Categoria AS CategoriaAlojamento, RV.Categoria AS CategoriaPreco, R.NrADultos AS NumeroAdultos, R.NrCriancas AS NumeroCriancas, R.NrBercos AS NumeroBercos, R.NrQuartos AS NumeroAlojamentos, R.PrecoTotal AS Preco, R.TipoReserva AS CodigoEstadoReserva, CAST(R.Observacoes AS VARCHAR(255)) AS Observacoes, CAST(R.Voucher AS VARCHAR(30)) as Voucher, R.Funcionario AS Utilizador, T.Codigo AS CodigoHospede, isnull(T.Nome,isnull(REH.nome,'')) AS NomeHospede, isnull(T.Apelido,isnull(REH.apelido,'')) AS ApelidoHospede, isnull(T.Pais,'') AS CodigoPais, PS.Nome AS Pais, 
                                                         P.Codigo AS CodigoPackage, P.Descricao as DescricaoPackage, isnull(RF.Data,R.CheckIn) AS DataRefeicao, SUM(CASE WHEN (RF.ExtraMeal <> 1 OR RF.PequenoAlmoco = 0)  THEN ISNULL(RF.PequenoAlmoco,0) ELSE ISNULL(RF.BreakFastExtraNumber, 0) END)  AS PequenoAlmoco, SUM(CASE WHEN (RF.ExtraMeal <> 1 OR RF.Almoco =0)  THEN ISNULL(RF.Almoco,0) ELSE ISNULL(RF.LunchExtraNumber, 0) END)  AS Almoco, SUM(CASE WHEN (RF.ExtraMeal <> 1 OR RF.Jantar =0) THEN ISNULL(RF.Jantar,0) ELSE ISNULL(RF.DinnerExtraNumber, 0) END) AS Jantar, MAX(ISNULL(RF.Observations,'')) AS Observations, E.Codigo as CodigoEntidade, isnull(T2.Nome,'') as NomeEntidade, isnull(T2.Apelido,'') as ApelidoEntidade, isnull(E.Tipo,0) as TipoEntidade, R.Tipodoc AS TipoDocFP, R.Serie AS SerieFP, R.Numdoc AS NumDocFP, 0 AS ValorPendente
                                                FROM whotreservas AS R  
                                                INNER JOIN whotreservasalojamentos AS RA ON (R.Unidade=RA.Unidade and R.Codigo=RA.codigoreserva and R.LinhaReserva=RA.LinhaReserva)  INNER JOIN whotReservasRefeicoes as RF ON R.Unidade=RF.Unidade and R.Codigo=RF.CodigoReserva and R.LinhaReserva=RF.LinhaReserva  INNER JOIN whotreservasvaloresextra AS RV ON (RF.Unidade=RV.Unidade and RF.CodigoReserva=RV.codigoreserva and RF.LinhaReserva=RV.LinhaReserva and RV.tipo=2 AND RV.Datainicio<=RF.Data AND (RV.DataFim>RF.Data OR (RV.DataFim=R.Checkout AND RV.DataFim=RF.Data)))  LEFT JOIN whotpackages AS P ON (RV.Unidade=P.Unidade) AND (RV.Codigo=P.Codigo)  LEFT JOIN whotreservasentidades AS REH ON (R.unidade=REH.unidade AND R.codigo=REH.codigoreserva AND R.linhareserva=REH.linhareserva AND REH.IsHospedePrincipal=1)  LEFT JOIN wgcterceiros AS T ON (R.CodigoHospede=T.Codigo)  LEFT JOIN wgcPaises AS PS ON (T.Pais=PS.Codigo)  LEFT JOIN whotreservasentidades AS E ON (R.Unidade=E.Unidade) AND (R.Codigo=E.CodigoReserva) AND (R.LinhaReserva=E.LinhaReserva) AND (E.IsEntidadePrincipal=1)  LEFT JOIN wgcterceiros AS T2 ON (E.Codigo=T2.Codigo)
                                                WHERE ISNULL(e.Tipo, 0)<>1 AND ISNULL(e.Tipo, 0)<>2 AND R.Dividida=0 AND RF.Data>='"+data.ToString("yyyyMMdd")+ @"' AND RF.Data<='" + data.ToString("yyyyMMdd") + @"' AND RA.DataInicio<='" + data.ToString("yyyyMMdd") + @"' AND RA.DataFim>='" + data.ToString("yyyyMMdd") + @"' AND R.TipoReserva in ('RSV','CKI','CKO') AND (RF.PequenoAlmoco=1 or RF.Almoco=1 or RF.Jantar=1)
                                                GROUP BY R.Unidade, R.Codigo, R.LinhaReserva, R.DataCriacao, R.CheckIn, R.HoraChegada, R.CheckOut, R.nrnoites, R.HoraSaida, RA.DataInicio, RA.DataFim, RA.Alojamento, RA.Categoria, RV.Categoria, R.NrADultos, R.NrCriancas, R.NrBercos, R.NrQuartos, R.PrecoTotal, R.TipoReserva, CAST(R.Observacoes AS VARCHAR(255)), CAST(R.Voucher AS VARCHAR(30)), R.Funcionario, T.Codigo, ISNULL(T.Nome, isnull(REH.nome, '')), ISNULL(T.Apelido, isnull(REH.apelido, '')), isnull(T.Pais, ''), PS.Nome, P.Codigo, P.Descricao, isnull(RF.Data, R.CheckIn), E.Codigo, isnull(T2.Nome, ''), isnull(T2.Apelido, ''), isnull(E.Tipo, 0), R.Tipodoc, R.Serie, R.Numdoc, RF.Data, R.Quarto
                                                ORDER BY RF.Data, R.Quarto, R.Codigo, R.LinhaReserva, RA.DataInicio");

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var refeicao = new Refeicao();
                                refeicao.CodigoReserva = reader["CodigoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoReserva"]);
                                refeicao.LinhaReserva = reader["LinhaReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["LinhaReserva"]);
                                refeicao.DataCriacao = reader["DataCriacao"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCriacao"]);
                                refeicao.DataCheckIn = reader["DataCheckIn"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCheckIn"]);
                                refeicao.HoraDeChegada = reader["HoraChegada"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraChegada"]);
                                refeicao.DataCheckOut = reader["DataCheckOut"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCheckOut"]);
                                refeicao.NumeroNoites = reader["NumeroNoites"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroNoites"]);
                                refeicao.HoraPartida = reader["HoraPartida"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraPartida"]);
                                refeicao.DataInicioAlojamento = reader["DataInicioAlojamento"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataInicioAlojamento"]);
                                refeicao.DataFimAlojamento = reader["DataFimAlojamento"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataFimAlojamento"]);
                                refeicao.CodigoAlojamento = reader["CodigoAlojamento"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoAlojamento"]);
                                refeicao.CategoriaAlojamento = reader["CategoriaAlojamento"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CategoriaAlojamento"]);
                                refeicao.CategoriaPreco = reader["CategoriaPreco"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CategoriaPreco"]);
                                refeicao.NumeroAdultos = reader["NumeroAdultos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroAdultos"]);
                                refeicao.NumeroCriancas = reader["NumeroCriancas"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroCriancas"]);
                                refeicao.NumeroBercos = reader["NumeroBercos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroBercos"]);
                                refeicao.NumeroAlojamentos = reader["NumeroAlojamentos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroAlojamentos"]);
                                refeicao.Preco = reader["Preco"].GetType() == typeof(DBNull) ? 0 : Convert.ToDouble(reader["Preco"]);
                                refeicao.CodigoEstadoReserva = reader["CodigoEstadoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoEstadoReserva"]);
                                refeicao.Observacoes = reader["Observacoes"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Observacoes"]);
                                refeicao.Voucher = reader["Voucher"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Voucher"]);
                                refeicao.CodigoHospede = reader["CodigoHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoHospede"]);
                                refeicao.NomeHospede = reader["NomeHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["NomeHospede"]);
                                refeicao.ApelidoHospede = reader["ApelidoHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["ApelidoHospede"]);
                                refeicao.CodigoPais = reader["CodigoPais"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoPais"]);
                                refeicao.Pais = reader["Pais"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Pais"]);
                                refeicao.CodigoPackage = reader["CodigoPackage"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoPackage"]);
                                refeicao.DescricaoPackage = reader["DescricaoPackage"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["DescricaoPackage"]);
                                refeicao.DataRefeicao = reader["DataRefeicao"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataRefeicao"]);
                                refeicao.PequenoAlmoco = reader["PequenoAlmoco"].GetType() == typeof(DBNull) ? false : Convert.ToBoolean(reader["PequenoAlmoco"]);
                                refeicao.Almoco = reader["Almoco"].GetType() == typeof(DBNull) ? false : Convert.ToBoolean(reader["Almoco"]);
                                refeicao.Jantar = reader["Jantar"].GetType() == typeof(DBNull) ? false : Convert.ToBoolean(reader["Jantar"]);
                                refeicao.Observations = reader["Observations"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Observations"]);
                                refeicao.CodigoEntidade = reader["CodigoEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoEntidade"]);
                                refeicao.NomeEntidade = reader["NomeEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["NomeEntidade"]);
                                refeicao.ApelidoEntidade = reader["ApelidoEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["ApelidoEntidade"]);
                                refeicao.TipoEntidade = reader["TipoEntidade"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["TipoEntidade"]);
                                refeicao.TipoDocFP = reader["TipoDocFP"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["TipoDocFP"]);
                                refeicao.SerieFP = reader["SerieFP"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["SerieFP"]);
                                refeicao.NumDocFP = reader["NumDocFP"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumDocFP"]);
                                refeicao.ValorPendente = reader["ValorPendente"].GetType() == typeof(DBNull) ? 0 : Convert.ToDouble(reader["ValorPendente"]);
                                ListaRefeicoes.Add(refeicao);
                            }
                        }
                    }
                }
                return ListaRefeicoes;
            }
            catch (Exception ex)
            {
                Logs.Erro("CarregaEstados", ex);
                return new List<Refeicao>();
            }
        }

        public static List<Refeicao> GetRefeicoesData(DateTime dataDe, DateTime dataAte)
        {
            try
            {
                List<Refeicao> ListaRefeicoes = new List<Refeicao>();
                using (var connection = new SqlConnection(Sessao.SQLServerConnectionString))
                {
                    connection.Open();

                    string sql = string.Format(@"SELECT  R.Codigo AS CodigoReserva, R.LinhaReserva AS LinhaReserva, R.DataCriacao AS DataCriacao, R.CheckIn AS DataCheckIn, R.HoraChegada AS HoraChegada, R.CheckOut AS DataCheckOut, R.nrnoites AS NumeroNoites, R.HoraSaida AS HoraPartida, RA.DataInicio AS DataInicioAlojamento, RA.DataFim AS DataFimAlojamento, RA.Alojamento AS CodigoAlojamento, RA.Categoria AS CategoriaAlojamento, RV.Categoria AS CategoriaPreco, R.NrADultos AS NumeroAdultos, R.NrCriancas AS NumeroCriancas, R.NrBercos AS NumeroBercos, R.NrQuartos AS NumeroAlojamentos, R.PrecoTotal AS Preco, R.TipoReserva AS CodigoEstadoReserva, CAST(R.Observacoes AS VARCHAR(255)) AS Observacoes, CAST(R.Voucher AS VARCHAR(30)) as Voucher, R.Funcionario AS Utilizador, T.Codigo AS CodigoHospede, isnull(T.Nome,isnull(REH.nome,'')) AS NomeHospede, isnull(T.Apelido,isnull(REH.apelido,'')) AS ApelidoHospede, isnull(T.Pais,'') AS CodigoPais, PS.Nome AS Pais, 
                                                         P.Codigo AS CodigoPackage, P.Descricao as DescricaoPackage, isnull(RF.Data,R.CheckIn) AS DataRefeicao, SUM(CASE WHEN (RF.ExtraMeal <> 1 OR RF.PequenoAlmoco = 0)  THEN ISNULL(RF.PequenoAlmoco,0) ELSE ISNULL(RF.BreakFastExtraNumber, 0) END)  AS PequenoAlmoco, SUM(CASE WHEN (RF.ExtraMeal <> 1 OR RF.Almoco =0)  THEN ISNULL(RF.Almoco,0) ELSE ISNULL(RF.LunchExtraNumber, 0) END)  AS Almoco, SUM(CASE WHEN (RF.ExtraMeal <> 1 OR RF.Jantar =0) THEN ISNULL(RF.Jantar,0) ELSE ISNULL(RF.DinnerExtraNumber, 0) END) AS Jantar, MAX(ISNULL(RF.Observations,'')) AS Observations, E.Codigo as CodigoEntidade, isnull(T2.Nome,'') as NomeEntidade, isnull(T2.Apelido,'') as ApelidoEntidade, isnull(E.Tipo,0) as TipoEntidade, R.Tipodoc AS TipoDocFP, R.Serie AS SerieFP, R.Numdoc AS NumDocFP, 0 AS ValorPendente
                                                FROM whotreservas AS R  
                                                INNER JOIN whotreservasalojamentos AS RA ON (R.Unidade=RA.Unidade and R.Codigo=RA.codigoreserva and R.LinhaReserva=RA.LinhaReserva)  INNER JOIN whotReservasRefeicoes as RF ON R.Unidade=RF.Unidade and R.Codigo=RF.CodigoReserva and R.LinhaReserva=RF.LinhaReserva  INNER JOIN whotreservasvaloresextra AS RV ON (RF.Unidade=RV.Unidade and RF.CodigoReserva=RV.codigoreserva and RF.LinhaReserva=RV.LinhaReserva and RV.tipo=2 AND RV.Datainicio<=RF.Data AND (RV.DataFim>RF.Data OR (RV.DataFim=R.Checkout AND RV.DataFim=RF.Data)))  LEFT JOIN whotpackages AS P ON (RV.Unidade=P.Unidade) AND (RV.Codigo=P.Codigo)  LEFT JOIN whotreservasentidades AS REH ON (R.unidade=REH.unidade AND R.codigo=REH.codigoreserva AND R.linhareserva=REH.linhareserva AND REH.IsHospedePrincipal=1)  LEFT JOIN wgcterceiros AS T ON (R.CodigoHospede=T.Codigo)  LEFT JOIN wgcPaises AS PS ON (T.Pais=PS.Codigo)  LEFT JOIN whotreservasentidades AS E ON (R.Unidade=E.Unidade) AND (R.Codigo=E.CodigoReserva) AND (R.LinhaReserva=E.LinhaReserva) AND (E.IsEntidadePrincipal=1)  LEFT JOIN wgcterceiros AS T2 ON (E.Codigo=T2.Codigo)
                                                WHERE ISNULL(e.Tipo, 0)<>1 AND ISNULL(e.Tipo, 0)<>2 AND R.Dividida=0 AND RF.Data>='" + dataDe.ToString("yyyyMMdd") + @"' AND RF.Data<='" + dataAte.ToString("yyyyMMdd") + @"' AND RA.DataInicio<='" + dataAte.ToString("yyyyMMdd") + @"' AND RA.DataFim>='" + dataDe.ToString("yyyyMMdd") + @"' AND R.TipoReserva in ('RSV','CKI','CKO') AND (RF.PequenoAlmoco=1 or RF.Almoco=1 or RF.Jantar=1)
                                                GROUP BY R.Unidade, R.Codigo, R.LinhaReserva, R.DataCriacao, R.CheckIn, R.HoraChegada, R.CheckOut, R.nrnoites, R.HoraSaida, RA.DataInicio, RA.DataFim, RA.Alojamento, RA.Categoria, RV.Categoria, R.NrADultos, R.NrCriancas, R.NrBercos, R.NrQuartos, R.PrecoTotal, R.TipoReserva, CAST(R.Observacoes AS VARCHAR(255)), CAST(R.Voucher AS VARCHAR(30)), R.Funcionario, T.Codigo, ISNULL(T.Nome, isnull(REH.nome, '')), ISNULL(T.Apelido, isnull(REH.apelido, '')), isnull(T.Pais, ''), PS.Nome, P.Codigo, P.Descricao, isnull(RF.Data, R.CheckIn), E.Codigo, isnull(T2.Nome, ''), isnull(T2.Apelido, ''), isnull(E.Tipo, 0), R.Tipodoc, R.Serie, R.Numdoc, RF.Data, R.Quarto
                                                ORDER BY RF.Data, R.Quarto, R.Codigo, R.LinhaReserva, RA.DataInicio");

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var refeicao = new Refeicao();
                                refeicao.CodigoReserva = reader["CodigoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoReserva"]);
                                refeicao.LinhaReserva = reader["LinhaReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["LinhaReserva"]);
                                refeicao.DataCriacao = reader["DataCriacao"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCriacao"]);
                                refeicao.DataCheckIn = reader["DataCheckIn"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCheckIn"]);
                                refeicao.HoraDeChegada = reader["HoraChegada"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraChegada"]);
                                refeicao.DataCheckOut = reader["DataCheckOut"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCheckOut"]);
                                refeicao.NumeroNoites = reader["NumeroNoites"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroNoites"]);
                                refeicao.HoraPartida = reader["HoraPartida"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraPartida"]);
                                refeicao.DataInicioAlojamento = reader["DataInicioAlojamento"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataInicioAlojamento"]);
                                refeicao.DataFimAlojamento = reader["DataFimAlojamento"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataFimAlojamento"]);
                                refeicao.CodigoAlojamento = reader["CodigoAlojamento"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoAlojamento"]);
                                refeicao.CategoriaAlojamento = reader["CategoriaAlojamento"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CategoriaAlojamento"]);
                                refeicao.CategoriaPreco = reader["CategoriaPreco"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CategoriaPreco"]);
                                refeicao.NumeroAdultos = reader["NumeroAdultos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroAdultos"]);
                                refeicao.NumeroCriancas = reader["NumeroCriancas"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroCriancas"]);
                                refeicao.NumeroBercos = reader["NumeroBercos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroBercos"]);
                                refeicao.NumeroAlojamentos = reader["NumeroAlojamentos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroAlojamentos"]);
                                refeicao.Preco = reader["Preco"].GetType() == typeof(DBNull) ? 0 : Convert.ToDouble(reader["Preco"]);
                                refeicao.CodigoEstadoReserva = reader["CodigoEstadoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoEstadoReserva"]);
                                refeicao.Observacoes = reader["Observacoes"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Observacoes"]);
                                refeicao.Voucher = reader["Voucher"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Voucher"]);
                                refeicao.CodigoHospede = reader["CodigoHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoHospede"]);
                                refeicao.NomeHospede = reader["NomeHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["NomeHospede"]);
                                refeicao.ApelidoHospede = reader["ApelidoHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["ApelidoHospede"]);
                                refeicao.CodigoPais = reader["CodigoPais"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoPais"]);
                                refeicao.Pais = reader["Pais"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Pais"]);
                                refeicao.CodigoPackage = reader["CodigoPackage"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoPackage"]);
                                refeicao.DescricaoPackage = reader["DescricaoPackage"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["DescricaoPackage"]);
                                refeicao.DataRefeicao = reader["DataRefeicao"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataRefeicao"]);
                                refeicao.PequenoAlmoco = reader["PequenoAlmoco"].GetType() == typeof(DBNull) ? false : Convert.ToBoolean(reader["PequenoAlmoco"]);
                                refeicao.Almoco = reader["Almoco"].GetType() == typeof(DBNull) ? false : Convert.ToBoolean(reader["Almoco"]);
                                refeicao.Jantar = reader["Jantar"].GetType() == typeof(DBNull) ? false : Convert.ToBoolean(reader["Jantar"]);
                                refeicao.Observations = reader["Observations"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Observations"]);
                                refeicao.CodigoEntidade = reader["CodigoEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoEntidade"]);
                                refeicao.NomeEntidade = reader["NomeEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["NomeEntidade"]);
                                refeicao.ApelidoEntidade = reader["ApelidoEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["ApelidoEntidade"]);
                                refeicao.TipoEntidade = reader["TipoEntidade"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["TipoEntidade"]);
                                refeicao.TipoDocFP = reader["TipoDocFP"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["TipoDocFP"]);
                                refeicao.SerieFP = reader["SerieFP"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["SerieFP"]);
                                refeicao.NumDocFP = reader["NumDocFP"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumDocFP"]);
                                refeicao.ValorPendente = reader["ValorPendente"].GetType() == typeof(DBNull) ? 0 : Convert.ToDouble(reader["ValorPendente"]);
                                ListaRefeicoes.Add(refeicao);
                            }
                        }
                    }
                }
                return ListaRefeicoes;
            }
            catch (Exception ex)
            {
                Logs.Erro("CarregaEstados", ex);
                return new List<Refeicao>();
            }
        }
    }
}
