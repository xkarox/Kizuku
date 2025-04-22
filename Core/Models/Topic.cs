using Core.Enums;

namespace Core.Classes;

public class Topic
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public StudyStatus Status { get; set; } = StudyStatus.NotStarted;

    public int ModuleId { get; set; }
    public virtual Module Module { get; set; }

    public int? ParentTopicId { get; set; }
    public virtual Topic ParentTopic { get; set; }

    public virtual List<Topic> ChildrenTopics { get; set; } = new List<Topic>();
    
    public virtual List<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();

    public virtual List<Tag> Tags { get; set; } = new List<Tag>();
}