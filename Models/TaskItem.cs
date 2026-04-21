namespace GestorTareas.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = EstadoTarea.Pendiente;
        public int IdUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Navegación
        public User? Usuario { get; set; }
    }

    /// Constantes para los valores válidos de Estado.
    public static class EstadoTarea
    {
        public const string Pendiente = "pendiente";
        public const string EnProgreso = "en progreso";
        public const string Completada = "completada";

        public static readonly IReadOnlyList<string> Todos =
            new[] { Pendiente, EnProgreso, Completada };

        public static bool EsValido(string estado) =>
            Todos.Contains(estado, StringComparer.OrdinalIgnoreCase);
    }
}
