using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel360InteractiveServer.Controller
{
    public class Logs
    {
        private static string ExMessage(Exception ex)
        {
            return Utils.ExMessage(ex);
        }

        public static void Erro(string msg, Exception ex)
        {
            Trace.TraceError("{0} - {1}:\r\n{2}", DateTime.Now.ToString("dd-MM-yyy HH:mm"), msg, ExMessage(ex));
            Trace.Close();
        }

        public static void Erro(string msg, string ex)
        {
            Trace.TraceError("{0} - {1}:\r\n{2}", DateTime.Now.ToString("dd-MM-yyy HH:mm"), msg, ex);
            Trace.Close();
        }

        public static void Erro(string msg)
        {
            Trace.TraceError("{0} - {1}", DateTime.Now.ToString("dd-MM-yyy HH:mm"), msg);
            Trace.Close();
        }

        public static void Aviso(string msg, Exception ex)
        {
            Trace.TraceWarning("{0} - {1}:\r\n{2}", DateTime.Now.ToString("dd-MM-yyy HH:mm"), msg, ExMessage(ex));
            Trace.Close();
        }

        public static void Aviso(string msg)
        {
            Trace.TraceWarning("{0} - {1}", DateTime.Now.ToString("dd-MM-yyy HH:mm"), msg);
            Trace.Close();
        }

        public static void Info(string msg, Exception ex)
        {
            Trace.TraceInformation("{0} - {1}:\r\n{2}", DateTime.Now.ToString("dd-MM-yyy HH:mm"), msg, ExMessage(ex));
            Trace.Close();
        }

        public static void Info(string msg)
        {
            Trace.TraceInformation("{0} - {1}", DateTime.Now.ToString("dd-MM-yyy HH:mm"), msg);
            Trace.Close();
        }

        public static void ZipFile(string origem, string destino)
        {
            using (FileStream sourceFile = File.OpenRead(origem))
            {
                using (FileStream destFile = File.Create(destino))
                {
                    using (var compStream = new BufferedStream(new GZipStream(destFile, CompressionMode.Compress)))
                    {

                        int theByte = sourceFile.ReadByte();
                        while (theByte != -1)
                        {
                            compStream.WriteByte((byte)theByte);
                            theByte = sourceFile.ReadByte();
                        }

                    }
                }
            }

        }


        public static void InicializaFicheiro(string rootPath, bool forca = false)
        {
            try
            {
                InicializaLog(rootPath);
                Info("A verificar tamanho do log");
                var logFile = rootPath + "Log.txt";
                var logFileInfo = new FileInfo(logFile);
                if (logFileInfo.Length > 1048576 || forca == true)
                {
                    Trace.Close();
                    var logBackup = String.Format("{0}Logs\\Log-{1:dd-MM-yyyy_HH_mm_ss}.zip", rootPath, DateTime.Now);
                    ZipFile(logFile, logBackup);
                    using (var sw = new StreamWriter(logFile))
                    {
                        sw.Write("");
                        sw.Close();
                    }
                    InicializaLog(rootPath);
                    Info("Backup e limpeza ao ficheiro de log bem sucedida");
                }
            }
            catch (Exception ex)
            {
                Erro("Não foi possivel fazer o backup e limpeza ao ficheiro de log", ex);
            }

        }

        private static void InicializaLog(string rootPath)
        {
            var path = rootPath + "Log.txt";
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new TextWriterTraceListener(path));
            Trace.IndentSize = 4;
            Trace.AutoFlush = true;
        }

    }
}
