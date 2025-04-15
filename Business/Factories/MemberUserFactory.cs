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
            AddressId = dto.AddressId,
            PictureId = dto.PictureId,
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
}
