using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            using (File.Create(relativePath))
            {
            }

        options.UseSqlite("Data Source=" + relativePath);
    }

    protected override void UpdatedAtColumn<TProperty>(PropertyBuilder<TProperty> builder)
    {
        builder
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql(CurrentDatetimeSyntax);
    }
}