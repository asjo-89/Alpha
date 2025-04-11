using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Business.Services;

public class AuthService(SignInManager<MemberUserEntity> signInManager, UserManager<MemberUserEntity> userManager, IMemberUserRepository memberRepository, IPictureRepository pictureRepository) : IAuthService
{
    private readonly SignInManager<MemberUserEntity> _signInManager = signInManager;
    private readonly UserManager<MemberUserEntity> _userManager = userManager;
    private readonly IMemberUserRepository _memberRepository = memberRepository;
    private readonly IPictureRepository _pictureRepository = pictureRepository;


    public async Task<AuthResult<bool>> SignInAsync(SignInFormData formData)
    {
        if (formData == null)
            return new AuthResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Invalid email och password.", Data = false };

        var entity = await _userManager.FindByEmailAsync(formData.Email);
        if (entity == null)
            return new AuthResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = $"No user with email address {formData.Email} was found.", Data = false };

        try
        {
            var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, formData.IsPersistent, lockoutOnFailure: false);

            return result.Succeeded
                ? new AuthResult<bool> { Succeeded = true, StatusCode = 200, Data = true }
                : new AuthResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Error signing in.", Data = false };
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


    public async Task<MemberUserResult<bool>> CreateUserAsync(CreateUserFormData formData)
    {
        if (formData == null)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All required fields must be completed.", Data = false };

        var pictureResult = await _pictureRepository.GetAsync(
            filterBy: x => x.ImageUrl == formData.ImageUrl, 
            includes: null!
        );

        if (pictureResult.Data == null)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = "Picture not found.", Data = false };

        formData.PictureId = pictureResult.Data.Id;

        // problem här!
        var entity = formData.MapTo<MemberUserEntity>();
        entity.UserName = formData.Email;

        var exists = await _memberRepository.ExistsAsync(x => x.Email == entity.Email);

        if (exists.Success)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 409, ErrorMessage = $"Member with email address {formData.Email} already exists.", Data = false };

        try
        {
            await _memberRepository.BeginTransactionAsync();

            var result = await _userManager.CreateAsync(entity, entity.Password);

            //Kolla getall!
            var members = await _memberRepository.GetAllAsync
                (
                    orderByDescending: false,
                    orderBy: x => x.Email,
                    filterBy: null!,
                    includes: null!
                );
            if (members.Data == null || !members.Data.Any())
            {
                await _userManager.AddToRoleAsync(entity, "Administrator");
            }
            else
            {
                await _userManager.AddToRoleAsync(entity, "User");
            }

            if (result == null!)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Failed to create member.", Data = false };

            await _memberRepository.CommitTransactionAsync();

            return new MemberUserResult<bool> { Succeeded = true, StatusCode = 201, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _memberRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to create member: {ex.Message} ", Data = false };
        }
    }
}
