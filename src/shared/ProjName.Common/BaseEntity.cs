namespace ProjName.Common;
public abstract class BaseEntity<TPK> where TPK: struct
{
    public TPK Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public TPK? CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public TPK? UpdatedById { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public abstract class BaseEntity: BaseEntity<Guid>
{

}