# Gestor de Tareas

Aplicación web fullstack para gestión de tareas desarrollada con tres estados

---

## Tecnologías utilizadas

| Capa | Tecnología |
|---|---|
| Frontend | React 18 + TypeScript + Vite |
| Backend | ASP.NET Core 9 Web API |
| Base de datos | SQL Server / LocalDB |
| ORM | Entity Framework Core 9 |
| Documentación API | Swagger / Swashbuckle |
| Control de versiones | Git + GitHub |

---

## Estructura del repositorio

```
GestorTareas/       # ASP.NET Core Web API
|                     
├── ├── Controllers/
│   │   └── TasksController.cs          # Endpoints REST
│   ├── Services/
│   │   ├── ITaskService.cs             # Interfaz
│   │   └── TaskService.cs              # Lógica de negocio
│   ├── Repositories/
│   │   ├── ITaskRepository.cs          # Interfaz
│   │   └── TaskRepository.cs           # Acceso a datos (EF Core)
│   ├── Models/
│   │   ├── User.cs
│   │   └── TaskItem.cs                 # Entidades + EstadoTarea
│   ├── DTOs/
│   │   └── TaskDtos.cs                 # CreateTaskDto, UpdateTaskDto, TaskResponseDto
│   ├── Data/
│   │   └── AppDbContext.cs             # DbContext con fluent API
│   ├── Middleware/
│   │   └── ErrorHandlingMiddleware.cs  # Retorno de Codigos HTTP
|   |    
│   ├── Program.cs                      # Composición raíz (DI, Swagger, CORS)
│   ├── appsettings.json                # Configuracion del connectionString
│
|
│
├── frontend/                       # React + TypeScript + Vite
│   ├── src/
│   │   ├── components/
│   │   │   ├── TaskCard.tsx
│   │   │   ├── TaskFilter.tsx
│   │   │   ├── TaskForm.tsx
│   │   │   └── TaskList.tsx
│   │   ├── hooks/
│   │   │   └── useTasks.ts         # Hook personalizado
│   │   ├── services/
│   │   │   └── taskService.ts      # Capa de acceso a la API
│   │   ├── types/
│   │   │   └── index.ts
│   │   ├── __tests__/
│   │   │   └── useTasks.test.ts
│   │   ├── App.tsx
│   │   ├── main.tsx
│   │   └── index.css
│   ├── index.html
│   ├── package.json
│   ├── vite.config.ts
│   └── tsconfig.json
│
├── database.sql                    # Script de BD (CREATE + INSERT)
└── README.md
```

---

## Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- [Node.js 18+](https://nodejs.org/)
- SQL Server Express o LocalDB  
  Verificar que LocalDB esté disponible: `sqllocaldb info`

---

## Instrucciones para ejecutar localmente

### 1. Clonar el repositorio

```bash
git clone https://github.com/juanvaz25/GestorTareas.git
cd gestor-tareas
```

### 2. Base de datos

Ejecutar los script SQL en SQL Server Management Studio o en la terminal:

```bash
cd Documentacion BD
sqlcmd -S "(localdb)\MSSQLLocalDB" -i QueryTablas.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -i QueryDatos.sql
```

El primer comando crea la base de datos con las tablas y el segundo agrega datos (opcional)

### 3. Backend

```bash
cd ..
```

Revisar la cadena de conexión en `appsettings.json`.

```json
"DefaultConnection": "Server="tu servidor";Database=GestorTareas;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

Si usás SQL Server Express, reemplazarla por:

```
Server=localhost\SQLEXPRESS;Database=GestorTareas;Trusted_Connection=True
```

Ejecutar la API:

```bash
dotnet run
```

Swagger UI accesible en `https://localhost:7112` o `http://localhost:5135`

### 4. Frontend

```bash
cd frontend
npm install
npm run dev
```

La aplicación queda disponible en `http://localhost:5173`.

> El proxy de Vite redirige `/api/*` automáticamente, por lo que no es necesario configurar variables de entorno en desarrollo.

---

## Endpoints de la API

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/tasks` | Listar todas las tareas |
| GET | `/api/tasks?status={estado}` | Filtrar por estado |
| GET | `/api/tasks/{id}` | Obtener tarea por ID |
| POST | `/api/tasks` | Crear tarea |
| PUT | `/api/tasks/{id}` | Actualizar tarea |
| DELETE | `/api/tasks/{id}` | Eliminar tarea |

**Estados válidos:** `pendiente` · `en progreso` · `completada`

### Ejemplo de payload (POST / PUT)

```json
{
  "titulo": "Revisar documentación",
  "descripcion": "Leer los requisitos del módulo de reportes.",
  "estado": "pendiente",
  "idUsuario": 1
}
```

---

## Decisiones de diseño

### Backend

**Arquitectura en capas (Controller → Service → Repository)**  
Se separaron las responsabilidades para que cada capa tenga una única razón de cambio: el Controller maneja HTTP, el Service contiene la lógica de negocio (validaciones de estado, existencia de usuario) y el Repository centraliza el acceso a datos. Esto facilita el testing unitario ya que cada capa puede mockearse de forma independiente.

**DTOs como `record`**  
Se usaron C# `record` para los DTOs de entrada y salida porque son inmutables por definición, lo que evita mutaciones accidentales y hace explícito que son objetos de transferencia, no entidades de dominio.

**`EstadoTarea` como clase de constantes**  
En lugar de usar un `enum` (que requeriría conversiones al persistir en SQL), se centralizaron los valores válidos en una clase estática. El `CHECK CONSTRAINT` en la base de datos garantiza integridad a nivel de storage; la clase garantiza integridad en la capa de aplicación.

**Middleware de manejo de errores global**  
Evita try/catch redundantes en cada controller y garantiza una respuesta JSON consistente para cualquier excepción no controlada.

**CORS parametrizado**  
Los orígenes permitidos se leen desde `appsettings.json` en lugar de estar hardcodeados, lo que facilita el despliegue en distintos entornos sin recompilar.

### Frontend

**`useTasks` como hook centralizado**  
Encapsula todo el estado relacionado con tareas (lista, loading, error, filtro) y las operaciones que lo modifican. Los componentes solo reciben datos y callbacks, sin lógica de fetch propia.

**Cancelación de efectos**  
El `useEffect` en `useTasks` usa una bandera `cancelled` para evitar actualizar estado en componentes desmontados, lo que previene memory leaks y warnings de React.

**`taskService` como módulo desacoplado**  
La capa de servicio centraliza todas las llamadas HTTP. Si la URL base o los headers cambian (por ejemplo, agregar autenticación), el cambio se hace en un solo lugar.

**Modal con cierre por overlay**  
El formulario de creación/edición se muestra en un modal que se cierra al hacer click fuera de él, patrón UX estándar implementado sin librerías externas.

---

## Herramientas de IA utilizadas

Durante el desarrollo se utilizó **Claude** como asistente de programación.

**De qué manera ayudó:**
- Sugerencias sobre patrones de arquitectura (Repository + Service)
- Revisión de tipos TypeScript y configuración de Vite/Jest

**Lo que se verificó manualmente:**
- Coherencia entre el esquema SQL y el `DbContext` de EF Core
- Que los contratos de DTOs coincidan entre backend y frontend
- Que los códigos HTTP retornados sean los correctos según cada caso
- La lógica de validación de estados y existencia de usuario

Toda decisión de diseño fue revisada y comprendida antes de incorporarla al código.
