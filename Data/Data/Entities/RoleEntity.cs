namespace Data.Entities;

public class RoleEntity
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = null!;


    // Relations
    public ICollection<EmployeeEntity> Employees { get; set; } = [];
}
