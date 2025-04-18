using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Factories;

public static class MemberUserFactory
{
    public static MemberUserEntity CreateEntityFromDto(MemberUserDto dto)
    {
        return new MemberUserEntity
        {
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            JobTitle = dto.JobTitle,
            DateOfBirth = dto.DateOfBirth,
            PictureId = dto.PictureId,
        };
    }

    public static MemberUserEntity UpdateEntityFromDto(MemberUserEntity entity, MemberUserDto dto)
    {
        entity.Id = dto.Id;
        entity.UserName = dto.Email;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;
        entity.JobTitle = dto.JobTitle;
        entity.DateOfBirth = dto.DateOfBirth;
        entity.PictureId = dto.PictureId;

        return entity;
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
            Address = entity.Address != null ? new Address
            {
                Id = entity.Address.Id,
                StreetAddress = entity.Address.StreetAddress,
                PostalCode = entity.Address.PostalCode,
                City = entity.Address.City
            } : null,
            ImageUrl = entity.Picture?.ImageUrl
        };
    }

    public static MemberUserEntity CreateEntityFromModel(MemberUser model)
    {
        return new MemberUserEntity
        {
            Id = model.Id,
            FirstName = model.FirstName ?? "First name is missing.",
            LastName = model.LastName ?? "Last name is missing.",
            Email = model.Email ?? "Email is missing.",
            PhoneNumber = model.PhoneNumber,
            JobTitle = model.JobTitle,
            DateOfBirth = model.DateOfBirth,
            Address = model.Address != null ? new AddressEntity
            {
                Id = model.Address.Id,
                StreetAddress = model.Address.StreetAddress,
                PostalCode = model.Address.PostalCode,
                City = model.Address.City
            } : null,
            PictureId = model.PictureId
        };
    }
}
