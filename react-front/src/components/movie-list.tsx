import { useCallback, useEffect, useState } from 'react'
import type { Movie } from '../types/movie'
import { deleteMovie, getMovies } from '../services/movie-service'
import { CreateMovieForm } from './create-movie-form'
import { MovieCard } from './movie-card'

export const MovieList = function () {
  const [movies, setMovies] = useState<Movie[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [deletingId, setDeletingId] = useState<number | null>(null)

  const loadMovies = useCallback(async () => {
    const data = await getMovies()
    setMovies(data)
    setError(null)
  }, [])

  useEffect(() => {
    let cancelled = false

    const fetchMovies = async () => {
      try {
        const data = await getMovies()

        if (!cancelled) {
          setMovies(data)
          setError(null)
        }
      } catch (err) {
        if (!cancelled) {
          setError(err instanceof Error ? err.message : 'Не удалось загрузить фильмы')
        }
      } finally {
        if (!cancelled) {
          setLoading(false)
        }
      }
    }

    fetchMovies()

    return () => {
      cancelled = true
    }
  }, [])

  const handleMovieCreated = useCallback(async (movie: Movie) => {
    try {
      // await loadMovies()
      // setMovies([...movies, movie])
      setMovies(old => [...old, movie])
    } catch (err) {
        setError(err instanceof Error ? err.message : 'Не удалось обновить список фильмов')
      }
    },
    [movies],
  )

  const handleDelete = useCallback(
    async (id: number) => {
      setDeletingId(id)
      setError(null)

      try {
        await deleteMovie(id)
        // await loadMovies()
        setMovies(movies => movies.filter((movie) => movie.id !== id))
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Не удалось удалить фильм')
      } finally {
        setDeletingId(null)
      }
    },
    [loadMovies],
  )

  if (loading) {
    return <p className="movie-list__status">Загрузка фильмов...</p>
  }

  if (error) {
    return <p className="movie-list__status movie-list__status--error">{error}</p>
  }

  return (
    <section className="movie-list">
      <h2 className="movie-list__heading">Фильмы</h2>
      <CreateMovieForm onCreated={handleMovieCreated} />
      <div className="movie-list__grid">
        {movies.map((movie) => (
          <MovieCard
            key={movie.id}
            movie={movie}
            onDelete={handleDelete}
            deleting={deletingId === movie.id}
          />
        ))}
      </div>
    </section>
  )
}
