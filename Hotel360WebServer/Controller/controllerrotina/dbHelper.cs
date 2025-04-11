using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel360WebServer.Models;


namespace Hotel360WebServer.Controller
{
    class dbHelper
    {
        public static bool AlteraEstadoQuartoAsair(CheckOut cko)
        {
            try
            {
                //Logs.Info("INIT: AlteraEstadoQuarto :" + cko.CodigoAlojamento + "-> " + Properties.Settings.Default.EstadoAsair);
                using (var connection = new SqlConnection(Sessao.SQLServerConnectionString))
                {
                    connection.Open();

                    //using (var cmd = new SqlCommand())
                    //{

                    //    cmd.CommandText =
                    //        string.Format(@"UPDATE whotalojamento SET estado = '{1}' WHERE codigo = '{0}'", cko.CodigoAlojamento, Properties.Settings.Default.EstadoAsair);

                    //    cmd.CommandType = CommandType.Text;
                    //    cmd.Connection = connection;

                    //    var result = cmd.ExecuteNonQuery();
                    //}
                }
                return true;

            }
            catch (Exception ex)
            {
                Logs.Erro("Erro: AlteraEstadoQuartoAsair : " + ex.Message.ToString());
                return false;
            }
        }
    }
}
