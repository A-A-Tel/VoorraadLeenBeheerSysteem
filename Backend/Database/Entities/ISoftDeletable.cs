namespace Backend.Database.Entities;

public interface ISoftDeletable
{
    DateTime? DeletedAt { get; set; }
    bool IsDeleted => DeletedAt.HasValue;
}