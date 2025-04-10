﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEmployeeEntity
{
    public Guid ProjectId { get; set; }
    public Guid EmployeeId { get; set; } 


    // Relations
    [ForeignKey(nameof(ProjectId))]
    public ProjectEntity Project { get; set; } = null!;

    [ForeignKey(nameof(EmployeeId))]
    public MemberUserEntity Employee { get; set; } = null!;
}
