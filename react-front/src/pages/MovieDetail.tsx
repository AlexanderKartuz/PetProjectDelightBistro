import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { MovieTags } from "../components/movie-tags";
import {
  addMovieTag,
  getMovie,
  removeMovieTag,
} from "../services/movie-service";
import type { Movie } from "../types/movie";

export const MovieDetail = () => {
  const { id } = useParams();
  const [movie, setMovie] = useState<Movie | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [addingTag, setAddingTag] = useState(false);
  const [removingTag, setRemovingTag] = useState<string | null>(null);
  const [tagError, setTagError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    const fetchMovie = async () => {
      setLoading(true);
      setError(null);

      try {
        const data = await getMovie(Number(id));

        if (cancelled) {
          return;
        }

        if (!data) {
          setError("Фильм не найден");
          setMovie(null);
        } else {
          setMovie(data);
        }
      } catch (err) {
        if (!cancelled) {
          setError(
            err instanceof Error ? err.message : "Не удалось загрузить фильм",
          );
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    };

    fetchMovie();

    return () => {
      cancelled = true;
    };
  }, [id]);

  const handleAddTag = useCallback(
    async (tagName: string) => {
      if (!movie) {
        return;
      }

      setAddingTag(true);
      setTagError(null);

      try {
        const updated = await addMovieTag(movie.id, tagName);
        setMovie(updated);
      } catch (err) {
        setTagError(
          err instanceof Error ? err.message : "Не удалось добавить тег",
        );
      } finally {
        setAddingTag(false);
      }
    },
    [movie],
  );

  const handleRemoveTag = useCallback(
    async (tagName: string) => {
      if (!movie) {
        return;
      }

      setRemovingTag(tagName);
      setTagError(null);

      try {
        const updated = await removeMovieTag(movie.id, tagName);
        setMovie(updated);
      } catch (err) {
        setTagError(
          err instanceof Error ? err.message : "Не удалось удалить тег",
        );
      } finally {
        setRemovingTag(null);
      }
    },
    [movie],
  );

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div
      style={{
        minHeight: "100vh",
        position: "relative",
        background: movie?.url
          ? `linear-gradient(to bottom, rgba(10, 10, 10, 0.85) 0%, rgba(20,20,20,0.6) 70%, rgba(30,30,30,0.95) 100%), url(${movie.url}) center/cover no-repeat`
          : "#222",
        color: "#fafafa",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        flexDirection: "column",
        padding: "2rem 1rem",
      }}
    >
      <div
        style={{
          backgroundColor: "rgba(15, 15, 15, 0.88)",
          borderRadius: "1.6rem",
          boxShadow: "0 4px 32px rgba(0,0,0,.26)",
          padding: "2.5rem 3rem",
          maxWidth: 420,
          width: "95%",
          textAlign: "center",
          backdropFilter: "blur(2px)",
          marginBottom: "2rem",
        }}
      >
        <div style={{ marginBottom: "1.5rem" }}>
          {movie?.url ? (
            <img
              src={movie.url}
              alt={movie.name}
              style={{
                width: "100%",
                borderRadius: "1.2rem",
                boxShadow: "0 2px 16px rgba(0,0,0,0.27)",
                objectFit: "cover",
              }}
            />
          ) : (
            <div
              style={{
                width: "100%",
                height: 320,
                background: "#484848",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                color: "#ccc",
                borderRadius: "1.2rem",
                fontSize: "1.14rem",
              }}
            >
              Нет постера
            </div>
          )}
        </div>
        <h1 style={{ margin: "0 0 0.5em", fontSize: "2rem", fontWeight: 700 }}>
          {movie?.name}
        </h1>
        <div
          style={{
            fontSize: "1.1rem",
            marginBottom: "0.5em",
            color: "#ffd700",
            fontWeight: 500,
          }}
        >
          ⭐ Рейтинг: {movie?.rating}
        </div>
        <ul
          style={{
            listStyle: "none",
            padding: 0,
            margin: "0.6em 0 0",
            fontSize: "1.08rem",
            color: "#efefef",
          }}
        >
          <li>
            <span style={{ color: "#8cf", fontWeight: 500 }}>ID:</span>{" "}
            {movie?.id}
          </li>
        </ul>

        <div className="movie-detail__tags" style={{ marginTop: "1.5rem" }}>
          <h2
            style={{
              margin: "0 0 0.75rem",
              fontSize: "1.1rem",
              fontWeight: 600,
            }}
          >
            Теги
          </h2>
          <MovieTags
            tags={movie?.tags ?? []}
            clickable
            editable
            onAdd={handleAddTag}
            onRemove={handleRemoveTag}
            adding={addingTag}
            removingTag={removingTag}
          />
          {tagError && (
            <p className="movie-detail__tag-error">{tagError}</p>
          )}
        </div>
      </div>
    </div>
  );
};
