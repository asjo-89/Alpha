using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class MemberService(IBaseRepository<MemberUserEntity> repository, IBaseRepository<AddressEntity> addressRepository, IBaseRepository<PictureEntity> pictureRepository, UserManager<MemberUserEntity> userManager) : IMemberService
{
    private readonly IBaseRepository<MemberUserEntity> _repository = repository;
    private readonly IBaseRepository<AddressEntity> _addressRepository = addressRepository;
    private readonly IBaseRepository<PictureEntity> _pictureRepository = pictureRepository;

    private readonly UserManager<MemberUserEntity> _userManager = userManager;


    public async Task<MemberModel> AddMember(CreateMemberRegForm form)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMember(MemberModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<MemberModel>> GetAllMembers()
    {
        var entities = await _repository.GetAllAsync();
        var members = new List<MemberModel>();
        foreach (var entity in entities)
        {

            var role = await _userManager.GetRolesAsync(entity);

            var jobTitle = role.FirstOrDefault() ?? "No role assigned";

            members.Add(new MemberModel()
            {
                Id = Guid.Parse(entity.Id),
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                JobTitle = jobTitle,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                ProfileImage = entity.Picture?.PictureUrl ?? ""
            });
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
}
