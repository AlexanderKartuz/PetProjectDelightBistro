import type { SteamGame } from "../types/steam-game";

interface GameCardProps {
  game: SteamGame;
}

export const GameCard = function ({ game }: GameCardProps) {
  return (
    <article className="game-card">
      <div className="game-card__media">
        {game.imageUrl ? (
          <img src={game.imageUrl} alt={game.title} />
        ) : (
          <div className="game-card__no-image">No image</div>
        )}
      </div>
      <div className="game-card__body">
        <h3 className="game-card__title">{game.title}</h3>
        {game.genres.length > 0 && (
          <div className="game-card__genres">
            {game.genres.map((genre) => (
              <span key={genre} className="game-card__genre">
                {genre}
              </span>
            ))}
          </div>
        )}
        <p className="game-card__price">${game.price.toFixed(2)}</p>
      </div>
    </article>
  );
};
