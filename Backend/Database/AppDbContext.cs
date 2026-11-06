using System.Linq.Expressions;
using System.Text.Json;
using Backend.Database.Entities;
using Backend.Database.Entities.Junctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MySqlConnector;
using Protos.Role;

namespace Backend.Database;

public class AppDbContext : DbContext
{
    protected virtual string CurrentDatetimeSyntax => "CURRENT_TIMESTAMP(6)";
    protected virtual string AuditLogDataColumnType => "JSON";
    protected virtual string UserCardBytesColumnType => "BINARY(7)";

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Title> Titles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

    public DbSet<LoanProduct> LoanProducts { get; set; }
    public DbSet<ProductNote> ProductNotes { get; set; }
    public DbSet<UserNote> UserNotes { get; set; }

    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region Config

        // Exclude soft deleted entities from queries
        foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                builder.Entity(entityType.ClrType)
                    .HasQueryFilter(ConvertFilterExpression(entityType.ClrType));

        // Set AuditLog.AuditData to appropriate column type
        ValueConverter<JsonDocument, string> jsonDocumentConverter =
            new(
                document => document.RootElement.GetRawText(),
                json => JsonDocument.Parse(json)
            );

        builder.Entity<AuditLog>()
            .Property(al => al.AuditData)
            .HasColumnType(AuditLogDataColumnType)
            .HasConversion(jsonDocumentConverter);

        // Set Users.CardBytes to appropriate column type
        builder.Entity<User>()
            .Property(u => u.CardBytes)
            .HasColumnType(UserCardBytesColumnType);

        #endregion

        #region Junction Composite Keys

        builder.Entity<LoanProduct>()
            .HasKey(x => new { x.LoanId, x.ProductId });

        builder.Entity<ProductNote>()
            .HasKey(x => new { x.ProductId, x.NoteId });

        builder.Entity<UserNote>()
            .HasKey(x => new { x.UserId, x.NoteId });

        #endregion

        #region Foreign Keys

        builder.Entity<Log>()
            .HasOne(x => x.Invoker)
            .WithMany(x => x.InvokedLogs)
            .HasForeignKey(y => y.InvokerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Log>()
            .HasOne(x => x.Target)
            .WithMany(x => x.TargetLogs)
            .HasForeignKey(y => y.TargetId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Loan>()
            .HasOne(x => x.Lender)
            .WithMany(x => x.LentLoans)
            .HasForeignKey(y => y.LenderId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Loan>()
            .HasOne(x => x.Borrower)
            .WithMany(x => x.BorrowedLoans)
            .HasForeignKey(y => y.BorrowerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Note>()
            .HasOne(x => x.Writer)
            .WithMany(x => x.Notes)
            .HasForeignKey(y => y.WriterId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<ProductNote>()
            .HasOne(x => x.Product)
            .WithMany(x => x.ProductNotes)
            .HasForeignKey(y => y.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProductNote>()
            .HasOne(x => x.Note)
            .WithMany(x => x.ProductNotes)
            .HasForeignKey(y => y.NoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserNote>()
            .HasOne(x => x.Note)
            .WithMany(x => x.UserNotes)
            .HasForeignKey(y => y.NoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserRefreshToken>()
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Default Values

        #region Dates

        builder.Entity<User>()
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(CurrentDatetimeSyntax);

        UpdatedAtColumn(
            builder.Entity<User>()
                .Property(x => x.UpdatedAt)
        );

        builder.Entity<Note>()
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(CurrentDatetimeSyntax);

        UpdatedAtColumn(
            builder.Entity<Note>()
                .Property(x => x.UpdatedAt)
        );

        builder.Entity<Loan>()
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(CurrentDatetimeSyntax);

        UpdatedAtColumn(
            builder.Entity<Loan>()
                .Property(x => x.UpdatedAt)
        );

        builder.Entity<Product>()
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(CurrentDatetimeSyntax);

        UpdatedAtColumn(
            builder.Entity<Product>()
                .Property(x => x.UpdatedAt)
        );

        builder.Entity<Log>()
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(CurrentDatetimeSyntax);

        builder.Entity<LoanProduct>()
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(CurrentDatetimeSyntax);

        UpdatedAtColumn(
            builder.Entity<LoanProduct>()
                .Property(x => x.UpdatedAt)
        );

        builder.Entity<ProductNote>()
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(CurrentDatetimeSyntax);

        builder.Entity<UserNote>()
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql(CurrentDatetimeSyntax);

        #endregion

        #endregion

        #region Data

        builder.Entity<Role>()
            .HasData(
                new Role
                {
                    Id = RoleType.Borrower
                },
                new Role
                {
                    Id = RoleType.Lender
                },
                new Role
                {
                    Id = RoleType.Manager
                }
            );

        builder.Entity<Title>()
            .HasData(
                new Title
                {
                    Id = TitleType.Student
                },
                new Title
                {
                    Id = TitleType.Teacher
                }
            );

        #endregion
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        MySqlConnectionStringBuilder builder = new()
        {
            Server = GetEnv("DB_SERVER"),
            Port = GetEnvIntUnsigned("DB_PORT"),
            Database = GetEnv("DB_NAME"),
            UserID = GetEnv("DB_USERNAME"),
            Password = GetEnv("DB_PASSWORD")
        };

        options.UseMySql(builder.ConnectionString, ServerVersion.AutoDetect(builder.ConnectionString));
    }

    protected virtual void UpdatedAtColumn<TProperty>(PropertyBuilder<TProperty> builder)
    {
        builder
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql($"{CurrentDatetimeSyntax} ON UPDATE {CurrentDatetimeSyntax}");
    }

    private static string GetEnv(string key)
    {
        return Environment.GetEnvironmentVariable(key) ??
               throw new NullReferenceException("Environment value not found.");
    }

    private static uint GetEnvIntUnsigned(string key)
    {
        string parsable = GetEnv(key);
        return uint.Parse(parsable);
    }

    private static LambdaExpression ConvertFilterExpression(Type type)
    {
        ParameterExpression parameter = Expression.Parameter(type, "e");
        MemberExpression property = Expression.Property(parameter, nameof(ISoftDeletable.DeletedAt));
        ConstantExpression nullConstant = Expression.Constant(null, typeof(DateTime?));
        BinaryExpression comparison = Expression.Equal(property, nullConstant);

        return Expression.Lambda(comparison, parameter);
    }
}