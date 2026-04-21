using GestorTareas.Repositories;
using GestorTareas.DTOs;
using GestorTareas.Models;

namespace GestorTareas.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(string? estado = null)
        {
            // Validar estado si viene en el query string
            if (!string.IsNullOrWhiteSpace(estado) && !EstadoTarea.EsValido(estado))
                throw new ArgumentException($"Estado inválido: '{estado}'. " +
                    $"Valores aceptados: {string.Join(", ", EstadoTarea.Todos)}");

            var tasks = await _repository.GetAllAsync(estado);
            return tasks.Select(ToDto);
        }

        public async Task<TaskResponseDto?> GetByIdAsync(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            return task is null ? null : ToDto(task);
        }

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
        {
            if (!EstadoTarea.EsValido(dto.Estado))
                throw new ArgumentException($"Estado inválido: '{dto.Estado}'.");

            if (!await _repository.UserExistsAsync(dto.IdUsuario))
                throw new KeyNotFoundException($"No existe un usuario con Id {dto.IdUsuario}.");

            var task = new TaskItem
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Estado = dto.Estado.ToLower(),
                IdUsuario = dto.IdUsuario
            };

            var created = await _repository.CreateAsync(task);
            return ToDto(created);
        }

        public async Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto)
        {
            if (!EstadoTarea.EsValido(dto.Estado))
                throw new ArgumentException($"Estado inválido: '{dto.Estado}'.");

            if (!await _repository.UserExistsAsync(dto.IdUsuario))
                throw new KeyNotFoundException($"No existe un usuario con Id {dto.IdUsuario}.");

            var task = new TaskItem
            {
                Id = id,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Estado = dto.Estado.ToLower(),
                IdUsuario = dto.IdUsuario
            };

            var updated = await _repository.UpdateAsync(task);
            return updated is null ? null : ToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _repository.DeleteAsync(id);

        // ── Mapper privado ───────────────────────────────────────────────────────
        private static TaskResponseDto ToDto(TaskItem t) => new(
            t.Id,
            t.Titulo,
            t.Descripcion,
            t.Estado,
            t.IdUsuario,
            t.Usuario?.Nombre ?? string.Empty,
            t.FechaCreacion
        );
    }
}
