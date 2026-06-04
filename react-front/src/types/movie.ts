export interface Movie {
  id: number
  name: string
  url: string
  rating: number
}

export interface CreateMoviePayload {
  name: string
  url: string
  rating: number
}
