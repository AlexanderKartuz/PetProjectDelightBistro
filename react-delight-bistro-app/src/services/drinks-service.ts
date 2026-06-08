import type { CreateDrinkPayload, Drink } from '../types/drinks.js';

const API_BASE = 'https://localhost:7090';
const API_GET_TEAS = '/GetTeas';
const API_POST_CREATE_TEA = '/CreateTea';
const API_DELETE = '/DeleteDrink';
const API_CHANGE = '/ChangeDrink';

export async function getDrinks(): Promise<Drink[]> {
  const response = await fetch(`${API_BASE}${API_GET_TEAS}`);

  if (!response.ok) {
    throw new Error(`Ошибка запроса ${response.status}`);
  }

  const data: Drink[] = await response.json();
  return data;
}

export async function createDrink(payload: CreateDrinkPayload): Promise<Drink> {
  const response = await fetch(`${API_BASE}${API_POST_CREATE_TEA}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`);
  }
  const data: Drink = await response.json();
  return data;
}

export async function deleteDrink(id: number): Promise<void> {
  const response = await fetch(`${API_BASE}${API_DELETE}`, {
    method: 'DELETE',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(id),
  });

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`);
  }
}

export async function changeDrink(
  id: number,
  payload: CreateDrinkPayload,
): Promise<Drink> {
  const response = await fetch(`${API_BASE}${API_CHANGE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });

  if (response.status === 404) {
    throw new Error(`Чай с id=${id} не найден`);
  }

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`);
  }

  const data: Drink = await response.json();
  return data;
}
