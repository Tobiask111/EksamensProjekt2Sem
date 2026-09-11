using Core;
using ExcelDataReader; // Bruges til at læse Excel-filer
using System.IO;
using System.Text;
namespace Server.Service
{
    // Service-klasse der konverterer Excel-data til ProjectHour
    public class WorkerConverter
    {
        // Konverterer Excel-stream til liste af ProjectHour
        public static List<ProjectHour> Convert(Stream stream)
        {
            // Registrerer encoding provider
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Opretter Excel reader til .xlsx filer
            IExcelDataReader reader =  ExcelReaderFactory.CreateOpenXmlReader(stream);

            // Læser Excel-data ind i et DataSet
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false // Bruger ikke første række som header
                }
            });
            // Liste der skal indeholde arbejdstimer
            List<ProjectHour> result = new();
            int row_no = 1;

            // Gennemløber alle rækker i Excel-arket
            while (row_no < ds.Tables[0].Rows.Count)
            {
                var row = ds.Tables[0].Rows[row_no];
                ProjectHour p = new ProjectHour();
                p.Medarbejder = row[0].ToString();
                p.Dato = DateTime.Parse(row[1].ToString());
                p.Stoptid = DateTime.Parse(row[2].ToString());
                p.Timer = Decimal.Parse(row[5].ToString());
                p.Type = row[6].ToString();
                p.Kostpris = Decimal.Parse(row[8].ToString());
                // Koden læser data fra bestemte kolonner i Excel og gemmer dem i et ProjectHour-objekt.

                // Tilføjer arbejdstimen til listen
                result.Add(p);
                // Logger arbejdstimen til konsollen
                Console.WriteLine($"dato= {p.Dato}, stop tid = {p.Stoptid} Timer = {p.Timer}, Type = {p.Type}  kostpris = {p.Kostpris}");
                // Går videre til næste række
                row_no++;

            }
            // Returnerer listen med arbejdstimer
            return result;

        }
    }
}
