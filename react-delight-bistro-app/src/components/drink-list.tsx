import { useCallback, useEffect, useState } from 'react';
import type { Drink } from '../types/drinks.js';
import { getDrinks } from '../services/drinks-service.js';
import { DrinkCard } from './drink-card.js';

const API_BASE = 'https://localhost:7090';
const API_GET_TEAS = '/GetTeas';

export const DrinksList = function () {
  const [drinks, setDrinks] = useState<Drink[]>([]);

  //   const [loading, setLoading] =useState(true)
  //   const [error, setError]= useState(null)

  const loadDrinks = useCallback(async () => {
    const data = await getDrinks();
    setDrinks(data);
  }, []);

  useEffect(() => {
    loadDrinks();
  }, [loadDrinks]);

  return (
    <section className="drink-list">
      <div className="drink-list-title">List of Drinks</div>
      <div className="drink-list-grid">
        {drinks.map((drink) => (
          <DrinkCard key={drink.id} drink={drink} />
        ))}
      </div>
    </section>
  );
};
