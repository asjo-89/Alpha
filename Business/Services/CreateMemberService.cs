﻿using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class CreateMemberService(IBaseRepository<MemberUserEntity> repository, IBaseRepository<AddressEntity> addressRepository, IBaseRepository<PictureEntity> pictureRepository) : ICreateMemberService
{
    private readonly IBaseRepository<MemberUserEntity> _repository = repository;
    private readonly IBaseRepository<AddressEntity> _addressRepository = addressRepository;
    private readonly IBaseRepository<PictureEntity> _pictureRepository = pictureRepository;


    public async Task<MemberModel> AddMember(CreateMemberRegForm form)
    {
        if (form == null)
        {
            Console.Error.WriteLine("Incomplete form data.");
            return null!;
        }

        var image = new PictureEntity()
        {
            PictureUrl = form.ProfileImage
        };
        var address = new AddressEntity()
        {
            StreetName = form.StreetAddress,
            PostalCode = form.PostalCode,
            City = form.City
        };

        


        try
        {


            await _pictureRepository.BeginTransactionAsync();
            await _pictureRepository.CreateAsync(image);
            await _pictureRepository.SaveChangesAsync();
            await _pictureRepository.CommitTransactionAsync();

            await _addressRepository.BeginTransactionAsync();
            await _addressRepository.CreateAsync(address);
            await _addressRepository.SaveChangesAsync();
            await _addressRepository.CommitTransactionAsync();

            await _repository.BeginTransactionAsync();

            var member = new MemberUserEntity()
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                EmailAddress = form.EmailAddress,
                PhoneNumber = form.PhoneNumber,
                AddressId = address.Id,
                DateOfBirth = form.DateOfBirth,
                PictureId = image.Id
            };

            bool result = await _repository.CreateAsync(member);
            if (!result) return null!;

            await _repository.SaveChangesAsync();
            await _repository.CommitTransactionAsync();

            var entity = await _repository.GetOneAsync(x => x.EmailAddress == member.EmailAddress);
            var addressEntity = await _addressRepository.GetOneAsync(x => x.Id == entity.AddressId);
            var pictureEntity = await _pictureRepository.GetOneAsync(x => x.Id == entity.PictureId);

            var newMember = new MemberModel()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.EmailAddress,
                Phone = entity.PhoneNumber,
                StreetAddress = addressEntity.StreetName,
                PostalCode = addressEntity.PostalCode,
                City = addressEntity.City,
                DateOfBirth = entity.DateOfBirth,
                ProfileImage = pictureEntity.PictureUrl
            };

            return newMember != null ? newMember : null!;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create member: {ex.Message}");
            await _repository.RollbackTransactionAsync();
            return null!;
        }
    }

    public Task<bool> DeleteMember(MemberModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MemberModel>> GetAllMembers()
    {
        throw new NotImplementedException();
    }

    public Task<MemberModel> GetMember(MemberModel model)
    {
        throw new NotImplementedException();
    }

    public Task<MemberModel> UpdateMember(MemberModel model)
    {
        throw new NotImplementedException();
    }
}
