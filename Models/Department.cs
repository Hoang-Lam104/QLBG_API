namespace QLGB.API.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = nameof(Department);
    public bool IsActive { get; set; }
}
