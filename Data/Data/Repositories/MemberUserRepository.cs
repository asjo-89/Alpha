using Data.Contexts;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public class MemberUserRepository(AlphaDbContext context) : BaseRepository<MemberUserEntity, MemberUser>(context)
{
}
