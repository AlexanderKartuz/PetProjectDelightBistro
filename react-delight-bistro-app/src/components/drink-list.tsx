import { useCallback, useEffect, useRef, useState } from 'react';
import type { Drink } from '../types/drinks.js';
import { deleteDrink, getDrinks } from '../services/drinks-service.js';
import { DrinkCard } from './drink-card.js';
import { CreteDrinkForm } from './create-drinks-form.js';
import { LoginForm } from './login-form.js';
import type { ApiError } from '../types/errors.js';
import { ErrorList } from './error-list.js';

export const DrinksList = function () {
  const [drinks, setDrinks] = useState<Drink[]>([]);
  const [errors, setErrors] = useState<ApiError[]>([]);
  const nextErrorId = useRef(0);

  const removeError = useCallback((id: number) => {
    setErrors((prev) => prev.filter((e) => e.id !== id));
  }, []);

  const addError = useCallback(
    (message: string, discription?: string) => {
      const id = nextErrorId.current++;
      const newError: ApiError = { id, message };

      if (discription !== undefined) {
        newError.description = discription;
      }
      setErrors((old) => [...old, newError]);

      setTimeout(() => {
        removeError(id);
      }, 5000);
    },
    [removeError],
  );

  const loadDrinks = useCallback(async () => {
    try {
      const data = await getDrinks();
      setDrinks(data);
    } catch (err) {
      addError(
        err instanceof Error ? err.message : 'Не удалось загрузить напитки',
        'Ошибка загрузки',
      );
    }
  }, [addError]);

  useEffect(() => {
    loadDrinks();
  }, [loadDrinks]);

  const handleDrinkCreated = useCallback((newDrink: Drink) => {
    setDrinks((old) => [newDrink, ...old]);
  }, []);

  const handleDelete = useCallback(
    async (id: number) => {
      try {
        await deleteDrink(id);
        setDrinks((drinks) => drinks.filter((drinks) => drinks.id != id));
      } catch (err) {
        addError(
          err instanceof Error ? err.message : 'Не удалось удалить напиток',
          'Ошибка удаления',
        );
      }
    },
    [addError],
  );

  return (
    <section className="drink-list">
      <h2 className="drink-list-title">List of Drinks</h2>
      <LoginForm />
      <ErrorList errors={errors} onRemove={removeError} />
      <div className="drink-list-grid">
        {drinks.map((drink) => (
          <DrinkCard key={drink.id} drink={drink} onDelete={handleDelete} />
        ))}
      </div>
      <CreteDrinkForm onCreated={handleDrinkCreated} />
    </section>
  );
};
