using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using Hotel360InteractiveServer.Controller;
using Hotel360InteractiveServer.Models;

namespace Hotel360InteractiveServer.ControllerServicos
{
    class ReservasServicos
    {
        public static List<ReservaServico> GetReservasServicosData(DateTime dataDe, DateTime dataAte)
        {
            try
            {
                List<ReservaServico> ListaReservaServico = new List<ReservaServico>();
                using (var connection = new SqlConnection(Sessao.SQLServerConnectionString))
                {
                    connection.Open(); 

                    string sql = string.Format(@"SELECT R2.*, R.Observacoes as Observacoes
                                                FROM whotreservas AS R  INNER JOIN (SELECT SUM(CASE WHEN isnull(E.tipo,255)=1 THEN 1 ELSE 0 END) AS nbercos, SUM(CASE WHEN isnull(E.tipo,255)=0 AND isnull(E.tipocama,0)=1 THEN 1 ELSE 0 END) AS ncamascasal, SUM(CASE WHEN isnull(E.tipo,255)=0 AND isnull(E.tipocama,0)=0 THEN 1 ELSE 0 END) AS ncamasind, R.Unidade AS Unidade, R.Codigo AS CodigoReserva, R.LinhaReserva AS LinhaReserva, R.TipoReserva AS EstadoReserva, R.NrAdultos AS NumeroAdultos, R.NrCriancas AS NumeroCriancas, R.NrBercos AS NumeroBebes, R.CheckIn AS CheckIn, R.CheckOut AS CheckOut, R.HoraChegada AS HoraChegada, R.HoraSaida AS HoraPartida, RA.DataInicio AS DataInicio, RA.DataFim AS DataFim, RA.Alojamento AS CodigoAlojamento, T1.Codigo AS CodigoHospede, IsNull(T1.Nome,'') AS NomeHospede, IsNull(T1.Apelido,'') AS ApelidoHospede, T2.Codigo AS CodigoGrupo, IsNull(T2.Nome,'') AS NomeGrupo, IsNull(T2.Apelido,'') AS ApelidoGrupo
                                                FROM whotreservas AS R  INNER JOIN whotreservasalojamentos AS RA ON (R.unidade=RA.unidade and R.codigo=RA.codigoreserva and R.linhareserva=RA.linhareserva)  LEFT JOIN wgcterceiros AS T1 ON (R.CodigoHospede=T1.Codigo)  LEFT JOIN wgcterceiros AS T2 ON (R.CodigoGrupo=T2.Codigo)  LEFT JOIN whotreservasvaloresextra AS RE ON (R.unidade=RE.unidade AND R.codigo=RE.codigoreserva AND R.linhareserva=RE.linhareserva AND RE.datainicio<='" + dataAte.ToString("yyyyMMdd") + @"' AND RE.datafim>='" + dataDe.ToString("yyyyMMdd") + @"' AND RE.tipo=3)  LEFT JOIN whotextrashotel AS E ON RE.codigo=E.codigo
                                                 WHERE R.Unidade='whotel' AND R.TipoReserva<>'CAN' AND R.TipoReserva<>'NEG' AND R.TipoReserva<>'NSW' AND RA.DataInicio<='" + dataAte.ToString("yyyyMMdd") + @"' AND RA.DataFim>='" + dataDe.ToString("yyyyMMdd") + @"'
                                                 GROUP BY R.Unidade, R.Codigo, R.LinhaReserva, R.TipoReserva, R.NrAdultos, R.NrCriancas, R.NrBercos, R.CheckIn, R.CheckOut, R.HoraChegada, R.HoraSaida, RA.DataInicio, RA.DataFim, RA.Alojamento, T1.Codigo, T1.Nome, T1.Apelido, T2.Codigo, T2.Nome, T2.Apelido

                                                ) as R2 ON R.unidade=R2.unidade and R.codigo=R2.CodigoReserva and R.LinhaReserva=R2.LinhaReserva
                                                 ORDER BY R2.CodigoAlojamento, R2.DataInicio, R2.DataFim");

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var RServico = new ReservaServico();
                                RServico.nBercos = reader["nBercos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["nBercos"]);
                                RServico.nCamasCasal = reader["nCamasCasal"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["nCamasCasal"]);
                                RServico.nCamasInd = reader["nCamasInd"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["nCamasInd"]);
                                RServico.CodigoReserva = reader["CodigoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoReserva"]);
                                RServico.LinhaReserva = reader["LinhaReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["LinhaReserva"]);
                                RServico.EstadoReserva = reader["EstadoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["EstadoReserva"]);
                                RServico.NumeroAdultos = reader["NumeroAdultos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroAdultos"]);
                                RServico.NumeroCriancas = reader["NumeroCriancas"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroCriancas"]);
                                RServico.NumeroBebes = reader["NumeroBebes"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroBebes"]);
                                RServico.CheckIn = reader["CheckIn"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["CheckIn"]);
                                RServico.HoraChegada = reader["HoraChegada"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraChegada"]);
                                RServico.CheckOut = reader["CheckOut"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["CheckOut"]);
                                RServico.HoraPartida = reader["HoraPartida"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraPartida"]);
                                RServico.DataFim = reader["DataFim"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataFim"]);
                                RServico.DataInicio = reader["DataInicio"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataInicio"]);
                                RServico.CodigoAlojamento = reader["CodigoAlojamento"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoAlojamento"]);
                                RServico.Observacoes = reader["Observacoes"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Observacoes"]);
                                RServico.NomeHospede = reader["NomeHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["NomeHospede"]);
                                RServico.ApelidoHospede = reader["ApelidoHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["ApelidoHospede"]);
                                ListaReservaServico.Add(RServico);
                            }
                        }
                    }
                }

                if(ListaReservaServico.Count() > 0)
                {
                    List<ServicoGovernanta> listaServicos = GetServicosData(dataDe, dataAte);
                    foreach (var servico in listaServicos)
                    {
                        List<ReservaServico> rs = ListaReservaServico.Where(X => X.CodigoReserva == servico.CodigoReserva && X.LinhaReserva == servico.LinhaReserva).ToList();
                        foreach (var s in rs)
                        {
                            s.listaServicos = s.listaServicos  + servico.DescricaoServico+ "; \n";
                        }
                    }
                }

                return ListaReservaServico;
            }
            catch (Exception ex)
            {
                Logs.Erro("GetReservasServicosData", ex);
                return new List<ReservaServico>();
            }
        }

        public static List<ServicoGovernanta> GetServicosData(DateTime dataDe, DateTime dataAte)
        {
            try
            {
                List<ServicoGovernanta> ListaServicos = new List<ServicoGovernanta>();
                using (var connection = new SqlConnection(Sessao.SQLServerConnectionString))
                {
                    connection.Open();

                    string sql = string.Format(@"SELECT R.Unidade AS Unidade, R.Codigo AS CodigoReserva, R.LinhaReserva AS LinhaReserva, RV.Codigo AS CodigoServico, SG.Descricao AS DescricaoServico
                                                FROM whotreservas AS R  INNER JOIN whotreservasvaloresextra AS RV ON (R.unidade=RV.unidade AND R.codigo=RV.codigoreserva AND R.linhareserva=RV.linhareserva AND RV.datainicio<='" + dataDe.ToString("yyyyMMdd") + @"' AND RV.datafim>='" + dataAte.ToString("yyyyMMdd") + @"' AND RV.tipo=4)  INNER JOIN whotServicosGovernantas AS SG ON (SG.Unidade=RV.Unidade AND SG.Codigo=RV.Codigo)
                                                 WHERE R.Unidade='whotel' AND R.TipoReserva<>'CAN' AND R.TipoReserva<>'NEG' AND R.TipoReserva<>'NSW' AND R.CheckIn<='" + dataAte.ToString("yyyyMMdd") + @"' AND R.CheckOut>='" + dataDe.ToString("yyyyMMdd") + @"' AND RV.DataInicio<='" + dataDe.ToString("yyyyMMdd") + @"' AND RV.DataFim>='" + dataAte.ToString("yyyyMMdd") + @"'
                                                 GROUP BY R.Unidade, R.Codigo, R.LinhaReserva, RV.Codigo, SG.Descricao
                                                 ORDER BY R.Codigo, R.LinhaReserva, RV.Codigo");

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var servicoGovernanta = new ServicoGovernanta();
                                servicoGovernanta.CodigoReserva = reader["CodigoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoReserva"]);
                                servicoGovernanta.LinhaReserva = reader["LinhaReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["LinhaReserva"]);
                                servicoGovernanta.CodigoServico = reader["CodigoServico"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoServico"]);
                                servicoGovernanta.DescricaoServico = reader["DescricaoServico"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["DescricaoServico"]);
                                ListaServicos.Add(servicoGovernanta);
                            }
                        }
                    }
                }
                return ListaServicos;
            }
            catch (Exception ex)
            {
                Logs.Erro("GetServicosData", ex);
                return new List<ServicoGovernanta>();
            }
        }


    }
}
