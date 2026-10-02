using System.ComponentModel.DataAnnotations;
using Backend.Database.Entities.Junctions;
using Protos.Role;

namespace Backend.Database.Entities;

public class Role
{
    public ulong Id { get; set; }

    public ulong? ParentId { get; set; }

    [Required] public RoleTree Tree { get; set; }

    [MinLength(1)][MaxLength(16)] public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    // Navigation properties

    public Role? Parent { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<ProductRole> ProductRoles { get; set; } = [];
}