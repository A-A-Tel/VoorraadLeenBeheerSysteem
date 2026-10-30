using Backend.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Protos.Role;

namespace Backend.Database;

public class DevelopmentDbContext : AppDbContext
{
    protected override string CurrentDatetimeSyntax => "CURRENT_TIMESTAMP";
    protected override string AuditLogDataColumnType => "TEXT";
    protected override string UserCardBytesColumnType => "BLOB";

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        const string relativePath = "Database/database.sqlite";

        if (!File.Exists(relativePath))
            using (File.Create(relativePath)) { }

        options.UseSqlite("Data Source=" + relativePath);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        PasswordHasher<User> hasher = new();
        string testPassword = hasher.HashPassword(null!, "Hallo12345%");

        #region Data

        builder.Entity<User>()
            .HasData(
                new User
                {
                    Id = 1,
                    RoleId = RoleType.Lender,
                    TitleId = TitleType.Teacher,
                    CardBytes = [0x04, 0x6C, 0xC8, 0xB2, 0xC8, 0x15, 0x90],
                    Name = "Uit L. Ener",
                    Email = "uitlener@roc-nijmegen.nl",
                    Number = "123456",
                    PasswordHash = testPassword
                }, new User
                {
                    Id = 2,
                    RoleId = RoleType.Borrower,
                    TitleId = TitleType.Student,
                    CardBytes = [0x04, 0x6C, 0xC8, 0xB2, 0xC8, 0x15, 0x80],
                    Name = "Regu Larjoe",
                    Email = "regularjoe@student.roc-nijmegen.nl",
                    Number = "1234567",
                    PasswordHash = testPassword
                }, new User
                {
                    Id = 3,
                    RoleId = RoleType.Manager,
                    TitleId = TitleType.Teacher,
                    CardBytes = [0x04, 0x6C, 0xC8, 0xB2, 0xC8, 0x15, 0x70],
                    Name = "Man Ager",
                    Email = "manager@roc-nijmegen.nl",
                    Number = "654321",
                    PasswordHash = testPassword
                });

        #endregion
    }

    protected override void UpdatedAtColumn<TProperty>(PropertyBuilder<TProperty> builder)
    {
        builder
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql(CurrentDatetimeSyntax);
    }
}