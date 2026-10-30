using System.ComponentModel.DataAnnotations;
using Protos.Role;

namespace Backend.Database.Entities;

public class Title
{
    public TitleType Id { get; set; }

    // Navigation properties

    public ICollection<User> Users { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
}