using System.ComponentModel.DataAnnotations;

namespace GestorTareas.DTOs
{
    // ── Respuesta (salida)

    public record TaskResponseDto(
        int Id,
        string Titulo,
        string? Descripcion,
        string Estado,
        int IdUsuario,
        string NombreUsuario,
        DateTime FechaCreacion
    );

    // ── Creación (entrada)

    public record CreateTaskDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El título no puede superar los 200 caracteres.")]
        public string Titulo { get; init; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres.")]
        public string? Descripcion { get; init; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; init; } = "pendiente";

        [Required(ErrorMessage = "El id de usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "IdUsuario debe ser mayor a 0.")]
        public int IdUsuario { get; init; }
    }

    // ── Actualización (entrada)

    public record UpdateTaskDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [MaxLength(200)]
        public string Titulo { get; init; } = string.Empty;

        [MaxLength(1000)]
        public string? Descripcion { get; init; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; init; } = "pendiente";

        [Required]
        [Range(1, int.MaxValue)]
        public int IdUsuario { get; init; }
    }
}
