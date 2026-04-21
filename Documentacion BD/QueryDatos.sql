USE GestorTareas;
GO

-- ============================================================
--  DATOS DE PRUEBA
-- ============================================================

INSERT INTO dbo.Users (Nombre, Email) VALUES
    ('Ana García',      'ana.garcia@example.com'),
    ('Carlos López',    'carlos.lopez@example.com'),
    ('María Fernández', 'maria.fernandez@example.com');
GO

-- ------------------------------------------------------------
--  Tareas (mínimo 6, distribuidas entre usuarios y estados)
-- ------------------------------------------------------------
INSERT INTO dbo.Tasks (Titulo, Descripcion, Estado, IdUsuario) VALUES
    -- Tareas de Ana (Id = 1)
    ('Revisar documentación técnica',
     'Leer y verificar los requisitos del nuevo módulo de reportes.',
     'completada', 1),

    ('Configurar entorno de desarrollo',
     'Instalar .NET 8 SDK, SQL Server Express y Node.js en la nueva máquina.',
     'completada', 1),

    -- Tareas de Carlos (Id = 2)
    ('Implementar API REST de tareas',
     'Desarrollar los endpoints GET, POST, PUT y DELETE para el recurso /api/tasks.',
     'en progreso', 2),

    ('Escribir pruebas unitarias del backend',
     'Cubrir los servicios TaskService y UserService con xUnit.',
     'pendiente', 2),

    -- Tareas de María (Id = 3)
    ('Desarrollar interfaz React',
     'Crear los componentes TaskList, TaskForm y TaskFilter con hooks personalizados.',
     'en progreso', 3),

    ('Documentar el proyecto en README',
     'Redactar instrucciones de instalación, variables de entorno y decisiones de diseño.',
     'pendiente', 3),

    -- Tarea adicional distribuida
    ('Configurar Swagger / OpenAPI',
     'Integrar Swashbuckle en el proyecto ASP.NET Core y documentar todos los endpoints.',
     'pendiente', 2);
GO

-- ============================================================
--  VERIFICACIÓN
-- ============================================================
SELECT 'Users' AS Tabla, COUNT(*) AS Registros FROM dbo.Users
UNION ALL
SELECT 'Tasks',          COUNT(*)               FROM dbo.Tasks;

SELECT
    t.Id,
    t.Titulo,
    t.Estado,
    u.Nombre AS Usuario,
    t.FechaCreacion
FROM dbo.Tasks t
INNER JOIN dbo.Users u ON u.Id = t.IdUsuario
ORDER BY t.Estado, t.Id;
GO