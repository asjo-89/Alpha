namespace Data.Entities;

public class AddressEntity
{
    public Guid Id { get; set; }
    public string StreetName { get; set; } = null!;
    public int PostalCode { get; set; }
    public string City { get; set; } = null!;


    // Relations
    public ICollection<EmployeeEntity> Employees { get; set; } = [];
}
