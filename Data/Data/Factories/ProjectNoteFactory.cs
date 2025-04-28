using Data.Entities;
using Domain.Models;

namespace Data.Factories;

public static class ProjectNoteFactory
{
    public static ProjectNoteEntity CreateEntityFromModel(ProjectNote model)
    {
        return new ProjectNoteEntity
        {
            Content = model.Content,
            Created = model.Created,
            MemberId = model.Member.Id,
            ProjectId = model.ProjectId
        };
    }

    public static ProjectNote CreateModelFromEntity(ProjectNoteEntity entity)
    {
        return new ProjectNote
        {
            Id = entity.Id,
            Content = entity.Content,
            Created = entity.Created,
            Member = new MemberUser
                {
                    Id = entity.Member.Id,
                    FirstName = entity.Member.FirstName ?? "First name is missing.",
                    LastName = entity.Member.LastName ?? "Last name is missing.",
                    Email = entity.Member.Email ?? "Email is missing.",
                    PhoneNumber = entity.Member.PhoneNumber ?? "Phone number is missing.",
                    JobTitle = entity.Member.JobTitle ?? "Job title is missing.",
                    DateOfBirth = entity.Member.DateOfBirth,
                    Address = entity.Member.Address != null
                        ? new Address
                        {
                            Id = entity.Member.Address.Id,
                            StreetAddress = entity.Member.Address.StreetAddress ?? "Street address is missing.",
                            PostalCode = entity.Member.Address.PostalCode ?? "Postal code is missing.",
                            City = entity.Member.Address.City ?? "City is missing."
                        }
                        : null,
                    ImageUrl = entity.Member.Picture?.ImageUrl ?? "Profile image is missing.",
                    PictureId = entity.Member.PictureId
                }
        };
    }
}
