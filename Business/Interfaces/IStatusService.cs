using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IStatusService
{
    Task<StatusResult<bool>> CreateAsync(StatusDto formData);
    Task<StatusResult<bool>> DeleteAsync(StatusDto formData);
    Task<StatusResult<Status>> GetStatusAsync(string value);
    Task<StatusResult<IEnumerable<Status>>> GetStatusesAsync();
    Task<StatusResult<bool>> UpdateAsync(StatusDto formData);
}