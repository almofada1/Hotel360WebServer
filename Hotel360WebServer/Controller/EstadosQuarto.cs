using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel360WebServer.Models;

namespace Hotel360WebServer.Controller
{
    class EstadosQuarto
    {

        public static  List<EstadoQuarto> ListaEstadosQuarto;

        public static bool CarregaEstados()
        {
            try
            {
                ListaEstadosQuarto = new List<EstadoQuarto>();
                using (var connection = new SqlConnection(Sessao.SQLServerConnectionString))
                {
                    connection.Open();

                    string sql = string.Format(@"SELECT [unidade]
                                              ,[codigo]
                                              ,[descricao]
                                              ,[codigocentral]
                                              ,[colorestado]
                                              ,[activo]
                                          FROM [dbo].[whotestadoquartos]");

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var estado = new EstadoQuarto();
                                estado.Codigo = reader["codigo"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["codigo"]);
                                estado.Descricao = reader["descricao"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["descricao"]);
                                estado.CodigoCentral = reader["codigocentral"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["codigocentral"]);
                                estado.CorEstado = reader["colorestado"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["colorestado"]);
                                estado.Ativo = reader["activo"].GetType() == typeof(DBNull) ? -1 : Convert.ToInt32(reader["activo"]);
                                ListaEstadosQuarto.Add(estado);
                            }
                        }
                    }
                }
                return true;
            } catch(Exception ex)
            {
                Logs.Erro("CarregaEstados", ex);
                return false;
            }
        }
    }
}
