namespace Backend.Database.Entities;

public interface ISoftDeletable
{
    bool IsDeleted => DeletedAt.HasValue;
    DateTime? DeletedAt { get; set; }
}