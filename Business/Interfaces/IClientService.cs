using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IClientService
    {
        Task<ClientResult<bool>> CreateAsync(ClientFormData formData);
        Task<ClientResult<bool>> DeleteAsync(ClientFormData formData);
        Task<ClientResult<Client>> GetClientAsync(string value);
        Task<ClientResult<IEnumerable<Client>>> GetClientsAsync();
        Task<ClientResult<bool>> UpdateAsync(ClientFormData formData);
    }
}