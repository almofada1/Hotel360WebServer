using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hotel360WebServer.Controller
{
    public class Utils
    {
        public static string CortaTextoPara35chars(string texto)
        {
            return texto != null && texto.Length < 35 ? texto : texto.Substring(32) + "...";
        }

        public static bool HumanStrComp(string a, string b)
        {
            try
            {
                if (a == b)
                {
                    return true;
                }
                else if (a == null || b == null)
                {
                    return false;
                }
                else
                {
                    return Regex.Replace(RemoveDiacritics(a.ToLower()), "[^a-zA-Z0-9]", "") == Regex.Replace(RemoveDiacritics(b.ToLower()), "[^a-zA-Z0-9]", "");
                }
            }
            catch (Exception ex)
            {
                Logs.Aviso("Erro em HumanStrComp", ex);
                return false;
            }
        }

        public static string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static string Codificar64X2(string TextoNatural)
        {
            if (string.IsNullOrEmpty(TextoNatural) == false)
            {
                byte[] DadosDescodificados = ASCIIEncoding.ASCII.GetBytes(TextoNatural);
                string ValorRetorno = Convert.ToBase64String(DadosDescodificados);
                DadosDescodificados = ASCIIEncoding.ASCII.GetBytes(ValorRetorno);
                return Convert.ToBase64String(DadosDescodificados);
            }
            return null;
        }

        public static string Descodificar64X2(string TextoCodificado)
        {
            if (string.IsNullOrEmpty(TextoCodificado) == false)
            {
                if (TextoCodificado == null)
                {
                    return null;
                }
                byte[] DadosCodificados = Convert.FromBase64String(TextoCodificado);
                string ValorRetorno = ASCIIEncoding.ASCII.GetString(DadosCodificados);
                DadosCodificados = Convert.FromBase64String(ValorRetorno);
                return ASCIIEncoding.ASCII.GetString(DadosCodificados);
            }
            return null;
        }

        public static string Encriptar(string texto)
        {
            if (string.IsNullOrEmpty(texto) == false)
            {

                return CipherUtility.Encrypt<TripleDESCryptoServiceProvider>(texto, "6545sfdg218§€€§@£§6344fgasdfbn43", "sdf323ffdf656468sdf4235ffª+´+~º");

            }
            return null;
        }

        public static string Desencriptar(string texto)
        {
            if (string.IsNullOrEmpty(texto) == false)
            {
                return CipherUtility.Decrypt<TripleDESCryptoServiceProvider>(texto, "6545sfdg218§€€§@£§6344fgasdfbn43", "sdf323ffdf656468sdf4235ffª+´+~º");
            }
            return null;
        }

        public static string ExMessage(Exception ex)
        {
            //if (ex.InnerException != null && ex.InnerException.InnerException != null)
            //{
            //    return ex.InnerException.InnerException.Message;
            //}
            //return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return ex.ToString();
        }

        public static void UploadFileToFtp(string user, string pass, string ftp, string filePath, string fileName, string newFileName)
        {
            try
            {
                Logs.Info("Inicia Upload de " + fileName);
                /* Create an FTP Request */
                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(ftp + "/" + newFileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                var ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */

                FileStream localFileStream = new FileStream(filePath + "\\" + fileName, FileMode.Open);
                /* Buffer for the Downloaded Data */
                int bufferSize = 2048;
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                Logs.Erro(ExMessage(ex));
                throw ex;

            }
            Logs.Info("Fim de Upload");
        }


    }

    public class CipherUtility
    {
        public static string Encrypt<T>(string value, string password, string salt)
           where T : SymmetricAlgorithm, new()
        {
            PasswordDeriveBytes rgb = new PasswordDeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        public static string Decrypt<T>(string text, string password, string salt)
           where T : SymmetricAlgorithm, new()
        {
            PasswordDeriveBytes rgb = new PasswordDeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(text)))
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
