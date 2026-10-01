using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Backend.Database.Entities;


public class AuditLog
{
    public ulong Id { get; set; }
    
    [Required]
    [MaxLength(1024)]
    public string EventType { get; set; } = string.Empty;
    
    [MaxLength(1024)]
    public string? UserName { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; }

    [Required] public JsonDocument AuditData { get; set; } = null!;
}