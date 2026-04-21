using GestorTareas.DTOs;

namespace GestorTareas.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllAsync(string? estado = null);
        Task<TaskResponseDto?> GetByIdAsync(int id);
        Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
        Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
