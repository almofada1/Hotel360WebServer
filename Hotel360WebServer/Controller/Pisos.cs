using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel360WebServer.Models;

namespace Hotel360WebServer.Controller
{
    class Pisos
    {
        public static List<Piso> ListaPisos;

        public static bool CarregaPisos()
        {
            try
            {
                ListaPisos = new List<Piso>();

                using (var connection = new SqlConnection(Sessao.SQLServerConnectionString))
                {
                    connection.Open();

                    string sql = string.Format(@"select [Codigo],
                                                    [Descricao] from whotpisos");

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var piso = new Piso();
                                piso.Codigo = reader["codigo"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["codigo"]);
                                piso.Descricao = reader["descricao"].GetType() == typeof(DBNull) ? "" : Convert.ToString(reader["descricao"]);
                                ListaPisos.Add(piso);
                            }
                        }
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Logs.Erro("CarregaPisos",ex);
                return false;
            }
            

        }
    }
}
