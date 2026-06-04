import type { Movie } from '../types/movie'

interface MovieCardProps {
  movie: Movie
  onDelete?: (id: number) => void
  deleting?: boolean
}

export const MovieCard = function ({ movie, onDelete, deleting }: MovieCardProps) {
  return (
    <article className="movie-card">
      <div className="movie-card__poster">
        {movie.url ? (
          <img src={movie.url} alt={movie.name} />
        ) : (
          <div className="movie-card__no-poster">Нет постера</div>
        )}
      </div>
      <div className="movie-card__info">
        <h3 className="movie-card__title">{movie.name}</h3>
        <p className="movie-card__rating">Рейтинг: {movie.rating}</p>
        {onDelete && (
          <button
            type="button"
            className="movie-card__delete"
            onClick={() => onDelete(movie.id)}
            disabled={deleting}
          >
            {deleting ? 'Удаление...' : 'Удалить'}
          </button>
        )}
      </div>
    </article>
  )
}
