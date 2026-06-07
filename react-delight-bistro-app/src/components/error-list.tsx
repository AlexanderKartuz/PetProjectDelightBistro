import type { ApiError } from '../types/errors.js';

interface ErrorListProps {
  errors: ApiError[];
  onRemove: (id: number) => void;
}

export const ErrorList = function ({ errors, onRemove }: ErrorListProps) {
  if (errors.length === 0) {
    return null;
  }
  return (
    <div className="error-list">
      {errors.map((error) => (
        <div key={error.id} className="error">
          <div className="error-message-discription">
            <div className="error-message">{error.message}</div>
            {error.description && (
              <div className="error-discription">{error.description}</div>
            )}
          </div>

          <button
            type="button"
            onClick={() => onRemove(error.id)}
            className="delete-btn button"
          >
            ✕
          </button>
        </div>
      ))}
    </div>
  );
};
