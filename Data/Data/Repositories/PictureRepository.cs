using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class PictureRepository(AlphaDbContext context) : BaseRepository<PictureEntity>(context), IPictureRepository
{
    public async Task<PictureEntity> GetOrAddAsync(string url)
    {
        var pic = await _entities.FirstOrDefaultAsync(x => x.PictureUrl == url);

        if (pic == null)
        {
            PictureEntity pictureEntity = new PictureEntity()
            {
                PictureUrl = url
            };

            var addedPicture = await _entities.AddAsync(pictureEntity);
            return addedPicture.Entity ?? null!;
        }

        return pic;
    }
}
