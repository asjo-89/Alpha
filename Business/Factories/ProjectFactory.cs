using Data.Entities;
using Domain.Dtos;
using Domain.Helpers;
using Domain.Models;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectEntity CreateEntityFromDto(ProjectDto model) => new ProjectEntity
    {
        ProjectTitle = model.ProjectTitle,
        Description = model.Description,
        StartDate = model.StartDate,
        EndDate = model.EndDate,
        Budget = model.Budget,
        PictureId = model.PictureId ?? Guid.Empty,
        ClientId = model.ClientId ?? Guid.Empty,
        StatusName = StatusHelper.SetStatus(model.StartDate, model.EndDate),
        ProjectMembers = model.Members.Select(member => 
            new ProjectMemberEntity
            {
                MemberId = member.Id,
                ProjectId = model.Id ?? Guid.Empty
            }).ToList(),
    };

    public static Project CreateModelFromEntity(ProjectEntity entity) => new Project
    {
        Id = entity.Id,
        ProjectTitle = entity.ProjectTitle,
        Description = entity.Description,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Budget = entity.Budget,
        Created = entity.Created,
        ImageUrl = entity.Picture.ImageUrl,
        Client = new Client
        {
            Id = entity.Client.Id,
            ClientName = entity.Client.ClientName,
            Email = entity.Client.Email,
            PhoneNumber = entity.Client.PhoneNumber
        },
        StatusName = entity.StatusName,
        ProjectMembers = entity.ProjectMembers?.Select(member =>
            new MemberUser
            {
                Id = member.MemberId,
                FirstName = member.Member.FirstName ?? "Unknown",
                LastName = member.Member.LastName ?? "Unknown",
                JobTitle = member.Member.JobTitle ?? "Unknown",
                Email = member.Member.Email ?? "Unknown",
                PhoneNumber = member.Member.PhoneNumber ?? "Unknown",
                DateOfBirth = member.Member.DateOfBirth,
                Address = new Address
                {
                    Id = member.Member.Address!.Id,
                    StreetAddress = member.Member.Address.StreetAddress,
                    PostalCode = member.Member.Address.PostalCode,
                    City = member.Member.Address.City
                } ?? new Address()

            }).ToList() ?? []
    };
}
