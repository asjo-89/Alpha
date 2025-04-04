using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class UserService(IBaseRepository<MemberUserEntity> repository, IAddressRepository addressRepository, IPictureRepository pictureRepository) : IUserService
{
    private readonly IBaseRepository<MemberUserEntity> _repository = repository;
    private readonly IAddressRepository _addressRepository = addressRepository;
    private readonly IPictureRepository _pictureRepository = pictureRepository;

    public async Task<MemberModel> AddUser(CreateMemberRegForm form)
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
            await _repository.BeginTransactionAsync();

            var userEntity = new MemberUserEntity()
            {
                UserName = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                PhoneNumber = form.PhoneNumber
            };

            bool result = await _repository.CreateAsync(userEntity);
            if (!result) return null!;

            await _repository.SaveChangesAsync();
            await _repository.CommitTransactionAsync();

            MemberModel newUser = new MemberModel()
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                PhoneNumber = userEntity.PhoneNumber,
            };

            return newUser != null ? newUser : null!;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create member: {ex.Message}");
            await _repository.RollbackTransactionAsync();
            return null!;
        }
    }

}
