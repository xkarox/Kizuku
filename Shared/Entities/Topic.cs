using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Entities;

public class Topic : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Guid? StatusId { get; set; }
    public virtual Status? Status { get; set; }
    
    public Guid ModuleId { get; set; }
    [JsonIgnore]
    public virtual Module Module { get; set; }
    
    public Guid? ParentTopicId { get; set; }
    [JsonIgnore]
    public virtual Topic? ParentTopic { get; set; }
    
    public virtual ICollection<Topic> SubTopics { get; set; } = new List<Topic>();
}