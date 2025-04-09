using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository)
{
    private readonly IClientRepository _clientRepository = clientRepository;


    public async Task<ClientResult<bool>> CreateAsync(ClientFormData formData)
    {
        if (formData == null) 
            return new ClientResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed." };

        var entity = formData.MapTo<ClientEntity>();
        var exists = await _clientRepository.ExistsAsync(x => x.ClientName == entity.ClientName);

        if (exists.Success)
            return new ClientResult<bool> { Succeeded = false, StatusCode = exists.StatusCode, ErrorMessage = $"{formData.ClientName} already exists." };

        try
        {
            await _clientRepository.BeginTransactionAsync();

            var result = await _clientRepository.CreateAsync(entity);
            await _clientRepository.CommitTransactionAsync();

            return result.Success
                ? new ClientResult<bool> { Succeeded = true, StatusCode = 201 }
                : new ClientResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to create client." };
        }
        catch (Exception ex)
        {
            var rollback = await _clientRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ClientResult<bool> { Succeeded = false, StatusCode = rollback.StatusCode, ErrorMessage = $"Failed to create client: {ex.Message} " };
        }
    }


    public async Task<ClientResult<IEnumerable<Client>>> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync(
            orderByDescending: false,
            orderBy: x => x.ClientName,
            filterBy: null!);      


        return result.Success
            ? new ClientResult<IEnumerable<Client>> { Succeeded = true, StatusCode = 200, Data = result.MapTo<IEnumerable<Client>>() }
            : new ClientResult<IEnumerable<Client>> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "No clients was found." };
    }


    public async Task<ClientResult<Client>> GetClientAsync(Guid id)
    {
        var result = await _clientRepository.GetAsync(
            filterBy: x => x.Id == id,
            includes: null!);

        return result.Success
            ? new ClientResult<Client> { Succeeded = true, StatusCode = 200, Data = result.Data?.MapTo<Client>() }
            : new ClientResult<Client> { Succeeded = false, StatusCode = 404, ErrorMessage = "No client was found." };
    }


    public async Task<ClientResult<bool>> UpdateAsync(ClientFormData formData)
    {
        if (formData == null)
            return new ClientResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed." };

        var entity = formData.MapTo<ClientEntity>();

        try
        {
            await _clientRepository.BeginTransactionAsync();

            var result = await _clientRepository.UpdateAsync(entity);
            
            if ( !result.Success)
                return new ClientResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to update client." };
            await _clientRepository.CommitTransactionAsync();

            return new ClientResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            var rollback = await _clientRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ClientResult<bool> { Succeeded = false, StatusCode = rollback.StatusCode, ErrorMessage = $"Failed to update client: {ex.Message} " };
        }
    }


    public async Task<ClientResult<bool>> DeleteAsync(ClientFormData formData)
    {
        var entity = formData.MapTo<ClientEntity>();

        try
        {
            await _clientRepository.BeginTransactionAsync();

            var result = await _clientRepository.DeleteAsync(entity);
            if (!result.Success)
                return new ClientResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to delete client." };
           
            await _clientRepository.CommitTransactionAsync();

            return new ClientResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            var rollback = await _clientRepository.RollbackTransactionAsync();
            Debug.WriteLine($"**********\n{ex.Message}\n**********");
            return new ClientResult<bool> { Succeeded = false, StatusCode = rollback.StatusCode, ErrorMessage = $"Failed to delete client: {ex.Message} " };
        }
    }
}
