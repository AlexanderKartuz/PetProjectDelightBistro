export interface Movie {
  id: number
  name: string
  url: string
  rating: number
  tags: string[]
}

export interface CreateMoviePayload {
  name: string
  url: string
  rating: number
  tags?: string[]
}

export function parseTagsInput(input: string): string[] {
  return input
    .split(',')
    .map((tag) => tag.trim())
    .filter(Boolean)
}
