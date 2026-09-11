namespace Core
{
    public class ProjectHour
    {
        public int HourId { get; set; }
        public int ProjectId { get; set; }
        public string? Medarbejder { get; set; }
        public DateTime? Dato { get; set; }
        public DateTime? Stoptid { get; set; }
        public decimal Timer { get; set; }
        public string? Type { get; set; }
        public decimal Kostpris { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}