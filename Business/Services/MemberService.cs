using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Data.Entities;
using Data.Errors;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Business.Services;

public class MemberService(IBaseRepository<MemberUserEntity> repository, IBaseRepository<PictureEntity> picRepository, IAddressRepository addressRepository, IPictureRepository pictureRepository, UserManager<MemberUserEntity> userManager) : IMemberService
{
    private readonly IBaseRepository<MemberUserEntity> _repository = repository;
    private readonly IBaseRepository<PictureEntity> _picRepository = picRepository;
    private readonly IAddressRepository _addressRepository = addressRepository;
    private readonly IPictureRepository _pictureRepository = pictureRepository;


    private readonly UserManager<MemberUserEntity> _userManager = userManager;


    public async Task<MemberModel> AddMember(CreateMemberRegForm form)
    {
        if (form == null) return null!;

        bool exists = await _repository.ExistsAsync(x => x.Email == form.Email);

        

        try
        {
            var address = await _addressRepository.GetOrAddAsync(form.StreetAddress, form.PostalCode, form.City);
            if (address == null) return null!;

            var picture = await _pictureRepository.GetOrAddAsync(form.ProfileImage);
            if (picture == null) return null!;


            if (exists)
            {
                var updatedMember = await _repository.GetOneAsync(x => x.Email == form.Email);

                updatedMember.UserName = form.Email;
                updatedMember.FirstName = form.FirstName;
                updatedMember.LastName = form.LastName;
                updatedMember.Email = form.Email;
                updatedMember.PhoneNumber = form.PhoneNumber;
                updatedMember.JobTitle = form.JobTitle;
                updatedMember.DateOfBirth = form.DateOfBirth;
                updatedMember.AddressId = address.Id;
                updatedMember.PictureId = picture.Id;
                
                await _repository.BeginTransactionAsync();

                var result = _repository.UpdateAsync(updatedMember);

                await _repository.SaveChangesAsync();
                await _repository.CommitTransactionAsync();

                MemberModel member = MemberFactory.CreateModelFromEntity(updatedMember);
                return member;                
            }

            MemberUserEntity entity = MemberFactory.CreateEntityFromDto(form, address, picture);

            await _repository.BeginTransactionAsync();
            if (await _repository.CreateAsync(entity))
            {
                await _repository.SaveChangesAsync();
                await _repository.CommitTransactionAsync();

                MemberModel member = MemberFactory.CreateModelFromEntity(entity);
                return member;
            }
            return null!;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create member: {ex.Message}");
            await _repository.RollbackTransactionAsync();
            return null!;
        }
    }

    public async Task<IEnumerable<MemberModel>> GetAllMembers()
    {
        var entities = await _repository.GetAllAsync();
        var members = new List<MemberModel>();

        var pictures = await _picRepository.GetAllAsync();


        foreach (var entity in entities)
        {
            var role = await _userManager.GetRolesAsync(entity);

            var picture = await _picRepository.GetOneAsync(x => x.Id == entity.PictureId);

            var jobTitle = role.FirstOrDefault() ?? "No role assigned";

            members.Add(MemberFactory.CreateModelFromEntity(entity));
        }
        return members;
    }

    public Task<MemberModel> GetMember(MemberModel model)
    {
        throw new NotImplementedException();
    }

    public Task<MemberModel> UpdateMember(MemberModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMember(MemberModel model)
    {
        throw new NotImplementedException();
    }
}
