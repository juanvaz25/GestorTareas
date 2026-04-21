namespace GestorTareas.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>(); //Para manejar navegacion de datos
    }
}
