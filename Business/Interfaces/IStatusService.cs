using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IStatusService
{
    Task<StatusResult<bool>> CreateAsync(StatusFormData formData);
    Task<StatusResult<bool>> DeleteAsync(StatusFormData formData);
    Task<StatusResult<Status>> GetClientAsync(string value);
    Task<StatusResult<IEnumerable<Status>>> GetClientsAsync();
    Task<StatusResult<bool>> UpdateAsync(StatusFormData formData);
}