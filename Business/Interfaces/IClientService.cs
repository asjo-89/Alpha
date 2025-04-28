using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<ClientResult<Client>> CreateAsync(string name);
        //Task<ClientResult<bool>> DeleteAsync(ClientDto formData);
        Task<ClientResult<Client>> GetClientAsync(string value);
        Task<ClientResult<IEnumerable<Client>>> GetClientsAsync();
        //Task<ClientResult<bool>> UpdateAsync(ClientDto formData);
    }
}