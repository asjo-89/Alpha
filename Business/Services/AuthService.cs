using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService(SignInManager<MemberUserEntity> signInManager, UserManager<MemberUserEntity> userManager) : IAuthService
{
    private readonly SignInManager<MemberUserEntity> _signInManager = signInManager;
    private readonly UserManager<MemberUserEntity> _userManager = userManager;


    public async Task<AuthResult<bool>> SignInAsync(SignInFormData formData)
    {
        if (formData == null)
            return new AuthResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Invalid email och password.", Data = false };

        var entity = await _userManager.FindByEmailAsync(formData.Email);
        if (entity == null)
            return new AuthResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = $"No user with email address {formData.Email} was found.", Data = false };

        try
        {
            await _signInManager.PasswordSignInAsync(entity, formData.Password, false, false);
            return new AuthResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
        }
        catch (Exception ex)
        {
            return new AuthResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to sign in user: {ex.Message}" };
        }
    }

    public async Task<AuthResult<bool>> SignOutAsync(MemberUserFormData formData)
    {
        try
        {
            await _signInManager.SignOutAsync();
            return new AuthResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
        }
        catch (Exception ex)
        {
            return new AuthResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to sign out user: {ex.Message}" };
        }
    }
}
