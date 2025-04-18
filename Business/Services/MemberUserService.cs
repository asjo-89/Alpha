using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class MemberUserService(IMemberUserRepository memberRepository, UserManager<MemberUserEntity> userManager, RoleManager<IdentityRole<Guid>> roleManager, IPictureService pictureService, IPictureRepository pictureRepository) : IMemberUserService
{
    private readonly IMemberUserRepository _memberRepository = memberRepository;
    private readonly UserManager<MemberUserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
    private readonly IPictureService _pictureService = pictureService;
    private readonly IPictureRepository _pictureRepository = pictureRepository;




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
        //var result = await _memberRepository.GetAllAsync(
        //    orderByDescending: false,
        //    orderBy: x => x.Email,
        //    filterBy: null!,
        //    includes: [x => x.Picture, x => x.Address]
        //);

        //var members = result.Data.Select(MemberUserFactory.CreateModelFromEntity);

        var members = await _userManager.Users
            .Include(x => x.Address)
            .Include(x => x.Picture)
            .ToListAsync();

        return members.Count > 0
            ? new MemberUserResult<IEnumerable<MemberUser>> { Succeeded = true, StatusCode = 200, Data = members.Select(MemberUserFactory.CreateModelFromEntity) ?? [] }
            : new MemberUserResult<IEnumerable<MemberUser>> { Succeeded = false, StatusCode = 404, ErrorMessage = "No members was found." };
    }


    public async Task<MemberUserResult<MemberUser>> GetMemberUserAsync(string value)
    {
        //var result = await _memberRepository.GetAsync(
        //    filterBy: x => x.FirstName.ToLower() == value.ToLower() || x.LastName.ToLower() == value.ToLower() || x.Email == value.ToLower(),
        //    includes: null!
        //);

        if (string.IsNullOrEmpty(value))
            return new MemberUserResult<MemberUser> { Succeeded = false, StatusCode = 400, ErrorMessage = "Invalid search term provided." };

        var member = await _userManager.Users
            .Include(x => x.Address)
            .Include(x => x.Picture)
            .FirstOrDefaultAsync(x => x.FirstName!.ToLower() == value.ToLower() || x.LastName!.ToLower() == value.ToLower() || x.Email == value.ToLower());

        return member != null
            ? new MemberUserResult<MemberUser> { Succeeded = true, StatusCode = 200, Data = MemberUserFactory.CreateModelFromEntity(member) }
            : new MemberUserResult<MemberUser> { Succeeded = false, StatusCode = 404, ErrorMessage = "No member was found." };
    }

    public async Task<MemberUserResult<MemberUser>> GetMemberUserAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return new MemberUserResult<MemberUser> { Succeeded = false, StatusCode = 400, ErrorMessage = "Invalid id provided." };
        }

        var member = await _userManager.Users
            .Include(x => x.Address)
            .Include(x => x.Picture)
            .FirstOrDefaultAsync(x => x.Id == id);

        return member != null
            ? new MemberUserResult<MemberUser> { Succeeded = true, StatusCode = 200, Data = MemberUserFactory.CreateModelFromEntity(member) }
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
                
        try
        {
            await _memberRepository.BeginTransactionAsync();

            var memberToUpdate = await _userManager.Users
                .Include(x => x.Picture)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (memberToUpdate == null)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = "Member to update was not found." };

            MemberUserFactory.UpdateEntityFromDto(memberToUpdate, dto);

            var result = await _userManager.UpdateAsync(memberToUpdate);

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


    public async Task<MemberUserResult<bool>> DeleteAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Invalid member provided." };

            await _memberRepository.BeginTransactionAsync();

            var memberToDelete = await _userManager.Users
                .Include(x => x.Picture)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (memberToDelete == null)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 404, ErrorMessage = "Member not found." };

            var result = await _userManager.DeleteAsync(memberToDelete);

            if (!result.Succeeded)
                return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Unable to delete member.", Data = false };

            if (memberToDelete.Picture != null)
            {
                var pictureResult = await _pictureRepository.DeleteAsync(memberToDelete.Picture);
                if (!pictureResult.Success)
                    return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "Unable to delete picture.", Data = false };
            }
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
