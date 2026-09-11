namespace Core
{
    public class ProjectMaterial
    {
        public int MaterialsId { get; set; }
        public int ProjectId { get; set; }
        public string? Beskrivelse { get; set; }
        public decimal Kostpris { get; set; }
        public decimal Antal { get; set; }
        public decimal Total { get; set; }
        
        public string? Leverandør { get; set; }
        public decimal Avance { get; set; }
        public decimal Dækningsgrad { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}