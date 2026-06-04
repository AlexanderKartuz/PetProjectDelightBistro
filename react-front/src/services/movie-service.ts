import type { CreateMoviePayload, Movie } from '../types/movie'

const API_BASE = 'https://localhost:7142'

export async function getMovies(): Promise<Movie[]> {
  const response = await fetch(`${API_BASE}/GetMovies`)

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`)
  }

  return response.json()
}

export async function createMovie(payload: CreateMoviePayload): Promise<Movie> {
  const response = await fetch(`${API_BASE}/CreateMovie`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(payload),
  })

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`)
  }

  return response.json() as Promise<Movie>
}

export async function deleteMovie(id: number): Promise<void> {
  const response = await fetch(`${API_BASE}/DeleteMovie`, {
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
