import type { LoginRequest, LoginResponse } from '../types/auth.js';

const API_BASE = 'https://localhost:7090';
const API_LOGIN = '/login';
const TOKEN_KEY = 'delightBistro.accessToken';

export function getAccessToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

export function setAccessToken(token: string): void {
  localStorage.setItem(TOKEN_KEY, token);
}

export function clearAccessToken(): void {
  localStorage.removeItem(TOKEN_KEY);
}

export function isLoggedIn(): boolean {
  return Boolean(getAccessToken());
}

export async function login(credentials: LoginRequest): Promise<LoginResponse> {
  const response = await fetch(`${API_BASE}${API_LOGIN}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(credentials),
  });

  if (response.status === 400) {
    throw new Error(await readValidationMessage(response));
  }

  if (response.status === 401) {
    throw new Error('Неверный логин или пароль');
  }

  if (!response.ok) {
    throw new Error(`Ошибка входа: ${response.status}`);
  }

  const data: LoginResponse = await response.json();
  setAccessToken(data.accessToken);
  return data;
}

export function logout(): void {
  clearAccessToken();
}

async function readValidationMessage(response: Response): Promise<string> {
  try {
    const problem = (await response.json()) as {
      errors?: Record<string, string[]>;
      title?: string;
    };
    if (problem.errors) {
      return Object.entries(problem.errors)
        .flatMap(([field, messages]) =>
          messages.map((m) => (field ? `${field}: ${m}` : m)),
        )
        .join('; ');
    }
    return problem.title ?? 'Ошибка валидации';
  } catch {
    return 'Ошибка валидации';
  }
}
