namespace Backend.Database.Entities;

public interface ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted => DeletedAt.HasValue;
}