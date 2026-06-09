import { useCallback, useEffect, useState } from 'react';
import { DrinkCard } from '../components/drink-card.js';
import { useParams } from 'react-router';
import { getDrink } from '../services/drinks-service.js';
import type { Drink } from '../types/drinks.js';

export const DrinkPage = function () {
  const { id } = useParams();
  const drinkId = Number(id);
  const [drink, setDrink] = useState<Drink | null>(null);
  const [error, setError] = useState<string | null>(null);

  const loadDrink = useCallback(async () => {
    try {
      const data = await getDrink(drinkId);
      setDrink(data);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : `Элемент с id= ${drinkId} не найден`,
      );
    }
  }, [drinkId]);

  useEffect(() => {
    loadDrink();
  }, [loadDrink]);

  if (!drink) {
    return <div>Элемент не найден</div>;
  }
  return (
    <>
      <DrinkCard drink={drink} showDetailsLink={false} />
    </>
  );
};
