using Alpha_Mvc.Models;
using Data.Entities;
using Domain.Dtos;

namespace Alpha_Mvc.Factories;

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

    public static CreateAccountDto CreateDtoFromModel(CreateAccountModel model)
    {
        return new CreateAccountDto
        {
            Email = model.Email,
            Password = model.Password,
            ImageUrl = "~/Images/Profiles/Profile1.png"
        };
    }
}
