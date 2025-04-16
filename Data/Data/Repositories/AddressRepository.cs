using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Data.Repositories;

public class AddressRepository(AlphaDbContext context) : BaseRepository<AddressEntity, Address>(context), IAddressRepository
{
}
