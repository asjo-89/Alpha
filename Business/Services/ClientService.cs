using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<ClientResult<Client>> CreateAsync(string name)
    {
        if (name == null)
            return new ClientResult<Client> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields must be completed." };
        
        var entity = new ClientEntity
        {
            ClientName = name,
            Email = "client@domain.com"
        };
        var exists = await _clientRepository.ExistsAsync(x => x.ClientName == entity.ClientName);

        if (exists.Success)
            return new ClientResult<Client> { Succeeded = false, StatusCode = 409, ErrorMessage = $"{name} already exists." };

        try
        {
            await _clientRepository.BeginTransactionAsync();

            var result = await _clientRepository.CreateAsync(entity);

            if (!result.Success)
                return new ClientResult<Client> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Failed to create client." };

            await _clientRepository.CommitTransactionAsync();

            var client = await _clientRepository.GetAsync(
                filterBy: x => x.ClientName == entity.ClientName);

            return new ClientResult<Client> { Succeeded = true, StatusCode = 201, Data = ClientFactory.CreateModelFromEntity(client.Data) };
        }
        catch (Exception ex)
        {
            var rollback = await _clientRepository.RollbackTransactionAsync();
            Console.WriteLine($"**********\n{ex.Message}\n**********");
            return new ClientResult<Client> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to create client: {ex.Message} " };
        }
    }


    public async Task<ClientResult<IEnumerable<Client>>> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync(
            orderByDescending: false,
            orderBy: x => x.ClientName,
            filterBy: null!);


        return result.Success
            ? new ClientResult<IEnumerable<Client>> { Succeeded = true, StatusCode = 200, Data = result.Data?.Select(ClientFactory.CreateModelFromEntity) }
            : new ClientResult<IEnumerable<Client>> { Succeeded = false, StatusCode = 404, ErrorMessage = "No clients was found." };
    }


    public async Task<ClientResult<Client>> GetClientAsync(string value)
    {
        var result = await _clientRepository.GetAsync(
            filterBy: x => x.ClientName.ToLower() == value.ToLower() && x.Email == value.ToLower(),
            includes: null!);

        return result.Success
            ? new ClientResult<Client> { Succeeded = true, StatusCode = 200, Data = ClientFactory.CreateModelFromEntity(result.Data!) }
            : new ClientResult<Client> { Succeeded = false, StatusCode = 404, ErrorMessage = "No client was found." };
    }


    //public async Task<ClientResult<bool>> UpdateAsync(ClientDto formData)
    //{
    //    if (formData == null)
    //        return new ClientResult<bool> { Succeeded = false, StatusCode = 400, ErrorMessage = "All reaquired fields are not completed.", Data = false };

    //    var entity = formData.MapTo<ClientEntity>();

    //    try
    //    {
    //        await _clientRepository.BeginTransactionAsync();

    //        var result = await _clientRepository.UpdateAsync(entity);

    //        if (!result.Success)
    //            return new ClientResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to update client.", Data = false };
    //        await _clientRepository.CommitTransactionAsync();

    //        return new ClientResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
    //    }
    //    catch (Exception ex)
    //    {
    //        var rollback = await _clientRepository.RollbackTransactionAsync();
    //        Debug.WriteLine($"**********\n{ex.Message}\n**********");
    //        return new ClientResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to update client: {ex.Message}", Data = false };
    //    }
    //}


    //public async Task<ClientResult<bool>> DeleteAsync(ClientDto formData)
    //{
    //    var entity = formData.MapTo<ClientEntity>();

    //    try
    //    {
    //        await _clientRepository.BeginTransactionAsync();

    //        var result = await _clientRepository.DeleteAsync(entity);
    //        if (!result.Success)
    //            return new ClientResult<bool> { Succeeded = false, StatusCode = result.StatusCode, ErrorMessage = "Unable to delete client.", Data = false };

    //        await _clientRepository.CommitTransactionAsync();

    //        return new ClientResult<bool> { Succeeded = true, StatusCode = 200, Data = true };
    //    }
    //    catch (Exception ex)
    //    {
    //        var rollback = await _clientRepository.RollbackTransactionAsync();
    //        Debug.WriteLine($"**********\n{ex.Message}\n**********");
    //        return new ClientResult<bool> { Succeeded = false, StatusCode = 500, ErrorMessage = $"Failed to delete client: {ex.Message}", Data = false };
    //    }
    //}
}
