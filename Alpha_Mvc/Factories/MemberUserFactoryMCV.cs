using Alpha_Mvc.Models;
using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Alpha_Mvc.Factories;

public static class MemberUserFactoryMCV
{
    //public static MemberUserEntity CreateEntityFromModel(MemberUserDto dto)
    //{
    //    return new MemberUserEntity
    //    {
    //        FirstName = dto.FirstName,
    //        LastName = dto.LastName,
    //        Email = dto.Email,
    //        PhoneNumber = dto.PhoneNumber,
    //        JobTitle = dto.JobTitle,
    //        DateOfBirth = dto.DateOfBirth,
    //        PictureId = dto.PictureId,
    //    };
    //}

    public static MemberUserDto CreateDtoFromModel(MemberFormModel model)
    {
        return new MemberUserDto
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            JobTitle = model.JobTitle,
            DateOfBirth = model.DateOfBirth,
            StreetAddress = model.StreetAddress,
            PostalCode = model.PostalCode,
            City = model.City,
            RoleId = model.RoleId,
            ImageUrl = model.ImageUrl
        };
    }

    public static MemberUserDto CreateDtoFromModel(MemberUserModel model)
    {
        return new MemberUserDto
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            JobTitle = model.JobTitle,
            DateOfBirth = model.DateOfBirth,
            StreetAddress = model.StreetAddress,
            PostalCode = model.PostalCode,
            City = model.City,
            RoleId = model.RoleId,
            ImageUrl = model.ImageUrl
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
            PictureId = entity.PictureId,
            Address = entity.Address != null ? new Address
            {
                Id = entity.Address.Id,
                StreetAddress = entity.Address.StreetAddress,
                PostalCode = entity.Address.PostalCode,
                City = entity.Address.City
            } : null,
            ImageUrl = entity.Picture.ImageUrl
        };
    }

    public static MemberUserModel CreateModelFromDomainModel(MemberUser domain)
    {
        return new MemberUserModel
        {
            Id = domain.Id,
            FirstName = domain.FirstName ?? "First name is missing",
            LastName = domain.LastName ?? "Last name is missing",
            Email = domain.Email ?? "Email is missing",
            PhoneNumber = domain.PhoneNumber ?? "Phone number is missing",
            ImageUrl = $"/{domain.ImageUrl}"
        };
    }
}
