using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class ClientFactory
{
    //public static ClientEntity CreateEntityFromModel(Client model)
    //{
    //    return new ClientEntity
    //    {
    //        ClientName = model.ClientName,
    //        Email = model.Email,
    //        PhoneNumber = model.PhoneNumber,
    //        Projects = model.Projects.Select(project =>
    //            new ProjectEntity
    //            {
    //                Id = project.Id,
    //                ProjectTitle = project.ProjectTitle,
    //                Description = project.Description,
    //                StartDate = project.StartDate,
    //                EndDate = project.EndDate,
    //                Budget = project.Budget,
    //                Created = project.Created,
    //                PictureId = project.PictureId,
    //                ClientId = project.Client.Id,
    //                StatusId = project.Status.Id,
    //            }).ToList() ?? []
    //    };
    //}

    public static Client CreateModelFromEntity(ClientEntity entity) => new Client
    {        
        Id = entity.Id,
        ClientName = entity.ClientName,
        Email = entity.Email,
        PhoneNumber = entity.PhoneNumber        
    };
    
}
