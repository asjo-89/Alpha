using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class StatusFactory
{
    public static StatusEntity CreateEntityFromDto (StatusRegForm dto)
    {
        StatusEntity entity = new()
        {
            StatusName = dto.StatusName
        };
        return entity;
    }

    public static StatusModel CreateModelFromEntity(StatusEntity entity)
    {
        StatusModel model = new()
        {
            Id = entity.Id,
            StatusName = entity.StatusName
        };
        return model;
    }

    public static StatusEntity CreateEntityFromModel(StatusModel model)
    {
        StatusEntity entity = new()
        {
            Id = model.Id,
            StatusName = model.StatusName
        };
        return entity;
    }
}
