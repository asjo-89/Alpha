﻿using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class ClientRepository(AlphaDbContext context) : BaseRepository<ClientEntity, Client>(context), IClientRepository
{
}
