using System.Drawing;
using Hotel360InteractiveServer.Data;
using Hotel360InteractiveServer.Models;

namespace Hotel360InteractiveServer.Controller
{
    public class QuartosController
    {
        public static List<Quarto> ListaQuartos { get; set; } = new List<Quarto>();

        public static async Task<bool> CarregaQuartosAsync(ApplicationDbContext dbContext)
        {
            try
            {
                string sql = @"
                    SELECT 
                        [codigo] AS Codigo,
                        [descricao] AS Descricao,
                        [categoriaalojamento] AS Categoria,
                        [extensao],
                        [lotacao],
                        [numcamasextra] AS Numcamaextra,
                        [numberco] AS Numberco,
                        [piso],
                        [observacoes] AS Observarvacoes,
                        [estado],
                        [AcessivelDeficientes]
                    FROM [dbo].[whotalojamento]";

                var quartosResult = await dbContext.QueryAsync<Quarto>(sql);
                ListaQuartos = quartosResult.ToList();

                return true;
            }
            catch (Exception ex)
            {
                Logs.Erro("Erro: CarregaQuartos : " + ex.Message);
                return false;
            }
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)new Bitmap(imgToResize, size);
        }

        public static async Task<string> AlteraEstadoQuartoAsync(ApplicationDbContext dbContext, Quarto quartoSel, EstadoQuarto novoEstado)
        {
            try
            {
                Logs.Info("INIT: AlteraEstadoQuarto: " + quartoSel.Codigo + " -> " + novoEstado.Codigo);

                string sql = @"
                    UPDATE whotalojamento 
                    SET estado = @Estado 
                    WHERE codigo = @Codigo";

                int result = await dbContext.ExecuteAsync(sql, new { Estado = novoEstado.Codigo, Codigo = quartoSel.Codigo });
                return result.ToString();
            }
            catch (Exception ex)
            {
                Logs.Erro("Erro: AlteraEstadoQuarto : " + ex.Message);
                throw;
            }
        }
        public static async Task<List<string>> GetRoomStateOptionsAsync(ApplicationDbContext dbContext)
        {
            string sql = "SELECT DISTINCT descricao FROM whotestadoquartos WHERE cdu_disponivelPortal = 1";
            var states = await dbContext.QueryAsync<string>(sql);
            var filteredStates = states.Where(state => !string.IsNullOrWhiteSpace(state)).ToList();
            return filteredStates;
        }

        public static async Task<Dictionary<string, Color>> GetStateColorsAsync(ApplicationDbContext dbContext)
        {
            string sql = "SELECT DISTINCT descricao, colorestado FROM whotestadoquartos";
            var results = await dbContext.QueryAsync<dynamic>(sql);
            var stateColors = new Dictionary<string, Color>();

            foreach (var row in results)
            {
                string state = row.descricao as string;
                if (string.IsNullOrEmpty(state))
                    continue;

                Color color = ColorTranslator.FromWin32(Convert.ToInt32(row.colorestado));

                stateColors[state] = color;
            }

            return stateColors;
        }

    }
}
