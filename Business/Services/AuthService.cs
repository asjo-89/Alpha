using Business.Dtos;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class AuthService(UserManager<MemberUserEntity> userManager)
{
    private readonly UserManager<MemberUserEntity> _userManager = userManager;

    public async Task<int> CreateAsync(CreateAccountRegForm form)
    {
        if (form == null)
            return 400;

        MemberUserEntity appUser = new MemberUserEntity
        {
            UserName = form.EmailAddress,
            Email = form.EmailAddress,
            FirstName = form.FirstName,
            LastName = form.LastName,
        };

        var result = await _userManager.CreateAsync(appUser, form.Password);

        if (result.Succeeded)
            return 201;

        else
            return 500;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        if (await _userManager.Users.AnyAsync(u => u.Email == email))
            return true;

        return false;
    }
}
