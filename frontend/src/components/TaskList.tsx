import type { Task } from '../types';
import { TaskCard } from './TaskCard';

interface Props {
  tasks: Task[];
  loading: boolean;
  error: string | null;
  onEdit: (task: Task) => void;
  onDelete: (id: number) => void;
}

export function TaskList({ tasks, loading, error, onEdit, onDelete }: Props) {
  if (loading) return <p className="state-msg">Cargando tareas…</p>;
  if (error)   return <p className="state-msg state-error">⚠️ {error}</p>;
  if (tasks.length === 0) return <p className="state-msg">No hay tareas para mostrar.</p>;

  return (
    <ul className="task-list">
      {tasks.map((task) => (
        <li key={task.id}>
          <TaskCard task={task} onEdit={onEdit} onDelete={onDelete} />
        </li>
      ))}
    </ul>
  );
}