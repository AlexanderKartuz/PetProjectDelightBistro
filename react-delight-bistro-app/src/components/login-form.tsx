import { useState, type FormEvent } from 'react';
import { isLoggedIn, login, logout } from '../services/auth-service.js';

interface LoginFormProps {
  onAuthChange?: () => void;
}

export const LoginForm = function ({ onAuthChange }: LoginFormProps) {
  const [loginName, setLoginName] = useState('');
  const [password, setPassword] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [loggedIn, setLoggedIn] = useState(isLoggedIn);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setSubmitting(true);
    setError(null);

    try {
      await login({ login: loginName, password });
      setLoggedIn(true);
      setPassword('');
      onAuthChange?.();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось войти');
    } finally {
      setSubmitting(false);
    }
  };

  const handleLogout = () => {
    logout();
    setLoggedIn(false);
    onAuthChange?.();
  };

  if (loggedIn) {
    return (
      <div className="api-login">
        <span className="api-login-status">JWT сохранён (нужен Admin для Delete)</span>
        <button type="button" className="button" onClick={handleLogout}>
          Выйти
        </button>
      </div>
    );
  }

  return (
    <form className="api-login" onSubmit={handleSubmit}>
      <span className="api-login-title">Вход в API (для удаления)</span>
      <input
        type="text"
        placeholder="Логин"
        value={loginName}
        onChange={(e) => setLoginName(e.target.value)}
        required
        autoComplete="username"
      />
      <input
        type="password"
        placeholder="Пароль"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        required
        autoComplete="current-password"
      />
      <button type="submit" className="button" disabled={submitting}>
        {submitting ? 'Вход...' : 'Войти'}
      </button>
      {error ? <div className="api-login-error">{error}</div> : null}
    </form>
  );
};
