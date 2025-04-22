namespace Core.Classes;

public class WasteTag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual List<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
}