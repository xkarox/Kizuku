namespace Core.Classes;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual List<Topic> Topics { get; set; } = new List<Topic>();
}

