using Data.Entities;
using Domain.Models;

namespace Data.Factories;

public static class ClientFactory
{
    public static ClientEntity CreateEntityFromModel(Client model)
    {
        return new ClientEntity
        {
            ClientName = model.ClientName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Projects = model.Projects.Select(project => 
                new ProjectEntity
                {
                    Id = project.Id,
                    ProjectTitle = project.ProjectTitle,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Budget = project.Budget,
                    Created = project.Created,
                    PictureId = project.Picture.Id,
                    ClientId = project.Client.Id,
                    StatusId = project.Status.Id,
                }).ToList() ?? []
        };
    }

    public static Project CreateModelFromEntity(ProjectEntity entity)
    {
        return new Project
        {
            Id = entity.Id,
            ProjectTitle = entity.ProjectTitle,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget= entity.Budget,
            Created = entity.Created,
            Picture = new Picture
            {
                Id = entity.Picture.Id,
                ImageUrl = entity.Picture.ImageUrl
            },
            Client = new Client
            {
                Id = entity.Client.Id,
                ClientName = entity.Client.ClientName,
                Email = entity.Client.Email,
                PhoneNumber = entity.Client.PhoneNumber
            }
        };
    }
}
