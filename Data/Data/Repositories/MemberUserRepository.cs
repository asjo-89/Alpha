using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class MemberUserRepository(AlphaDbContext context) : BaseRepository<MemberUserEntity, MemberUser>(context), IMemberUserRepository
{
    public async Task<MemberUserEntity> GetAddressAsync(Guid id)
    {
        var entity = await _entity.FirstOrDefaultAsync(x => x.Id == id);
        return entity ?? new MemberUserEntity();
    }
}
