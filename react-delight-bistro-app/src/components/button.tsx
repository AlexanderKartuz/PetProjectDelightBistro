import type { ReactNode } from 'react';

interface ButtonProps {
  onClick: () => void;
  className?: string;
  children?: ReactNode;
}
export const Button = function ({ onClick, className, children }: ButtonProps) {
  return (
    <button
      type="button"
      className={`button ${className ?? ''}`.trim()}
      onClick={onClick}
    >
      {children}
    </button>
  );
};
