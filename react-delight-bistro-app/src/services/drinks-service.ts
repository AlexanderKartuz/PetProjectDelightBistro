import type { CreateDrinkPayload, Drink } from '../types/drinks.js';
import { getAccessToken } from './auth-service.js';
import { throwForFailedResponse } from './api-error.js';

const API_BASE = 'https://localhost:7090';
const API_GET_DRINKS = '/GetDrinks';
const API_POST_CREATE_DRINK = '/CreateDrink';
const API_DELETE = '/DeleteDrink';
const API_CHANGE = '/ChangeDrink';
const API_GET_DRINK = '/GetDrink';

function jsonHeaders(includeAuth = false): HeadersInit {
  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
  };

  if (includeAuth) {
    const token = getAccessToken();
    if (token) {
      headers.Authorization = `Bearer ${token}`;
    }
  }

  return headers;
}

export async function getDrinks(): Promise<Drink[]> {
  const response = await fetch(`${API_BASE}${API_GET_DRINKS}`);

  if (!response.ok) {
    await throwForFailedResponse(response, 'Ошибка запроса');
  }

  return (await response.json()) as Drink[];
}

export async function createDrink(payload: CreateDrinkPayload): Promise<Drink> {
  const response = await fetch(`${API_BASE}${API_POST_CREATE_DRINK}`, {
    method: 'POST',
    headers: jsonHeaders(),
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    await throwForFailedResponse(response, 'Ошибка создания');
  }

  return (await response.json()) as Drink;
}

/** DELETE /DeleteDrink/{id} — без body; нужен JWT с ролью Admin. */
export async function deleteDrink(id: number): Promise<void> {
  const response = await fetch(`${API_BASE}${API_DELETE}/${id}`, {
    method: 'DELETE',
    headers: jsonHeaders(true),
  });

  if (response.status === 404) {
    throw new Error(`Напиток с id=${id} не найден`);
  }

  if (!response.ok) {
    await throwForFailedResponse(response, 'Ошибка удаления');
  }
}

export async function changeDrink(
  id: number,
  payload: CreateDrinkPayload,
): Promise<Drink> {
  const response = await fetch(`${API_BASE}${API_CHANGE}/${id}`, {
    method: 'PUT',
    headers: jsonHeaders(),
    body: JSON.stringify(payload),
  });

  if (response.status === 404) {
    throw new Error(`Напиток с id=${id} не найден`);
  }

  if (!response.ok) {
    await throwForFailedResponse(response, 'Ошибка изменения');
  }

  return (await response.json()) as Drink;
}

export async function getDrink(id: number): Promise<Drink> {
  const response = await fetch(`${API_BASE}${API_GET_DRINK}/${id}`, {
    method: 'GET',
  });

  if (response.status === 404) {
    throw new Error(`Напиток с id=${id} не найден`);
  }

  if (!response.ok) {
    await throwForFailedResponse(response, 'Ошибка запроса');
  }

  return (await response.json()) as Drink;
}
