using Core;
using ExcelDataReader; // Bruges til at læse excel filer
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Text;
namespace Server.Service
{
    public class MaterialConverter
    {
        // Konverterer Excel-stream til liste af ProjectMaterial
        public static List<ProjectMaterial> Convert(Stream stream)
        {
            // Registrerer encoding provider (kræves til ExcelDataReader)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Opretter Excel reader til .xlsx filer
            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            // Læser Excel-data ind i et DataSet
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false
                }
            });
            // Liste der skal indeholde materialer
            List<ProjectMaterial> result = new();
            int row_no = 1; // starter ved række 1 

            // Gennemløber alle rækker i Excel-arket
            while (row_no < ds.Tables[0].Rows.Count)
            {

                // Henter nuværende række
                var row = ds.Tables[0].Rows[row_no];
                ProjectMaterial p = new ProjectMaterial(); // Opretter nyt ProjectMaterial objekt
                p.Beskrivelse = row[1].ToString();
                p.Kostpris = Decimal.Parse(row[2].ToString());
                p.Antal = Decimal.Parse(row[4].ToString());
                p.Leverandør = row[8].ToString();
                p.Total = Decimal.Parse(row[17].ToString());
                p.Avance = Decimal.Parse(row[19].ToString()); // Læser værdier fra valgte kolonner og gemmer dem i objektet

                p.Dækningsgrad = string.IsNullOrEmpty(row[20].ToString()) ? 0 : decimal.Parse(row[20].ToString());

                // Tilføjer materialet til listen
                result.Add(p);
                // Logger materialet til konsollen
                Console.WriteLine($"Beskrivelse= {p.Beskrivelse}, Kostpris = {p.Kostpris} Antal = {p.Antal}, Total = {p.Total}  Avance = {p.Avance}, dækningsgrad = {p.Dækningsgrad}");
                // Går videre til næste række
                row_no++;

            }
            // Returnerer listen med materialer
            return result;

        }
    }
}
