using Business.Dtos;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<int> CreateAsync(CreateAccountRegForm form);

    Task<bool> ExistsAsync(string email);
}
