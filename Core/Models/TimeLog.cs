namespace Core.Classes;

public class TimeLog
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime Date { get; set; }

    public int TopicId { get; set; }
    public virtual Topic Topic { get; set; }

    public virtual List<WasteTag> WasteTags { get; set; } = new List<WasteTag>();
}