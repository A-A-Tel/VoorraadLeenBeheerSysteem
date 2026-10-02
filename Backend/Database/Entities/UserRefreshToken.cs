using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Entities;

[PrimaryKey(nameof(UserId))]
[Index(nameof(Token), IsUnique = true)]
public class UserRefreshToken
{
    public ulong UserId { get; set; }

    [MinLength(1)][MaxLength(255)] public string Token { get; set; } = null!;

    [MaxLength(64)] public string? CreatedByIp { get; set; } = null!;

    public string? ReplacedByToken { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties

    public User User { get; set; } = null!;

    // Helper Properties

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsExpired && !IsRevoked;
}
