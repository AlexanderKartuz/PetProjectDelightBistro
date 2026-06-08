import type { AnimalFact, CreateFactPayload } from '../types/animal-fact'

const MINIMAL_API_BASE = 'https://localhost:7264'
const COMMON_API_BASE = 'https://localhost:7284'

export async function getAnimalSpeciesNames(): Promise<string[]> {
  const response = await fetch(`${COMMON_API_BASE}/api/AnimalWorld/GetAnimalSpeciesNames`)

  if (!response.ok) {
    throw new Error(`Ошибка запроса видов: ${response.status}`)
  }

  return response.json()
}

export async function getFacts(): Promise<AnimalFact[]> {
  const response = await fetch(`${MINIMAL_API_BASE}/GetFacts`)

  if (!response.ok) {
    throw new Error(`Ошибка запроса фактов: ${response.status}`)
  }

  return response.json()
}

export async function createFact(payload: CreateFactPayload): Promise<AnimalFact> {
  const response = await fetch(`${MINIMAL_API_BASE}/AddFact`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(payload),
  })

  if (!response.ok) {
    throw new Error(`Ошибка добавления факта: ${response.status}`)
  }

  return payload
}