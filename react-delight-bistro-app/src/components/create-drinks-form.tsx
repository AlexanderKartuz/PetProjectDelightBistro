import React, { useState } from 'react';
import type { Drink } from '../types/drinks.js';
import { createDrink } from '../services/drinks-service.js';
import { DrinkCard } from './drink-card.js';

interface CreateDrinkFormProps {
  onCreated: (newDrink: Drink) => void;
}

export const CreteDrinkForm = function ({ onCreated }: CreateDrinkFormProps) {
  const [name, setName] = useState('');
  const [price, setPrice] = useState(0);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setSubmitting(true);
    setError(null);

    try {
      const drink = await createDrink({ name, price });
      onCreated(drink);
      setName('');
      setPrice(0);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось создать');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form className="create-drink-form" onSubmit={handleSubmit}>
      <div className="create-drink-title">Добавить элемент</div>
      <div className="create-drink-form-dield">
        <div>Назавние</div>
        <input
          type="text"
          value={name}
          onChange={(event) => setName(event.target.value)}
          required
        />
      </div>
      <div className="create-drink-form-dield">
        <div>Стоимость</div>
        <input
          type="number"
          value={price}
          onChange={(event) => setPrice(Number(event.target.value))}
          required
        />
      </div>

      <div className="create-drink-form-preview">
        <span>Превью</span>
        <DrinkCard
          drink={{
            id: 0,
            name: name || 'Название напитка',
            price,
          }}
        />
        <button type="submit" disabled={submitting}>
          {submitting ? 'Сохранение...' : 'Создать напиток'}
        </button>
      </div>
    </form>
  );
};
