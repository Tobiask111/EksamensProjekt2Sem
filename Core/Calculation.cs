using System.Collections.Generic;

namespace Core
{
    public class Calculation
    {
        public int CalcId { get; set; }
        public int ProjectId { get; set; }

        // Stamdata
        public Project Project { get; set; }

        // Rå data (kan evt. udelades hvis du kun vil sende de grupperede, men vi beholder dem for en sikkerheds skyld)
        public List<ProjectHour> Hours { get; set; } = new();
        public List<ProjectMaterial> Materials { get; set; } = new();

        // NYT: Færdigsorterede lister fra Serveren
        public List<HourGroupDto> GroupedHours { get; set; } = new();
        public List<ProjectMaterial> GroupedMaterialsClientView { get; set; } = new(); // Kategoriseret (Belysning, Kabler...)
        public List<ProjectMaterial> GroupedMaterialsInternView { get; set; } = new(); // Leverandør/Navn

        // Beregninger
        public decimal TotalKostPrisMaterialer { get; set; }
        public decimal TotalPrisMaterialer { get; set; }
        public decimal TotalTimer { get; set; }
        public decimal TotalKostPrisTimer { get; set; }
        public decimal TotalPrisTimer { get; set; }

        // Samlet
        public decimal SamletKostPris => TotalKostPrisMaterialer + TotalKostPrisTimer;
        public decimal SamletTotalPris => TotalPrisMaterialer + TotalPrisTimer;
        public decimal Dækningsgrad => SamletTotalPris > 0 ? (Dækningsbidrag / SamletTotalPris) * 100 : 0;
        public decimal Dækningsbidrag => SamletTotalPris - SamletKostPris;

        public string Type { get; set; }
        public decimal TotalHours { get; set; }
    }

    // Lille hjælpeklasse til time-gruppering
    public class HourGroupDto
    {
        public string Type { get; set; } = "";
        public decimal Total { get; set; }
    }
}