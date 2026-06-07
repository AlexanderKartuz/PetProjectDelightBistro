import type { Drink } from '../types/drinks.js';

interface DrinkCardProps {
  drink: Drink;
  onDelete?: (id: number) => void;
}

export const DrinkCard = function ({ drink, onDelete }: DrinkCardProps) {
  return (
    <div className="drink-card">
      <div className="drink-name">{drink.name}</div>
      <div className="drink-price">
        <span className="price-value">{drink.price}</span>
        <span className="price-currency">BYN</span>
      </div>
      {onDelete && (
        <button
          type="button"
          className="drink-card-delete-btn button"
          onClick={() => onDelete(drink.id)}
        >
          Удалить напиток
        </button>
      )}
    </div>
  );
};
