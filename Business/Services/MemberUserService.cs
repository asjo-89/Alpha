using Business.Factories;
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




    public async Task<MemberUserResult<bool>> CreateAsync(MemberUserDto dto)
    {
        if (dto == null)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All required fields must be completed.", Data = false };

        var entity = MemberUserFactory.CreateEntityFromDto(dto);
        var exists = await _memberRepository.ExistsAsync(x => x.Email == entity.Email);

        if (exists.Success)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 409, ErrorMessage = $"Member with email address {dto.Email} already exists.", Data = false };

        try
        {
            await _memberRepository.BeginTransactionAsync();

            var result = await _userManager.CreateAsync(entity);

            if (!result.Succeeded)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Failed to create member.", Data = false };

            var role = await _roleManager.FindByIdAsync(dto.RoleId.ToString()!);

            if (role == null)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = "The role could not be found.", Data = false };

            var addedToRole = await _userManager.AddToRoleAsync(entity, role.Name!);
            if (!addedToRole.Succeeded)
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
            includes: [x => x.Picture, x => x.Address]
        );

        var members = result.Data.Select(MemberUserFactory.CreateModelFromEntity);

        return result.Success
            ? new MemberUserResult<IEnumerable<MemberUser>> { Succeeded = true, StatusCode = 200, Data = members ?? [] }
            : new MemberUserResult<IEnumerable<MemberUser>> { Succeeded = false, StatusCode = 404, ErrorMessage = "No members was found." };
    }


    public async Task<MemberUserResult<MemberUser>> GetMemberUserAsync(string value)
    {
        var result = await _memberRepository.GetAsync(
            filterBy: x => x.FirstName.ToLower() == value.ToLower() || x.LastName.ToLower() == value.ToLower() || x.Email == value.ToLower(),
            includes: null!
        );

        return result.Success
            ? new MemberUserResult<MemberUser> { Succeeded = true, StatusCode = 200, Data = MemberUserFactory.CreateModelFromEntity(result.Data!) }
            : new MemberUserResult<MemberUser> { Succeeded = false, StatusCode = 404, ErrorMessage = "No member was found." };
    }

    public async Task<MemberUserResult<MemberUser>> GetMemberUserAsync(Guid id)
    {
        var result = await _memberRepository.GetAsync(
            filterBy: x => x.Id == id,
            includes: [x => x.Address, x => x.Picture]
        );

        return result.Success
            ? new MemberUserResult<MemberUser> { Succeeded = true, StatusCode = 200, Data = MemberUserFactory.CreateModelFromEntity(result.Data!) }
            : new MemberUserResult<MemberUser> { Succeeded = false, StatusCode = 404, ErrorMessage = "No member was found." };
    }


    public async Task<MemberUserResult<bool>> ExistsAsync(Guid id)
    {
        var result = await _memberRepository.ExistsAsync(x => x.Id == id);

        return result.Success
            ? new MemberUserResult<bool> { Succeeded = true, StatusCode = 200, Data = result.Data }
            : new MemberUserResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = "No member was found." };
    }


    public async Task<MemberUserResult<bool>> UpdateAsync(MemberUserDto dto)
    {
        if (dto == null)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed.", Data = false };

        var entity = MemberUserFactory.CreateEntityFromDto(dto);

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


    public async Task<MemberUserResult<bool>> DeleteAsync(MemberUser data)
    {

        try
        {
            var entityFromDb = await _memberRepository.GetAsync(
    filterBy: x => x.Id == data.Id,
    includes: [x => x.Address, x => x.Picture]
);

            if (entityFromDb == null)
            {
                return new MemberUserResult<bool>
                {
                    Succeeded = false,
                    StatusCode = 404,
                    ErrorMessage = "User not found.",
                    Data = false
                };
            }

            // Mappa modellen till entiteten
            var entity = entityFromDb.MapTo<MemberUserEntity>();


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
