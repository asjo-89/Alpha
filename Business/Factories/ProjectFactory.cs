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
        Client = entity.Client != null ? new Client
        {
            Id = entity.Client.Id,
            ClientName = entity.Client.ClientName ?? string.Empty,
            Email = entity.Client.Email ?? string.Empty,
            PhoneNumber = entity.Client.PhoneNumber,
        } : new Client
        {
            ClientName = string.Empty,
            Email = string.Empty,
            PhoneNumber = string.Empty
        },
        StatusName = entity.StatusName,
        ProjectMembers = entity.ProjectMembers.Select(member =>
            member != null ? new MemberUser
            {
                Id = member.MemberId,
                FirstName = member.Member.FirstName ?? string.Empty,
                LastName = member.Member.LastName ?? string.Empty,
                JobTitle = member.Member.JobTitle ?? string.Empty,
                Email = member.Member.Email ?? string.Empty,
                PhoneNumber = member.Member.PhoneNumber ?? string.Empty,
                DateOfBirth = member.Member.DateOfBirth ?? null,
                Address = member.Member.Address != null ? new Address
                {
                    Id = member.Member.Address.Id,
                    StreetAddress = member.Member.Address.StreetAddress,
                    PostalCode = member.Member.Address.PostalCode,
                    City = member.Member.Address.City
                } : new Address
                {
                    StreetAddress = string.Empty,
                    PostalCode= string.Empty,
                    City = string.Empty
                }

            } : new MemberUser
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                PhoneNumber = string.Empty,
                JobTitle= string.Empty                
            }).ToList()
    };
}
