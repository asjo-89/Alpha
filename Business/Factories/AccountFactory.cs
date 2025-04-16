using Data.Entities;
using Domain.Dtos;

namespace Business.Factories;

public static class AccountFactory
{
    public static MemberUserEntity CreateEntityFromDto(CreateAccountDto dto)
    {
        return new MemberUserEntity
        {
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PictureId = dto.PictureId
        };
    }

}
