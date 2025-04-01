using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class MemberFactory
{
    public static MemberUserEntity CreateEntityFromDto(CreateMemberRegForm dto, AddressEntity? address = null, PictureEntity? picture = null)
    {
        MemberUserEntity entity = new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            JobTitle = dto.JobTitle,
            DateOfBirth = dto.DateOfBirth,
            Password = dto.PassWord ?? ""
        };
        
        if (address != null)
        {
            entity.AddressId = address.Id;
        }
        else
        {
            entity.Address = new AddressEntity
            {
                StreetName = dto.StreetAddress,
                PostalCode = dto.PostalCode,
                City = dto.City
            };
        }

        if (picture != null)
        {
            entity.PictureId = picture.Id;
        }
        else
        {
            entity.Picture = new PictureEntity
            {
                PictureUrl = dto.ProfileImage
            };
        }

        return entity;
    }

    public static MemberModel CreateModelFromEntity(MemberUserEntity entity)
    {
        MemberModel model = new()
        {
            Id = Guid.Parse(entity.Id),
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email ?? "",
            PhoneNumber = entity.PhoneNumber ?? "",
            JobTitle = entity.JobTitle ?? "",
            DateOfBirth = entity.DateOfBirth,
            StreetAddress = entity.Address?.StreetName ?? "",
            PostalCode = entity.Address?.PostalCode ?? 0,
            City = entity.Address?.City ?? "",
            ProfileImage = entity.Picture?.PictureUrl ?? ""
        };

        return model;
    }

    
}
