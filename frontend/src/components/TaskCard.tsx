import type { Task } from '../types';

interface Props {
  task: Task;
  onEdit: (task: Task) => void;
  onDelete: (id: number) => void;
}

const ESTADO_CLASS: Record<string, string> = {
  'pendiente':   'badge-pendiente',
  'en progreso': 'badge-progreso',
  'completada':  'badge-completada',
};

export function TaskCard({ task, onEdit, onDelete }: Props) {
  const fecha = new Date(task.fechaCreacion).toLocaleDateString('es-AR', {
    day: '2-digit', month: '2-digit', year: 'numeric',
  });

  return (
    <article className="task-card">
      <div className="task-card__header">
        <h3 className="task-card__titulo">{task.titulo}</h3>
        <span className={`badge ${ESTADO_CLASS[task.estado] ?? ''}`}>
          {task.estado}
        </span>
      </div>

      {task.descripcion && (
        <p className="task-card__descripcion">{task.descripcion}</p>
      )}

      <div className="task-card__footer">
        <span className="task-card__meta">
          👤 {task.nombreUsuario} · 📅 {fecha}
        </span>
        <div className="task-card__actions">
          <button className="btn-icon" onClick={() => onEdit(task)} title="Editar">
            ✏️
          </button>
          <button
            className="btn-icon btn-danger"
            onClick={() => {
              if (confirm(`¿Eliminar "${task.titulo}"?`)) onDelete(task.id);
            }}
            title="Eliminar"
          >
            🗑️
          </button>
        </div>
      </div>
    </article>
  );
}