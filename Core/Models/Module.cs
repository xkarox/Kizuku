namespace Core.Classes;

public class Module
{
    public int Id { get; set; }
    public string Name { get; set; } = "New Module";
    public string Description { get; set; } = "Description of your new module";

    public virtual List<Topic> Topics { get; set; } = new List<Topic>();
}