
using Data.Entities;
using Domain.Dtos;

namespace Data.Factories;

public static class AccountFactoryData
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
