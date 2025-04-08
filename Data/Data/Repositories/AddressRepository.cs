using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class AddressRepository(AlphaDbContext context) : BaseRepository<AddressEntity, Address>(context)
{
}
