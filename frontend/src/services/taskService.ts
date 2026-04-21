import type { CreateTaskPayload, Task, UpdateTaskPayload } from '../types';

const BASE_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5135/api';

async function handleResponse<T>(res: Response): Promise<T> {
  if (!res.ok) {
    const body = await res.json().catch(() => ({}));
    throw new Error(body?.error ?? `Error ${res.status}: ${res.statusText}`);
  }
  if (res.status === 204) return undefined as T;
  return res.json();
}

export const taskService = {
  getAll: (estado?: string): Promise<Task[]> => {
    const url = estado
      ? `${BASE_URL}/tasks?status=${encodeURIComponent(estado)}`
      : `${BASE_URL}/tasks`;
    return fetch(url).then(handleResponse<Task[]>);
  },

  getById: (id: number): Promise<Task> =>
    fetch(`${BASE_URL}/tasks/${id}`).then(handleResponse<Task>),

  create: (payload: CreateTaskPayload): Promise<Task> =>
    fetch(`${BASE_URL}/tasks`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload),
    }).then(handleResponse<Task>),

  update: (id: number, payload: UpdateTaskPayload): Promise<Task> =>
    fetch(`${BASE_URL}/tasks/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload),
    }).then(handleResponse<Task>),

  delete: (id: number): Promise<void> =>
    fetch(`${BASE_URL}/tasks/${id}`, { method: 'DELETE' }).then(handleResponse<void>),
};