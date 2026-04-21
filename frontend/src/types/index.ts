export type EstadoTarea = 'pendiente' | 'en progreso' | 'completada';

export const ESTADOS: EstadoTarea[] = ['pendiente', 'en progreso', 'completada'];

export interface Task {
  id: number;
  titulo: string;
  descripcion: string | null;
  estado: EstadoTarea;
  idUsuario: number;
  nombreUsuario: string;
  fechaCreacion: string;
}

export interface CreateTaskPayload {
  titulo: string;
  descripcion?: string;
  estado: EstadoTarea;
  idUsuario: number;
}

export interface UpdateTaskPayload extends CreateTaskPayload {}