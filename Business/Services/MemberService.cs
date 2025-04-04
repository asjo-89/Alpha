using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Business.Services;

public class MemberService(IBaseRepository<MemberUserEntity> repository, 
    IBaseRepository<PictureEntity> picRepository, IAddressRepository addressRepository, 
    IPictureRepository pictureRepository, UserManager<MemberUserEntity> userManager, 
    RoleManager<IdentityRole<Guid>> roleManager) : IMemberService
{

    private readonly IBaseRepository<MemberUserEntity> _repository = repository;
    private readonly IBaseRepository<PictureEntity> _picRepository = picRepository;
    private readonly IAddressRepository _addressRepository = addressRepository;
    private readonly IPictureRepository _pictureRepository = pictureRepository;


    private readonly UserManager<MemberUserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;


    public async Task<MemberModel> AddMemberAsync(CreateMemberRegForm form)
    {
        if (form == null) return null!;

        try
        {
            bool exists = await _repository.ExistsAsync(x => x.Email == form.Email);
            var address = await _addressRepository.GetOrAddAsync(form.StreetAddress, form.PostalCode, form.City);
            var picture = await _pictureRepository.GetOrAddAsync(form.ProfileImage);
            var role = await _roleManager.FindByNameAsync(form.JobTitle);

            if (!exists && address != null && picture != null && role != null && role.Name != null)
            {

                MemberUserEntity entity = MemberFactory.CreateEntityFromDto(form, address, picture);

                await _repository.BeginTransactionAsync();
                var result = await _userManager.CreateAsync(entity);
                var roleAdded = await _userManager.AddToRoleAsync(entity, role.Name);

                if (result.Succeeded)
                {
                    await _repository.SaveChangesAsync();
                    await _repository.CommitTransactionAsync();

                    MemberModel member = MemberFactory.CreateModelFromEntity(entity, role.Name);
                    return member;
                }
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


    public async Task<IEnumerable<MemberModel>> GetAllMembersAsync()
    {
        var entities = await _repository.GetAllAsync();
        var members = new List<MemberModel>();

        var pictures = await _picRepository.GetAllAsync();


        foreach (var entity in entities)
        {
            var roles = await _userManager.GetRolesAsync(entity);

            var picture = await _picRepository.GetOneAsync(x => x.Id == entity.PictureId);

            var jobTitle = roles.FirstOrDefault() ?? "No role assigned";

            members.Add(MemberFactory.CreateModelFromEntity(entity, jobTitle));
        }
        return members;
    }


    public async Task<MemberModel> GetMemberAsync(Expression<Func<MemberUserEntity, bool>> expression)
    {
        MemberUserEntity? entity = await _repository.GetOneAsync(expression);
        if (entity == null) return null!;

        return MemberFactory.CreateModelFromEntity(entity);
    }


    public async Task<bool> ExistsAsync(Expression<Func<MemberUserEntity, bool>> expression)
    {
        return await _repository.ExistsAsync(expression);
    }


    public async Task<MemberModel> UpdateMember(MemberModel model)
    {
        if (model == null) return null!;

        try
        {
            bool exists = await _repository.ExistsAsync(x => x.Email == model.Email);

            if (!exists)
            {
                var address = await _addressRepository.GetOrAddAsync(model.StreetAddress, model.PostalCode, model.City);
                var picture = await _pictureRepository.GetOrAddAsync(model.ProfileImage);
                var role = await _roleManager.FindByNameAsync(model.JobTitle);
                MemberUserEntity? memberToUpdate = await _repository.GetOneAsync(x => x.Email == model.Email);

                if (memberToUpdate == null || address == null || picture == null || role == null || role.Name == null) return null!;

                memberToUpdate = MemberFactory.CreateEntityFromModel(model, address, picture);
                var addedToRole = await _userManager.AddToRoleAsync(memberToUpdate, role.Name);

                if (addedToRole == null || memberToUpdate == null) return null!;

                await _repository.BeginTransactionAsync();

                var result = await _userManager.UpdateAsync(memberToUpdate);

                await _repository.SaveChangesAsync();
                await _repository.CommitTransactionAsync();

                MemberModel member = MemberFactory.CreateModelFromEntity(memberToUpdate, model.JobTitle);
                return member ?? null!;
            }
            return null!;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to update member: {ex.Message}");
            await _repository.RollbackTransactionAsync();
            return null!;
        }
    }


    public async Task<bool> DeleteMember(Guid id)
    {
        var entity = await _repository.GetOneAsync(x => x.Id == id);
        if (entity == null) return false;

        try
        {
            await _repository.BeginTransactionAsync();

            var result = _repository.DeleteAsync(entity);
            
            await _repository.SaveChangesAsync();
            await _repository.CommitTransactionAsync();
            return result;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to delete member: {ex.Message}");
            await _repository.RollbackTransactionAsync();
            return false;
        }
    }
}
