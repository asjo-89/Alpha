using Data.Entities;
using Domain.Models;

namespace Data.Factories;

public static class PictureFactoryMVC
{
    public static PictureEntity CreateEntityFromModel(Picture model)
    {
        return new PictureEntity
        {
            ImageUrl = model.ImageUrl
        };
    }

    public static Picture CreateModelFromEntity(PictureEntity entity)
    {
        return new Picture
        {
            Id = entity.Id,
            ImageUrl = entity.ImageUrl
        };
    }
}
