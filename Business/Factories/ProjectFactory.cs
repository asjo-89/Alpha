using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectEntity CreateEntityFromDto(ProjectRegForm dto, PictureEntity picture)
    {
        ProjectEntity entity = new()
        {
            ProjectName = dto.ProjectName,
            ProjectDescription = dto.ProjectDescription ?? "",
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            StatusId = dto.StatusID,
            ClientId = dto.ClientId,
            Budget = dto.Budget,
            PictureId = picture.Id
        };
        return entity;
    }

    public static ProjectModel CreateModelFromEntity(ProjectEntity entity, PictureEntity picture, ClientEntity client, StatusEntity status)
    {
        ProjectModel model = new()
        {
            Id = entity.Id,
            ProjectName = entity.ProjectName,
            ProjectDescription = entity.ProjectDescription ?? "",
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            StatusName = status.StatusName,
            ClientName = client.ClientName,
            Budget = entity.Budget,
            PictureUrl = picture.PictureUrl,
            ProjectNotes = entity.Notes.Select(note => new ProjectNotesModel
            {
                Id = note.Id,
                Note = note.Note,
                Created = note.Created,
                Member = note.Employee.FirstName + " " + note.Employee.LastName,
                ProjectName = note.Project.ProjectName
            }).ToList(),
            Members = entity.ProjectEmployees.Select(pe => new MemberModel
            {
                Id = pe.Employee.Id,
                FirstName = pe.Employee.FirstName,
                LastName = pe.Employee.LastName,
                PhoneNumber = pe.Employee.PhoneNumber ?? "",
                Email = pe.Employee.Email ?? "",
                DateOfBirth = pe.Employee.DateOfBirth,
                StreetAddress = pe.Employee.Address?.StreetName ?? "",
                PostalCode = pe.Employee.Address?.PostalCode ?? 0,
                City = pe.Employee.Address?.City ?? "",
                ProfileImage = pe.Employee.Picture?.PictureUrl ?? ""
            }).ToList()
        };
        return model;
    }

    public static ProjectEntity CreateEntityFromModel(ProjectModel model, PictureModel picture, ClientModel client, StatusModel status, IEnumerable<ProjectNoteEntity> notes)
    {
        ProjectEntity entity = new()
        {
            Id = model.Id,
            ProjectName = model.ProjectName,
            ProjectDescription = model.ProjectDescription ?? "",
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            StatusId = status.Id,
            ClientId = client.Id,
            Budget = model.Budget,
            PictureId = picture.Id,
            Notes = notes.ToList(),
            ProjectEmployees = model.Members.Select(member => new ProjectEmployeeEntity
            {
                ProjectId = model.Id,
                EmployeeId = member.Id
            }).ToList()
        };
        return entity;
    }
}
