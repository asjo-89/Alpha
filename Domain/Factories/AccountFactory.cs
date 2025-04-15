using Alpha_Mvc.Models;
using Data.Entities;
using Domain.Dtos;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Factories;

public static class AccountFactory
{
    public static MemberUserEntity CreateEntityFromDto(CreateAccountDto dto)
    {
        return new MemberUserEntity
        {
            UserName = dto.Email,
            Email = dto.Email,
            PictureId = dto.PictureId,
        };
    }
}
