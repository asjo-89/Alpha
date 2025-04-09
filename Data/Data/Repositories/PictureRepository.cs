using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class PictureRepository(AlphaDbContext context) : BaseRepository<PictureEntity, Picture>(context), IPictureRepository
{
}
