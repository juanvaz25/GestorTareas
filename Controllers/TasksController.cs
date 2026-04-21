using GestorTareas.DTOs;
using GestorTareas.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestorTareas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        /// <summary>Lista todas las tareas. Opcionalmente filtra por estado.</summary>
        /// <param name="status">Estado: pendiente | en progreso | completada</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string? status)
        {
            try
            {
                var tasks = await _service.GetAllAsync(status);
                return Ok(tasks);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>Obtiene una tarea por su ID.</summary>
        /// <param name="id">ID de la tarea</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _service.GetByIdAsync(id);
            return task is null
                ? NotFound(new { error = $"Tarea con Id {id} no encontrada." })
                : Ok(task);
        }

        /// <summary>Crea una nueva tarea.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        /// <summary>Actualiza una tarea existente.</summary>
        /// <param name="id">ID de la tarea a actualizar</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return updated is null
                    ? NotFound(new { error = $"Tarea con Id {id} no encontrada." })
                    : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        /// <summary>Elimina una tarea.</summary>
        /// <param name="id">ID de la tarea a eliminar</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted
                ? NoContent()
                : NotFound(new { error = $"Tarea con Id {id} no encontrada." });
        }
    }
}
