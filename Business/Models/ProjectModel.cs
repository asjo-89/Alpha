﻿namespace Business.Models;

public class ProjectModel
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Budget { get; set; }
    public int StatusId { get; set; }
    public int PictureId { get; set; }
    public int ClientId { get; set; }

    public IEnumerable<EmployeeModel> Employees = [];    
}
