USE master;
GO

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'GestorTareas')
BEGIN
    CREATE DATABASE GestorTareas;
END
GO

USE GestorTareas;
GO

-- ============================================================
--  TABLAS
-- ============================================================

-- Eliminar tablas si existen (para re-ejecución limpia)
IF OBJECT_ID('dbo.Tasks', 'U') IS NOT NULL DROP TABLE dbo.Tasks;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
GO

-- ------------------------------------------------------------
--  Tabla: Users
-- ------------------------------------------------------------
CREATE TABLE dbo.Users (
    Id          INT             NOT NULL IDENTITY(1,1),
    Nombre      NVARCHAR(100)   NOT NULL,
    Email       NVARCHAR(150)   NOT NULL,
    FechaCreacion DATETIME2     NOT NULL CONSTRAINT DF_Users_FechaCreacion DEFAULT GETUTCDATE(),

    CONSTRAINT PK_Users PRIMARY KEY (Id),
    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);
GO

-- ------------------------------------------------------------
--  Tabla: Tasks
-- ------------------------------------------------------------
CREATE TABLE dbo.Tasks (
    Id              INT             NOT NULL IDENTITY(1,1),
    Titulo          NVARCHAR(200)   NOT NULL,
    Descripcion     NVARCHAR(1000)  NULL,
    Estado          NVARCHAR(20)    NOT NULL CONSTRAINT DF_Tasks_Estado DEFAULT 'pendiente',
    IdUsuario       INT             NOT NULL,
    FechaCreacion   DATETIME2       NOT NULL CONSTRAINT DF_Tasks_FechaCreacion DEFAULT GETUTCDATE(),

    CONSTRAINT PK_Tasks PRIMARY KEY (Id),
    CONSTRAINT FK_Tasks_Users FOREIGN KEY (IdUsuario) REFERENCES dbo.Users(Id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT CK_Tasks_Estado CHECK (Estado IN ('pendiente', 'en progreso', 'completada'))
);
GO

-- Índice para búsquedas frecuentes por estado y usuario
CREATE INDEX IX_Tasks_Estado    ON dbo.Tasks (Estado);
CREATE INDEX IX_Tasks_IdUsuario ON dbo.Tasks (IdUsuario);
GO
