using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class MemberUserService(IMemberUserRepository memberRepository, UserManager<MemberUserEntity> userManager, RoleManager<IdentityRole<Guid>> roleManager) : IMemberUserService
{
    private readonly IMemberUserRepository _memberRepository = memberRepository;
    private readonly UserManager<MemberUserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;




    public async Task<MemberUserResult<bool>> CreateAsync(MemberUserFormData formData)
    {
        if (formData == null)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All required fields must be completed.", Data = false };

        var entity = formData.MapTo<MemberUserEntity>();
        var exists = await _memberRepository.ExistsAsync(x => x.Email == entity.Email);

        if (exists.Success)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 409, ErrorMessage = $"Member with email address {formData.Email} already exists.", Data = false };

        try
        {
            await _memberRepository.BeginTransactionAsync();

            entity.UserName = entity.Email;

            exists = await _memberRepository.ExistsAsync(x => x.Email == entity.Email);

            if (exists.Success)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 409, ErrorMessage = $"Member with email address {formData.Email} already exists.", Data = false };

            var result = await _userManager.CreateAsync(entity);

            if (!result.Succeeded)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Failed to create member.", Data = false };

            var role = await _roleManager.FindByIdAsync(formData.RoleId.ToString()!);
            if (role == null)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = "The role could not be found.", Data = false };

            var addedRole = await _userManager.AddToRoleAsync(entity, role.Name!);
            if (!addedRole.Succeeded)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Failed to assign role to member.", Data = false };

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


    public async Task<MemberUserResult<IEnumerable<MemberUser>>> GetMemberUsersAsync()
    {
        var result = await _memberRepository.GetAllAsync(
            orderByDescending: false,
            orderBy: x => x.Email,
            filterBy: null!,
            includes: x => x.Picture
        );

        return result.Success
            ? new MemberUserResult<IEnumerable<MemberUser>> { Succeeded = true, StatusCode = 200, Data = result.Data ?? [] }
            : new MemberUserResult<IEnumerable<MemberUser>> { Succeeded = false, StatusCode = 404, ErrorMessage = "No members was found." };
    }


    public async Task<MemberUserResult<MemberUser>> GetMemberUserAsync(string value)
    {
        var result = await _memberRepository.GetAsync(
            filterBy: x => x.FirstName.ToLower() == value.ToLower() || x.LastName.ToLower() == value.ToLower() || x.Email == value.ToLower(),
            includes: null!);

        return result.Success
            ? new MemberUserResult<MemberUser> { Succeeded = true, StatusCode = 200, Data = result.Data?.MapTo<MemberUser>() }
            : new MemberUserResult<MemberUser> { Succeeded = false, StatusCode = 404, ErrorMessage = "No member was found." };
    }


    public async Task<MemberUserResult<bool>> ExistsAsync(Guid id)
    {
        var result = await _memberRepository.ExistsAsync(x => x.Id == id);

        return result.Success
            ? new MemberUserResult<bool> { Succeeded = true, StatusCode = 200, Data = result.Data }
            : new MemberUserResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = "No member was found." };
    }


    public async Task<MemberUserResult<bool>> UpdateAsync(MemberUserFormData formData)
    {
        if (formData == null)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed.", Data = false };

        var entity = formData.MapTo<MemberUserEntity>();

        try
        {
            await _memberRepository.BeginTransactionAsync();

            var result = await _userManager.UpdateAsync(entity);

            if (result == null)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Unable to update member.", Data = false };

            await _memberRepository.CommitTransactionAsync();

            return new MemberUserResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _memberRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to update member: {ex.Message}", Data = false };
        }
    }


    public async Task<MemberUserResult<bool>> DeleteAsync(MemberUserFormData formData)
    {
        var entity = formData.MapTo<MemberUserEntity>();

        try
        {
            await _memberRepository.BeginTransactionAsync();

            var result = await _userManager.DeleteAsync(entity);
            if (result == null)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Unable to delete member.", Data = false };

            await _memberRepository.CommitTransactionAsync();

            return new MemberUserResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
        }
        catch (Exception ex)
        {
            var rollback = await _memberRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to delete member: {ex.Message}", Data = false };
        }
    }
}
