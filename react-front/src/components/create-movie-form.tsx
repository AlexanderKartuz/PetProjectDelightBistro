import { useState, type FormEvent } from 'react'
import { createMovie } from '../services/movie-service'
import { parseTagsInput } from '../types/movie'
import { MovieCard } from './movie-card'
import type { Movie } from '../types/movie'

interface CreateMovieFormProps {
  onCreated: (movie: Movie) => void
}

export const CreateMovieForm = function ({ onCreated }: CreateMovieFormProps) {
  const [name, setName] = useState('')
  const [url, setUrl] = useState('')
  const [rating, setRating] = useState(0)
  const [tagsInput, setTagsInput] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const previewTags = parseTagsInput(tagsInput)

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setSubmitting(true)
    setError(null)

    try {
      const tags = parseTagsInput(tagsInput)
      const movie = await createMovie({
        name,
        url,
        rating,
        tags: tags.length > 0 ? tags : undefined,
      })
      onCreated(movie)

      setName('')
      setUrl('')
      setRating(0)
      setTagsInput('')
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось создать фильм')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <form className="create-movie-form" onSubmit={handleSubmit}>
      <h3 className="create-movie-form__heading">Добавить фильм</h3>

      <label className="create-movie-form__field">
        <span>Название</span>
        <input
          type="text"
          value={name}
          onChange={(event) => setName(event.target.value)}
          required
        />
      </label>

      <label className="create-movie-form__field">
        <span>URL постера</span>
        <input
          type="url"
          value={url}
          onChange={(event) => setUrl(event.target.value)}
          required
        />
      </label>

      <label className="create-movie-form__field">
        <span>Рейтинг</span>
        <input
          type="number"
          min={0}
          step={0.1}
          value={rating}
          onChange={(event) => setRating(Number(event.target.value))}
          required
        />
      </label>

      <label className="create-movie-form__field">
        <span>Теги</span>
        <input
          type="text"
          value={tagsInput}
          onChange={(event) => setTagsInput(event.target.value)}
          placeholder="Боевик, Кристофер Нолан"
        />
        <span className="create-movie-form__hint">Через запятую</span>
      </label>

      <div className="create-movie-form__preview">
        <span className="create-movie-form__preview-label">Превью</span>
        <MovieCard
          movie={{
            id: 0,
            name: name || 'Название фильма',
            url,
            rating,
            tags: previewTags,
          }}
        />
      </div>

      {error && <p className="create-movie-form__error">{error}</p>}

      <button type="submit" disabled={submitting}>
        {submitting ? 'Сохранение...' : 'Создать фильм'}
      </button>
    </form>
  )
}
