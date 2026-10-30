using System.ComponentModel.DataAnnotations;
using Backend.Database.Entities.Junctions;
using Protos.Role;

namespace Backend.Database.Entities;

public class Role
{
    public RoleType Id { get; set; }

    // Navigation properties

    public ICollection<User> Users { get; set; } = [];
}