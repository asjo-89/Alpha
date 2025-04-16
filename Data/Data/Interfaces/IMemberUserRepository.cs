using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IMemberUserRepository : IBaseRepository<MemberUserEntity, MemberUser>
{
    Task<MemberUserEntity> GetAddressAsync(Guid id);
}
