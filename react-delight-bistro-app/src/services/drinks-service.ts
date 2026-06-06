import type { Drink } from '../types/drinks.js';

const API_BASE = 'https://localhost:7090';
const API_GET_TEAS = '/GetTeas';
const API_POST_CREATE_TEA = '/CreateTea';

export async function getDrinks(): Promise<Drink[]> {
  const response = await fetch(`${API_BASE}${API_GET_TEAS}`);

  if (!response.ok) {
    throw new Error(`Ошибка запроса ${response.status}`);
  }

  return response.json();
}
