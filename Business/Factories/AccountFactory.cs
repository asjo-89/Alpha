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
            Email = dto.Email,
            PictureId = dto.PictureId
        };
    }

    //public static CreateAccountDto CreateDtoFromModel(CreateAccou dto)
    //{
    //    return new MemberUserEntity
    //    {
    //        UserName = dto.Email,
    //        Email = dto.Email,
    //        PictureId = dto.PictureId
    //    };
    //}
}
