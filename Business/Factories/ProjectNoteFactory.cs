using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProjectNoteFactory
{
    public static ProjectNoteEntity CreateEntityFromDto(ProjectNoteRegForm dto)
    {
        ProjectNoteEntity entity = new()
        {
            Note = dto.Description,
            Created = dto.Created,
            EmployeeId = dto.EmployeeId,
            ProjectId = dto.ProjectId
        };
        return entity;
    }

    public static ProjectNotesModel CreateModelFromEntity(ProjectNoteEntity entity)
    {
        ProjectNotesModel model = new()
        {
            Id = entity.Id,
            Note = entity.Note,
            Created = entity.Created,
            EmployeeId = entity.EmployeeId,
            ProjectId = entity.ProjectId
        };
        return model;
    }

    public static ProjectNoteEntity CreateEntityFromModel(ProjectNotesModel model)
    {
        ProjectNoteEntity entity = new()
        {
            Id = model.Id,
            Note = model.Note,
            Created = model.Created,
            EmployeeId = model.EmployeeId,
            ProjectId = model.ProjectId
        };
        return entity;
    }
}
