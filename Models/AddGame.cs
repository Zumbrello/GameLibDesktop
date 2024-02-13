namespace GameLibDesktop.Models;

public class AddGame
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int IdDeveloper { get; set; }
    public int IdPublisher { get; set; }
    public string SystemRequestMin { get; set; }
    public string SystemRequestRec { get; set; }
    public string ReleaseDate { get; set; }
    public string MainImage { get; set; }
}