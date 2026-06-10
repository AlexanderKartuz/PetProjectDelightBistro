import type { CreateRelicPayload, Relic } from '../types/relic'

const API_BASE = 'https://localhost:7050'

export async function getRelics(): Promise<Relic[]> {
  const response = await fetch(`${API_BASE}/GetRelics`)

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`)
  }

  return response.json()
}

export async function createRelic(payload: CreateRelicPayload): Promise<Relic> {
  const response = await fetch(`${API_BASE}/CreatRelic`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      id: 0,
      ...payload,
    }),
  })

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`)
  }

  return response.json() as Promise<Relic>
}

export async function deleteRelic(id: number): Promise<void> {
  const response = await fetch(`${API_BASE}/DeleteRelic`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(id),
  })

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`)
  }
}
