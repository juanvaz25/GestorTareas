using GestorTareas.Models;
using Microsoft.EntityFrameworkCore;


namespace GestorTareas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Users ────────────────────────────────────────────────────────────
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(u => u.Id);
                e.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
                e.Property(u => u.Email).IsRequired().HasMaxLength(150);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.FechaCreacion)
                 .HasDefaultValueSql("GETUTCDATE()")
                 .ValueGeneratedOnAdd();
            });

            // ── Tasks ────────────────────────────────────────────────────────────
            modelBuilder.Entity<TaskItem>(e =>
            {
                e.ToTable("Tasks");
                e.HasKey(t => t.Id);
                e.Property(t => t.Titulo).IsRequired().HasMaxLength(200);
                e.Property(t => t.Descripcion).HasMaxLength(1000);
                e.Property(t => t.Estado)
                 .IsRequired()
                 .HasMaxLength(20)
                 .HasDefaultValue("pendiente");
                e.Property(t => t.FechaCreacion)
                 .HasDefaultValueSql("GETUTCDATE()")
                 .ValueGeneratedOnAdd();

                e.HasOne(t => t.Usuario)
                 .WithMany(u => u.Tasks)
                 .HasForeignKey(t => t.IdUsuario)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(t => t.Estado);
                e.HasIndex(t => t.IdUsuario);
            });
        }
    }
}
