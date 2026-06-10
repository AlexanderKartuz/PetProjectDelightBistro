import { useEffect, useState } from "react";
import type { SteamGame } from "../types/steam-game";
import { getGames } from "../services/game-service";
import { GameCard } from "./game-card";

export const GameList = function () {
  const [games, setGames] = useState<SteamGame[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    const load = async () => {
      setLoading(true);

      try {
        const data = await getGames();

        if (!cancelled) {
          setGames(data);
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

    load();

    return () => {
      cancelled = true;
    };
  }, []);

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
      <p className="game-list__count">{games.length} games</p>
      <div className="game-list__grid">
        {games.map((game) => (
          <GameCard key={game.id} game={game} />
        ))}
      </div>
    </section>
  );
};
