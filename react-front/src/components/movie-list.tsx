import { useCallback, useEffect, useState, type FormEvent } from "react";
import { useSearchParams } from "react-router-dom";
import type { Movie } from "../types/movie";
import { deleteMovie, getMovies } from "../services/movie-service";
import { CreateMovieForm } from "./create-movie-form";
import { MovieCard } from "./movie-card";

export const MovieList = function () {
  const [searchParams, setSearchParams] = useSearchParams();
  const activeTag = searchParams.get("tag") ?? "";

  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [deletingId, setDeletingId] = useState<number | null>(null);

  useEffect(() => {
    let cancelled = false;

    const fetchMovies = async () => {
      setLoading(true);

      try {
        const data = await getMovies(activeTag || undefined);

        if (!cancelled) {
          setMovies(data);
          setError(null);
        }
      } catch (err) {
        if (err instanceof Error && err.message === "Failed to fetch") {
          if (!cancelled) {
            setError("Ты забыл включить MinimalApi c фильмами");
          }
        } else if (!cancelled) {
          setError(
            err instanceof Error ? err.message : "Не удалось загрузить фильмы",
          );
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    };

    fetchMovies();

    return () => {
      cancelled = true;
    };
  }, [activeTag]);

  const handleMovieCreated = useCallback(
    (movie: Movie) => {
      if (
        activeTag &&
        !movie.tags?.some((t) => t.toLowerCase() === activeTag.toLowerCase())
      ) {
        return;
      }

      setMovies((old) => [...old, movie]);
    },
    [activeTag],
  );

  const handleDelete = useCallback(async (id: number) => {
    setDeletingId(id);
    setError(null);

    try {
      await deleteMovie(id);
      setMovies((movies) => movies.filter((movie) => movie.id !== id));
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Не удалось удалить фильм",
      );
    } finally {
      setDeletingId(null);
    }
  }, []);

  const handleFilterSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const trimmed = String(formData.get("tag") ?? "").trim();

    if (trimmed) {
      setSearchParams({ tag: trimmed });
    } else {
      setSearchParams({});
    }
  };

  const handleClearFilter = () => {
    setSearchParams({});
  };

  if (loading) {
    return <p className="movie-list__status">Загрузка фильмов...</p>;
  }

  if (error) {
    return (
      <p className="movie-list__status movie-list__status--error">{error}</p>
    );
  }

  return (
    <section className="movie-list">
      <h2 className="movie-list__heading">Фильмы</h2>

      <form className="movie-list__filter" onSubmit={handleFilterSubmit}>
        <label className="movie-list__filter-field">
          <span>Поиск по тегу</span>
          <input
            key={activeTag}
            type="text"
            name="tag"
            defaultValue={activeTag}
            placeholder="Боевик, драма..."
          />
        </label>
        <button type="submit" className="movie-list__filter-submit">
          Найти
        </button>
        {activeTag && (
          <button
            type="button"
            className="movie-list__filter-clear"
            onClick={handleClearFilter}
          >
            Сбросить фильтр
          </button>
        )}
      </form>

      {activeTag && (
        <p className="movie-list__filter-active">
          Фильмы с тегом: <strong>{activeTag}</strong>
        </p>
      )}

      <div className="movie-list__grid">
        {movies.map((movie) => (
          <div key={movie.id}>
            <MovieCard
              movie={movie}
              onDelete={handleDelete}
              deleting={deletingId === movie.id}
            />
          </div>
        ))}
      </div>
      <CreateMovieForm onCreated={handleMovieCreated} />
    </section>
  );
};
