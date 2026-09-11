namespace Core;

public class Project
{
    public int ProjectId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public string ImageUrl { get; set; } = string.Empty;

    public int SvendTimePris { get; set; } = 572;

    public int LærlingTimePris { get; set; } = 395;

    public int KonsulentTimePris { get; set; } = 995;

    public int ArbejdsmandTimePris { get; set; } = 515;
    

}