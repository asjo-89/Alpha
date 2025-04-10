﻿using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Business.Services;

public class MemberUserService(IMemberUserRepository memberRepository, UserManager<MemberUserEntity> userManager) : IMemberUserService
{
    private readonly IMemberUserRepository _memberRepository = memberRepository;
    private readonly UserManager<MemberUserEntity> _userManager = userManager;



    public async Task<MemberUserResult<bool>> CreateAsync(MemberUserFormData formData)
    {
        if (formData == null)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields must be completed.", Data = false };

        var entity = formData.MapTo<MemberUserEntity>();
        var exists = await _memberRepository.ExistsAsync(x => x.Email == entity.Email);

        if (exists.Success)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 409, ErrorMessage = $"Member with email address {formData.Email} already exists.", Data = false };

        try
        {
            await _memberRepository.BeginTransactionAsync();

            var result = await _userManager.CreateAsync(entity);

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


    public async Task<MemberUserResult<bool>> CreateUserAsync(CreateUserFormData formData)
    {
        if (formData == null)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields must be completed.", Data = false };

        var entity = formData.MapTo<MemberUserEntity>();
        entity.UserName = formData.Email;
        var exists = await _memberRepository.ExistsAsync(x => x.Email == entity.Email);

        if (exists.Success)
            return new MemberUserResult<bool> { Succeeded = false, StatusCode = 409, ErrorMessage = $"Member with email address {formData.Email} already exists.", Data = false };

        try
        {
            await _memberRepository.BeginTransactionAsync();

            var result = await _userManager.CreateAsync(entity);

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


    public async Task<MemberUserResult<IEnumerable<MemberUser>>> GetMemberUsersAsync()
    {
        var result = await _memberRepository.GetAllAsync(
            orderByDescending: false,
            orderBy: x => new { x.FirstName, x.LastName },
            filterBy: null!);


        return result.Success
            ? new MemberUserResult<IEnumerable<MemberUser>> { Succeeded = true, StatusCode = 200, Data = result.MapTo<IEnumerable<MemberUser>>() }
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
