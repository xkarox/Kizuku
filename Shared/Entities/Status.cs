using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Status : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    
    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}