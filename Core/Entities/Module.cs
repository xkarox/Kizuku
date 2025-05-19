using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Module : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    
    
    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();

}