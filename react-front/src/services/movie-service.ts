import type { CreateMoviePayload, Movie } from '../types/movie'

const API_BASE = 'https://localhost:7142'

export async function getMovies(tag?: string): Promise<Movie[]> {
  const url = tag
    ? `${API_BASE}/GetMovies?tag=${encodeURIComponent(tag)}`
    : `${API_BASE}/GetMovies`

  const response = await fetch(url)

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

export async function getMovie(id: number): Promise<Movie | null> {
  const response = await fetch(`${API_BASE}/GetMovie?id=${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  })

  if (response.status === 404) {
    return null
  }

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`)
  }

  return response.json() as Promise<Movie>
}

export async function addMovieTag(
  movieId: number,
  tagName: string,
): Promise<Movie> {
  const response = await fetch(`${API_BASE}/AddMovieTag`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ movieId, tagName }),
  })

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`)
  }

  return response.json() as Promise<Movie>
}

export async function removeMovieTag(
  movieId: number,
  tagName: string,
): Promise<Movie> {
  const response = await fetch(`${API_BASE}/RemoveMovieTag`, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ movieId, tagName }),
  })

  if (!response.ok) {
    throw new Error(`Ошибка запроса: ${response.status}`)
  }

  return response.json() as Promise<Movie>
}
