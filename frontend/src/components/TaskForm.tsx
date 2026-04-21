import { useEffect, useState } from 'react';
import type { CreateTaskPayload, EstadoTarea, Task } from '../types';
import { ESTADOS } from '../types';

interface Props {
  task?: Task | null;
  onSubmit: (payload: CreateTaskPayload) => Promise<void>;
  onCancel: () => void;
}

const EMPTY: CreateTaskPayload = {
  titulo: '',
  descripcion: '',
  estado: 'pendiente',
  idUsuario: 1,
};

export function TaskForm({ task, onSubmit, onCancel }: Props) {
  const [form, setForm] = useState<CreateTaskPayload>(EMPTY);
  const [submitting, setSubmitting] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  useEffect(() => {
    if (task) {
      setForm({
        titulo: task.titulo,
        descripcion: task.descripcion ?? '',
        estado: task.estado,
        idUsuario: task.idUsuario,
      });
    } else {
      setForm(EMPTY);
    }
    setFormError(null);
  }, [task]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: name === 'idUsuario' ? Number(value) : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!form.titulo.trim()) {
      setFormError('El título es obligatorio.');
      return;
    }
    setSubmitting(true);
    setFormError(null);
    try {
      await onSubmit(form);
    } catch (err: unknown) {
      setFormError(err instanceof Error ? err.message : 'Error al guardar.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form className="task-form" onSubmit={handleSubmit} noValidate>
      <h2>{task ? 'Editar tarea' : 'Nueva tarea'}</h2>

      {formError && <p className="form-error">{formError}</p>}

      <label>
        Título *
        <input
          name="titulo"
          value={form.titulo}
          onChange={handleChange}
          maxLength={200}
          required
          autoFocus
        />
      </label>

      <label>
        Descripción
        <textarea
          name="descripcion"
          value={form.descripcion ?? ''}
          onChange={handleChange}
          rows={3}
          maxLength={1000}
        />
      </label>

      <label>
        Estado *
        <select name="estado" value={form.estado} onChange={handleChange}>
          {ESTADOS.map((e) => (
            <option key={e} value={e}>{e}</option>
          ))}
        </select>
      </label>

      <label>
        ID Usuario *
        <input
          name="idUsuario"
          type="number"
          min={1}
          value={form.idUsuario}
          onChange={handleChange}
          required
        />
      </label>

      <div className="form-actions">
        <button type="submit" className="btn-primary" disabled={submitting}>
          {submitting ? 'Guardando…' : task ? 'Actualizar' : 'Crear'}
        </button>
        <button type="button" className="btn-secondary" onClick={onCancel}>
          Cancelar
        </button>
      </div>
    </form>
  );
}