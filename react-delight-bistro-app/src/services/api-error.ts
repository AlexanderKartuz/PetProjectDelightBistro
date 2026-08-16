/** Разбор ошибок Minimal API (ValidationProblem, 401/403/404). */
export async function throwForFailedResponse(
  response: Response,
  fallbackMessage: string,
): Promise<never> {
  if (response.status === 400) {
    throw new Error(await readValidationProblem(response));
  }

  if (response.status === 401) {
    throw new Error('Нужна авторизация (JWT). Войдите как Admin.');
  }

  if (response.status === 403) {
    throw new Error('Недостаточно прав (нужна роль Admin).');
  }

  if (response.status === 404) {
    throw new Error('Ресурс не найден');
  }

  throw new Error(`${fallbackMessage}: ${response.status}`);
}

async function readValidationProblem(response: Response): Promise<string> {
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
