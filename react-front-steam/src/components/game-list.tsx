import { useEffect, useState } from "react";
import type { SteamGame } from "../types/steam-game";
import { getGames } from "../services/game-service";
import { GameCard } from "./game-card";
import type { PaginatedResponse } from "../types/pagination";

const PAGE_SIZE = 12;

export const GameList = function () {
  const [pagination, setPagination] =
    useState<PaginatedResponse<SteamGame> | null>(null);
  const [games, setGames] = useState<SteamGame[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1);

  useEffect(() => {
    let cancelled = false;

    const load = async (page: number) => {
      setLoading(true);

      try {
        const data = await getGames({ page, pageSize: PAGE_SIZE });

        if (!cancelled) {
          setGames(data.items);
          setPagination(data);
          setError(null);
        }
      } catch (err) {
        if (err instanceof Error && err.message === "Failed to fetch") {
          if (!cancelled) {
            setError("Start backend server");
          }
        } else if (!cancelled) {
          setError(err instanceof Error ? err.message : "Can't load games");
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    };

    load(currentPage);

    return () => {
      cancelled = true;
    };
  }, [currentPage]);

  const goToPage = (page: number) => {
    if (!pagination) {
      return;
    }

    if (page < 1 || page > pagination.totalPages) {
      return;
    }

    setCurrentPage(page);
    window.scrollTo({ top: 0, behavior: "smooth" });
  };

  if (loading) {
    return <p className="game-list__status">Games loading...</p>;
  }

  if (error) {
    return (
      <p className="game-list__status game-list__status--error">{error}</p>
    );
  }

  if (games.length === 0) {
    return <p className="game-list__status">Games not found</p>;
  }

  return (
    <section className="game-list">
      <h2 className="game-list__heading">Steam catalog</h2>
      <p className="game-list__count">
        {pagination?.totalCount ?? games.length} games total
        {pagination && pagination.totalPages > 1 && (
          <> · page {pagination.currentPage} of {pagination.totalPages}</>
        )}
      </p>

      <div className="game-list__grid">
        {games.map((game) => (
          <GameCard key={game.id} game={game} />
        ))}
      </div>

      {pagination && pagination.totalPages > 1 && (
        <nav className="pagination" aria-label="Game pages">
          <button
            type="button"
            className="pagination__btn"
            disabled={!pagination.hasPrevious}
            onClick={() => goToPage(currentPage - 1)}
          >
            ← Back
          </button>

          <span className="pagination__info">
            {pagination.currentPage} / {pagination.totalPages}
          </span>

          <button
            type="button"
            className="pagination__btn"
            disabled={!pagination.hasNext}
            onClick={() => goToPage(currentPage + 1)}
          >
            Next →
          </button>
        </nav>
      )}
    </section>
  );
};
