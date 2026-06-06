import type { Drink } from '../types/drinks.js';

interface DrinkCardProps {
  drink: Drink;
}

export const DrinkCard = function ({ drink }: DrinkCardProps) {
  return (
    <div className="drink-card">
      <div className="drink-name">{drink.name}</div>
      <div className="drink-price">
        <span className="price-value">{drink.price}</span>
        <span className="price-currency">BYN</span>
      </div>
    </div>
  );
};
