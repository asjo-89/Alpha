using Data.Entities;
using Domain.Models;

namespace Data.Factories;

public static class MemberUserFactory
{
    public static MemberUserEntity CreateEntityFromModel(MemberUser model)
    {
        return new MemberUserEntity
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            JobTitle = model.JobTitle,
            DateOfBirth = model.DateOfBirth,
            AddressId = model.Address.Id,
            PictureId = model.PictureId,
        };
    }

    public static MemberUser CreateModelFromEntity(MemberUserEntity entity)
    {
        return new MemberUser
        {
            Id = entity.Id,
            FirstName = entity.FirstName ?? "First name is missing.",
            LastName = entity.LastName ?? "Last name is missing.",
            Email = entity.Email ?? "Email is missing.",
            PhoneNumber = entity.PhoneNumber,
            JobTitle = entity.JobTitle,
            DateOfBirth = entity.DateOfBirth,
            AddressId = entity.AddressId,
            PictureId = entity.PictureId,
            Address = entity.Address != null ? new Address
            {
                Id = entity.Address.Id,
                StreetAddress = entity.Address.StreetAddress,
                PostalCode = entity.Address.PostalCode,
                City = entity.Address.City
            } : null,
            ImageUrl = entity.Picture!.ImageUrl ?? ""
        };
    }
}
