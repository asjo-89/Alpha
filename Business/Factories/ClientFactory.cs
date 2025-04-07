using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ClientFactory
{
    public static ClientEntity CreateEntityFromDto(ClientRegForm dto)
    {
        ClientEntity entity = new()
        {
            ClientName = dto.ClientName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email ?? ""
        };
        return entity;
    }

    public static ClientModel CreateModelFromEntity(ClientEntity entity)
    {
        ClientModel model = new()
        {
            ClientName = entity.ClientName,
            PhoneNumber = entity.PhoneNumber,
            Email = entity.Email ?? ""
        };
        return model;
    }

    public static ClientEntity CreateEntityFromModel(ClientModel model)
    {
        ClientEntity entity = new()
        {
            ClientName = model.ClientName,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email ?? ""
        };
        return entity;
    }
}
