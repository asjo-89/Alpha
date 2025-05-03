using Alpha_Mvc.Models;
using Domain.Dtos;

namespace Alpha_Mvc.Factories;

public static class AccountFactoryMCV
{
    //public static MemberUserEntity CreateEntityFromDto(CreateAccountDto dto)
    //{
    //    return new MemberUserEntity
    //    {
    //        UserName = dto.Email,
    //        FirstName = dto.FirstName,
    //        LastName = dto.LastName,
    //        Email = dto.Email,
    //        PictureId = dto.PictureId,
    //    };
    //}

    public static CreateAccountDto CreateDtoFromModel(CreateAccountModel model)
    {
        return new CreateAccountDto
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password,
            ImageUrl = "~/Images/Profiles/Profile2.png"
        };
    }

    public static SignInDto SignInDtoFromModel(SignInFormModel model)
    {
        return new SignInDto
        {
            Email = model.Email,
            Password = model.Password,
            IsPersistent = model.IsPersistent
        };
    }
}
