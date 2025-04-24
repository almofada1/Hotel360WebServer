using System.Data;
using Microsoft.Data.SqlClient;

namespace Hotel360InteractiveServer.Controller
{
    class Sessao
    {
        public static string SQLServerConnectionString;
        public static string ServerElement;
        public static string Id;
        public static string Password;


        //static Sessao()
        //{
        //    var wintouchConfig = Properties.Settings.Default.DiretoriaSGW.TrimEnd('\\') + "\\Wintouch.config";
        //    var doc = XDocument.Load(wintouchConfig);

        //    var serverElement = doc.Element("wintouch").Element("settings").Element("setting").Element("server");

        //    var server = serverElement.Element("name").Attribute("value").Value;
        //    var id = serverElement.Element("user").Attribute("value").Value;
        //    var password = serverElement.Element("password").Attribute("value").Value;

        //    ServerElement = server;
        //    Id = id;
        //    Password = password;


        //    SQLServerConnectionString =
        //       string.Format("server={0};user id={1};password={2};database={3};Trusted_Connection=no;Connection Timeout=8",
        //            server,
        //            id,
        //            password,
        //            Properties.Settings.Default.Database);
        //}

        public static DateTime DataServidorSQL()
        {
            try
            {
                return (DateTime)Sessao.ExecutaConsulta("SELECT getdate()");
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        public static object ExecutaConsulta(string comando)
        {

            object result = null;
            using (var sqlConnection1 = new SqlConnection(SQLServerConnectionString))
            {
                sqlConnection1.Open();

                using (var cmd = new SqlCommand())
                {

                    cmd.CommandText = comando;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;

                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }

        public static bool TestarLigacao()
        {
            SqlConnection Conn = new SqlConnection();
            try
            {
                Conn = new SqlConnection(SQLServerConnectionString);
                Conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }
}
