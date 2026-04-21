using GestorTareas.Data;
using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorTareas.API.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(string? estado = null)
    {
        var query = _context.Tasks.Include(t => t.Usuario).AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(t => t.Estado == estado.ToLower());

        return await query.OrderBy(t => t.FechaCreacion).ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id) =>
        await _context.Tasks
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Recargar con navegación
        await _context.Entry(task).Reference(t => t.Usuario).LoadAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateAsync(TaskItem task)
    {
        var existing = await _context.Tasks.FindAsync(task.Id);
        if (existing is null) return null;

        existing.Titulo = task.Titulo;
        existing.Descripcion = task.Descripcion;
        existing.Estado = task.Estado;
        existing.IdUsuario = task.IdUsuario;

        await _context.SaveChangesAsync();
        await _context.Entry(existing).Reference(t => t.Usuario).LoadAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task is null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UserExistsAsync(int userId) =>
        await _context.Users.AnyAsync(u => u.Id == userId);
}