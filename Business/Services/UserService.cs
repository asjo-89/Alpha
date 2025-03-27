using Business.Dtos;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class UserService(IBaseRepository<MemberUserEntity> repository, IBaseRepository<AddressEntity> addressRepository, IBaseRepository<PictureEntity> pictureRepository)
{
    private readonly IBaseRepository<MemberUserEntity> _repository = repository;
    private readonly IBaseRepository<AddressEntity> _addressRepository = addressRepository;
    private readonly IBaseRepository<PictureEntity> _pictureRepository = pictureRepository;

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
                Email = form.Email,
                PhoneNumber = form.PhoneNumber,
                AddressId = address.Id,
                DateOfBirth = form.DateOfBirth,
                PictureId = image.Id
            };

            bool result = await _repository.CreateAsync(member);
            if (!result) return null!;

            await _repository.SaveChangesAsync();
            await _repository.CommitTransactionAsync();

            var entity = await _repository.GetOneAsync(x => x.Email == member.Email);
            var addressEntity = await _addressRepository.GetOneAsync(x => x.Id == entity.AddressId);
            var pictureEntity = await _pictureRepository.GetOneAsync(x => x.Id == entity.PictureId);

            var newMember = new MemberModel()
            {
                Id = Guid.Parse(entity.Id),
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
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

}
