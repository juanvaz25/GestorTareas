import { ESTADOS } from '../types';

interface Props {
  value: string;
  onChange: (estado: string) => void;
}

const ETIQUETAS: Record<string, string> = {
  '': 'Todas',
  'pendiente': 'Pendiente',
  'en progreso': 'En progreso',
  'completada': 'Completada',
};

export function TaskFilter({ value, onChange }: Props) {
  return (
    <div className="task-filter">
      {['', ...ESTADOS].map((estado) => (
        <button
          key={estado}
          className={`filter-btn ${value === estado ? 'active' : ''}`}
          onClick={() => onChange(estado)}
        >
          {ETIQUETAS[estado]}
        </button>
      ))}
    </div>
  );
}