using Hotel360InteractiveServer.Models;
using Microsoft.Data.SqlClient;

namespace Hotel360InteractiveServer.Controller
{
    class CheckOuts
    {
        public static List<CheckOut> ListaCheckOuts;

        public static bool CarregaCheckOutDeHoje()
        {
            try
            {
                using (var connection = new SqlConnection(Sessao.SQLServerConnectionString))
                {
                    connection.Open();

                    string sql = string.Format(@"SELECT R.Unidade AS CodigoUnidade, R.Codigo AS CodigoReserva, R.LinhaReserva AS LinhaReserva, R.DataCriacao AS DataCriacao, R.CheckIn AS DataCheckIn, R.HoraChegada AS HoraChegada, R.CheckOut AS DataCheckOut, R.nrnoites AS NumeroNoites, R.HoraSaida AS HoraPartida, RA.DataInicio AS DataInicioAlojamento, RA.DataFim AS DataFimAlojamento, RA.Alojamento AS CodigoAlojamento, RA.Categoria AS CategoriaAlojamento, RV.Categoria AS CategoriaPreco, R.NrADultos AS NumeroAdultos, R.NrCriancas AS NumeroCriancas, R.NrBercos AS NumeroBercos, R.NrQuartos AS NumeroAlojamentos, R.PrecoTotal AS Preco, R.TipoReserva AS CodigoEstadoReserva, R.Observacoes AS Observacoes, R.Voucher AS Voucher, R.Funcionario AS Utilizador, T.Codigo AS CodigoHospede, isnull(T.Nome,isnull(REH.nome,'')) AS NomeHospede, isnull(T.Apelido,isnull(REH.apelido,'')) AS ApelidoHospede, isnull(T.Pais,'') AS CodigoPais, PS.Nome AS Pais, P.Codigo AS CodigoPackage, P.Descricao as DescricaoPackage, E.Codigo as CodigoEntidade, isnull(T2.Nome,'') as NomeEntidade, isnull(T2.Apelido,'') as ApelidoEntidade, isnull(E.Tipo,0) as TipoEntidade, R.Tipodoc AS TipoDocFP, R.Serie AS SerieFP, R.Numdoc AS NumDocFP, isnull(VLP.ValorPendente,0) AS ValorPendente
                                                    FROM whotreservas AS R  INNER JOIN whotreservasalojamentos AS RA ON (R.Unidade=RA.Unidade and R.Codigo=RA.codigoreserva and R.LinhaReserva=RA.LinhaReserva AND R.Checkout=RA.DataFim)  INNER JOIN whotreservasvaloresextra AS RV ON (RA.Unidade=RV.Unidade and RA.CodigoReserva=RV.codigoreserva and RA.LinhaReserva=RV.LinhaReserva and RV.tipo=2 AND RV.Datainicio<=RA.Datainicio AND (RV.DataFim>RA.Datainicio OR (RV.DataInicio=RV.DataFim AND RV.DataFim=RA.Datainicio)))  LEFT JOIN whotpackages AS P ON (RV.Unidade=P.Unidade) AND (RV.Codigo=P.Codigo)  LEFT JOIN whotreservasentidades AS REH ON (R.unidade=REH.unidade AND R.codigo=REH.codigoreserva AND R.linhareserva=REH.linhareserva AND REH.IsHospedePrincipal=1)  LEFT JOIN wgcterceiros AS T ON (R.CodigoHospede=T.Codigo)  LEFT JOIN wgcPaises AS PS ON (T.Pais=PS.Codigo)  LEFT JOIN whotreservasentidades AS E ON (R.Unidade=E.Unidade) AND (R.Codigo=E.CodigoReserva) AND (R.LinhaReserva=E.LinhaReserva) AND (E.IsEntidadePrincipal=1)  LEFT JOIN wgcterceiros AS T2 ON (E.Codigo=T2.Codigo)  LEFT JOIN (SELECT C.unidade, C.reserva, C.linhareserva, SUM(CL.ValorLinha) as ValorPendente
                                                    FROM whotcontas as C  INNER JOIN whotcontaslinhas as CL ON C.tipodoc=CL.tipodoc and C.serie=CL.serie and C.numdoc=CL.numdoc
                                                     WHERE C.Estado=0 AND isnull(CL.qtdfacturada,0)=0 AND isnull(CL.linhaestorno,-1)=-1 AND isnull(CL.motivoanulacao,'')=''
                                                     GROUP BY C.unidade, C.reserva, C.linhareserva

                                                    ) AS VLP ON (R.Unidade=VLP.Unidade) AND (R.Codigo=VLP.Reserva) AND (R.LinhaReserva=VLP.LinhaReserva)
                                                     WHERE ISNULL(e.Tipo, 0)<>1 AND ISNULL(e.Tipo, 0)<>2 AND R.Dividida=0 AND R.CheckOut>='{0}' AND R.CheckOut<='{0}' AND R.TipoReserva in ('CKI','RSV')

                                                     ORDER BY R.Checkout, R.Quarto, R.Codigo, R.LinhaReserva, RA.DataInicio", DateTime.Now.Date.ToString("yyyyMMdd"));

                    List<CheckOut> listaCheckOutHoje = new List<CheckOut>();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CheckOut cko = new CheckOut();
                                cko.CodigoReserva = reader["CodigoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoReserva"]);
                                cko.LinhaReserva = reader["LinhaReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["LinhaReserva"]);

                                cko.DataCriacao = reader["DataCriacao"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCriacao"]);
                                cko.HoraChegada = reader["HoraChegada"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraChegada"]);
                                cko.DataCheckIn = reader["DataCheckIn"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCheckIn"]);
                                cko.DataCheckOut = reader["DataCheckOut"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["DataCheckOut"]);

                                cko.NumeroNoites = reader["NumeroNoites"].GetType() == typeof(DBNull) ? 1 : Convert.ToInt32(reader["NumeroNoites"]);
                                cko.HoraPartida = reader["HoraPartida"].GetType() == typeof(DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["HoraPartida"]);
                                cko.CodigoAlojamento = reader["CodigoAlojamento"].GetType() == typeof(DBNull) ? "" : reader["CodigoAlojamento"].ToString();
                                cko.CategoriaAlojamento = reader["CategoriaAlojamento"].GetType() == typeof(DBNull) ? "" : reader["CategoriaAlojamento"].ToString();
                                cko.CategoriaPreco = reader["CategoriaPreco"].GetType() == typeof(DBNull) ? "" : reader["CategoriaPreco"].ToString();

                                cko.NumeroAdultos = reader["NumeroAdultos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroAdultos"]);
                                cko.NumeroCriancas = reader["NumeroCriancas"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroCriancas"]);
                                cko.NumeroBercos = reader["NumeroBercos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroBercos"]);
                                cko.NumeroAlojamentos = reader["NumeroAlojamentos"].GetType() == typeof(DBNull) ? 0 : Convert.ToInt32(reader["NumeroAlojamentos"]);

                                cko.Preco = reader["Preco"].GetType() == typeof(DBNull) ? 0 : float.Parse(reader["Preco"].ToString());

                                cko.CodigoEstadoReserva = reader["CodigoEstadoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoEstadoReserva"]);
                                cko.Observacoes = reader["Observacoes"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["Observacoes"]);
                                cko.NomeHospede = reader["NomeHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["NomeHospede"]);
                                cko.CodigoEstadoReserva = reader["CodigoEstadoReserva"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoEstadoReserva"]);
                                cko.ApelidoHospede = reader["ApelidoHospede"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["ApelidoHospede"]);
                                cko.CodigoPais = reader["CodigoPais"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoPais"]);
                                cko.CodigoPackage = reader["CodigoPackage"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["CodigoPackage"]);
                                cko.DescricaoPackage = reader["DescricaoPackage"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["DescricaoPackage"]);
                                cko.NomeEntidade = reader["NomeEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["NomeEntidade"]);
                                cko.ApelidoEntidade = reader["ApelidoEntidade"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["ApelidoEntidade"]);
                                listaCheckOutHoje.Add(cko);
                            }
                        }
                    }
                    ListaCheckOuts = listaCheckOutHoje;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logs.Erro("CarregaCheckOutDeHoje", ex);
                return false;
            }
        }
    }
}
