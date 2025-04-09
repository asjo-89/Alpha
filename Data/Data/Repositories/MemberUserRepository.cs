using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class MemberUserRepository(AlphaDbContext context) : BaseRepository<MemberUserEntity, MemberUser>(context), IMemberUserRepository
{
}
