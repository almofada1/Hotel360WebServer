using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel360InteractiveServer.Controller;
using Hotel360InteractiveServer.Models;

namespace Hotel360InteractiveServer.Controller
{
    class CheckInsRotina
    {
        public static  List<CheckIn> ListaCheckIns;

        public static bool CarregaCheckinsDeHoje()
        {
            try
            {
                using (var connection = new SqlConnection(Sessao.SQLServerConnectionString))
                {
                    connection.Open();

                    string sql = string.Format(@"SELECT  R.Codigo AS CodigoReserva, R.LinhaReserva AS LinhaReserva, R.DataCriacao AS DataCriacao, R.CheckIn AS DataCheckIn, R.HoraChegada AS HoraChegada, R.CheckOut AS DataCheckOut, R.nrnoites AS NumeroNoites, R.HoraSaida AS HoraPartida, RA.Alojamento AS CodigoAlojamento, RA.Categoria AS CategoriaAlojamento, RV.Categoria AS CategoriaPreco, R.NrADultos AS NumeroAdultos, R.NrCriancas AS NumeroCriancas, R.NrBercos AS NumeroBercos, R.NrQuartos AS NumeroAlojamentos, R.PrecoTotal AS Preco, R.TipoReserva AS CodigoEstadoReserva, R.Observacoes AS Observacoes, isnull(T.Nome,isnull(REH.nome,'')) AS NomeHospede, isnull(T.Apelido,isnull(REH.apelido,'')) AS ApelidoHospede, isnull(T.Pais,'') AS CodigoPais, P.Codigo AS CodigoPackage, P.Descricao as DescricaoPackage, isnull(T2.Nome,'') as NomeEntidade, isnull(T2.Apelido,'') as ApelidoEntidade
                                                 FROM whotreservas AS R  INNER JOIN whotreservasalojamentos AS RA ON (R.Unidade=RA.Unidade and R.Codigo=RA.codigoreserva and R.LinhaReserva=RA.LinhaReserva)  INNER JOIN whotreservasvaloresextra AS RV ON (RA.Unidade=RV.Unidade and RA.CodigoReserva=RV.codigoreserva and RA.LinhaReserva=RV.LinhaReserva and RV.tipo=2 AND RV.Datainicio<=RA.Datainicio AND (RV.DataFim>RA.Datainicio OR (RV.DataInicio=RV.DataFim AND RV.DataFim=RA.Datainicio)))  LEFT JOIN whotpackages AS P ON (RV.Unidade=P.Unidade) AND (RV.Codigo=P.Codigo)  LEFT JOIN whotreservasentidades AS REH ON (R.unidade=REH.unidade AND R.codigo=REH.codigoreserva AND R.linhareserva=REH.linhareserva AND REH.IsHospedePrincipal=1)  LEFT JOIN wgcterceiros AS T ON (R.CodigoHospede=T.Codigo)  LEFT JOIN wgcPaises AS PS ON (T.Pais=PS.Codigo)  LEFT JOIN whotreservasentidades AS E ON (R.Unidade=E.Unidade) AND (R.Codigo=E.CodigoReserva) AND (R.LinhaReserva=E.LinhaReserva) AND (E.IsEntidadePrincipal=1)  LEFT JOIN wgcterceiros AS T2 ON (E.Codigo=T2.Codigo)
                                                 WHERE ISNULL(e.Tipo, 0)<>1 AND ISNULL(e.Tipo, 0)<>2 AND R.CheckIn='20190701' AND R.TipoReserva in ('RSV','ECF','LTE','OVB','CKI') and r.quarto is not null and r.quarto <> ''
                                                 ORDER BY R.Quarto, R.Codigo, R.LinhaReserva, RA.DataInicio");

                    List<CheckIn> listaCheckinHoje = new List<CheckIn>();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CheckIn cki = new CheckIn();
                                cki.CodigoReserva = reader["CodigoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoReserva"]);
                                cki.LinhaReserva = reader["LinhaReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["LinhaReserva"]);

                                cki.DataCriacao = reader["DataCriacao"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCriacao"]);
                                cki.HoraChegada = reader["HoraChegada"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraChegada"]);
                                cki.DataCheckIn = reader["DataCheckIn"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCheckIn"]);
                                cki.DataCheckOut = reader["DataCheckOut"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCheckOut"]);

                                cki.NumeroNoites = reader["NumeroNoites"].GetType() == typeof(DBNull) ? 1 : Convert.ToInt32(reader["NumeroNoites"]);
                                cki.HoraPartida = reader["HoraPartida"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraPartida"]);
                                cki.CodigoAlojamento = reader["CodigoAlojamento"].GetType() == typeof(DBNull) ? "" : reader["CodigoAlojamento"].ToString();
                                cki.CategoriaAlojamento = reader["CategoriaAlojamento"].GetType() == typeof(DBNull) ? "" : reader["CategoriaAlojamento"].ToString();
                                cki.CategoriaPreco = reader["CategoriaPreco"].GetType() == typeof(DBNull) ? "" : reader["CategoriaPreco"].ToString();

                                cki.NumeroAdultos = reader["NumeroAdultos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroAdultos"]);
                                cki.NumeroCriancas = reader["NumeroCriancas"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroCriancas"]);
                                cki.NumeroBercos = reader["NumeroBercos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroBercos"]);
                                cki.NumeroAlojamentos = reader["NumeroAlojamentos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroAlojamentos"]);

                                cki.Preco = reader["Preco"].GetType() == typeof(DBNull) ? 0 : float.Parse(reader["Preco"].ToString());

                                cki.CodigoEstadoReserva = reader["CodigoEstadoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoEstadoReserva"]);
                                cki.Observacoes = reader["Observacoes"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Observacoes"]);
                                cki.NomeHospede = reader["NomeHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["NomeHospede"]);
                                cki.CodigoEstadoReserva = reader["CodigoEstadoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoEstadoReserva"]);
                                cki.ApelidoHospede = reader["ApelidoHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["ApelidoHospede"]);
                                cki.CodigoPais = reader["CodigoPais"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoPais"]);
                                cki.CodigoPackage = reader["CodigoPackage"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoPackage"]);
                                cki.DescricaoPackage = reader["DescricaoPackage"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["DescricaoPackage"]);
                                cki.NomeEntidade = reader["NomeEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["NomeEntidade"]);
                                cki.ApelidoEntidade = reader["ApelidoEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["ApelidoEntidade"]);
                                listaCheckinHoje.Add(cki);
                            }
                        }
                    }
                    ListaCheckIns = listaCheckinHoje;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logs.Erro("CarregaCheckinsDeHoje", ex);
                return false;
            }

        }
    }
}
