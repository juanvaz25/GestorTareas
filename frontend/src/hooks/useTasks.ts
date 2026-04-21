import { useCallback, useEffect, useState } from 'react';
import { taskService } from '../services/taskService';
import type { CreateTaskPayload, Task, UpdateTaskPayload } from '../types';

interface UseTasksReturn {
  tasks: Task[];
  loading: boolean;
  error: string | null;
  filtroEstado: string;
  setFiltroEstado: (estado: string) => void;
  createTask: (payload: CreateTaskPayload) => Promise<void>;
  updateTask: (id: number, payload: UpdateTaskPayload) => Promise<void>;
  deleteTask: (id: number) => Promise<void>;
  refresh: () => void;
}

export function useTasks(): UseTasksReturn {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [filtroEstado, setFiltroEstado] = useState('');
  const [reloadFlag, setReloadFlag] = useState(0);

  const refresh = useCallback(() => setReloadFlag((n) => n + 1), []);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    setError(null);

    taskService
      .getAll(filtroEstado || undefined)
      .then((data) => { if (!cancelled) setTasks(data); })
      .catch((err: Error) => { if (!cancelled) setError(err.message); })
      .finally(() => { if (!cancelled) setLoading(false); });

    return () => { cancelled = true; };
  }, [filtroEstado, reloadFlag]);

  const createTask = useCallback(async (payload: CreateTaskPayload) => {
    await taskService.create(payload);
    refresh();
  }, [refresh]);

  const updateTask = useCallback(async (id: number, payload: UpdateTaskPayload) => {
    await taskService.update(id, payload);
    refresh();
  }, [refresh]);

  const deleteTask = useCallback(async (id: number) => {
    await taskService.delete(id);
    refresh();
  }, [refresh]);

  return { tasks, loading, error, filtroEstado, setFiltroEstado, createTask, updateTask, deleteTask, refresh };
}