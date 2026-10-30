using System.ComponentModel.DataAnnotations;
using Backend.Database.Entities.Junctions;
using Microsoft.EntityFrameworkCore;
using Protos.Role;

namespace Backend.Database.Entities;

[Index(nameof(CardBytes), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Number), IsUnique = true)]
public class User : ISoftDeletable
{
    public ulong Id { get; set; }

    public RoleType RoleId { get; set; }

    public TitleType TitleId { get; set; }

    [MinLength(7)][MaxLength(7)] public byte[]? CardBytes { get; set; }

    [MinLength(1)][MaxLength(32)] public string Name { get; set; } = string.Empty;

    [MinLength(1)][MaxLength(254)] public string Email { get; set; } = string.Empty;

    [MinLength(6)][MaxLength(7)] public string Number { get; set; } = string.Empty;

    [MinLength(1)][MaxLength(255)] public string? PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties

    public Role Role { get; set; } = null!;
    public Title Title { get; set; } = null!;

    public ICollection<Log> InvokedLogs { get; set; } = [];
    public ICollection<Log> TargetLogs { get; set; } = [];
    public ICollection<Note> Notes { get; set; } = [];
    public ICollection<Loan> LentLoans { get; set; } = [];
    public ICollection<Loan> BorrowedLoans { get; set; } = [];
    public ICollection<UserNote> UserNotes { get; set; } = [];
    public ICollection<UserRefreshToken> RefreshTokens { get; set; } = [];

    public DateTime? DeletedAt { get; set; }
}