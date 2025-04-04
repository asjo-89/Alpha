using Business.Dtos;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Business.Factories;

public static class MemberFactory
{
    public static MemberUserEntity CreateEntityFromDto(CreateMemberRegForm dto, AddressEntity address, PictureEntity picture)
    {
        MemberUserEntity entity = new()
        {
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,            
            DateOfBirth = dto.DateOfBirth,
            Password = dto.PassWord ?? "",
            AddressId = address?.Id,
            PictureId = picture?.Id
        };

        return entity;
    }

    public static MemberModel CreateModelFromEntity(MemberUserEntity entity, string roleName = null!)
    {
        MemberModel model = new()
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email ?? "",
            PhoneNumber = entity.PhoneNumber ?? "",
            JobTitle = roleName ?? "No role assigned.",
            DateOfBirth = entity.DateOfBirth,
            StreetAddress = entity.Address?.StreetName ?? "",
            PostalCode = entity.Address?.PostalCode ?? 0,
            City = entity.Address?.City ?? "",
            ProfileImage = entity.Picture?.PictureUrl ?? ""
        };

        return model;
    }

    public static MemberUserEntity CreateEntityFromModel(MemberModel model, AddressEntity address, PictureEntity picture)
    {
        MemberUserEntity entity = new()
        {
            Id = model.Id,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth,
            AddressId = address?.Id,
            PictureId = picture?.Id
        };

        return entity;
    }

    public static MemberUserEntity CreateEntityFromModel(MemberModel model)
    {
        MemberUserEntity entity = new()
        {
            Id = model.Id,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email
        };

        return entity;
    }
}
