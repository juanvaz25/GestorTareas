import { useState } from 'react';
import { TaskFilter } from './components/TaskFilter';
import { TaskForm } from './components/TaskForm';
import { TaskList } from './components/TaskList';
import { useTasks } from './hooks/useTasks';
import type { CreateTaskPayload, Task } from './types';

export default function App() {
  const {
    tasks, loading, error,
    filtroEstado, setFiltroEstado,
    createTask, updateTask, deleteTask,
  } = useTasks();

  const [editingTask, setEditingTask] = useState<Task | null>(null);
  const [showForm, setShowForm] = useState(false);

  const handleEdit = (task: Task) => {
    setEditingTask(task);
    setShowForm(true);
  };

  const handleNew = () => {
    setEditingTask(null);
    setShowForm(true);
  };

  const handleCancel = () => {
    setShowForm(false);
    setEditingTask(null);
  };

  const handleSubmit = async (payload: CreateTaskPayload) => {
    if (editingTask) {
      await updateTask(editingTask.id, payload);
    } else {
      await createTask(payload);
    }
    setShowForm(false);
    setEditingTask(null);
  };

  const handleDelete = async (id: number) => {
    await deleteTask(id);
  };

  return (
    <div className="app">
      <header className="app-header">
        <div className="app-header__inner">
          <h1>Gestor de Tareas</h1>
          <button className="btn-primary" onClick={handleNew}>
            + Nueva tarea
          </button>
        </div>
      </header>

      <main className="app-main">
        {showForm && (
          <div className="modal-overlay" onClick={handleCancel}>
            <div className="modal" onClick={(e) => e.stopPropagation()}>
              <TaskForm
                task={editingTask}
                onSubmit={handleSubmit}
                onCancel={handleCancel}
              />
            </div>
          </div>
        )}

        <TaskFilter value={filtroEstado} onChange={setFiltroEstado} />

        <section className="tasks-section">
          <p className="tasks-count">
            {loading ? '…' : `${tasks.length} tarea${tasks.length !== 1 ? 's' : ''}`}
          </p>
          <TaskList
            tasks={tasks}
            loading={loading}
            error={error}
            onEdit={handleEdit}
            onDelete={handleDelete}
          />
        </section>
      </main>
    </div>
  );
}