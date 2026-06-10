import { useCallback, useState } from 'react';
import type { CreateDrinkPayload, Drink } from '../types/drinks.js';
import { Button } from './button.js';
import { changeDrink } from '../services/drinks-service.js';
import { NavLink } from 'react-router-dom';

interface DrinkCardProps {
  drink: Drink;
  onDelete?: (id: number) => void;
  showDetailsLink?: boolean;
}

export const DrinkCard = function ({
  drink,
  onDelete,
  showDetailsLink = true,
}: DrinkCardProps) {
  const [isNameEditing, setIsNameEditing] = useState(false);
  const [isPriceEditing, setIsPriceEditing] = useState(false);
  const [newName, setNewName] = useState(drink.name);
  const [newPrice, setNewPrice] = useState(drink.price);

  const [currentDrink, setCurrentDrink] = useState<Drink>(drink);

  const isItemChanged =
    newName !== currentDrink.name || newPrice !== currentDrink.price;

  const isEditing = isNameEditing || isPriceEditing;

  const resetEditing = () => {
    setIsNameEditing(false);
    setIsPriceEditing(false);
    setNewName(currentDrink.name);
    setNewPrice(currentDrink.price);
  };

  const handleSaveChanges = async () => {
    if (!isItemChanged) {
      resetEditing();
      return;
    }

    const changedDrink: CreateDrinkPayload = {
      name: newName.trim(),
      price: newPrice,
    };

    try {
      const updateDrink = await changeDrink(currentDrink.id, changedDrink);
      setCurrentDrink(updateDrink);
      resetEditing();
    } catch (err) {
      console.error('Ошибка сохранения', err);
    }
  };

  return (
    <div className="drink-card">
      {isEditing ? (
        <input
          type="text"
          className="drink-name-input"
          value={newName}
          onChange={(event) => setNewName(event.target.value)}
        />
      ) : (
        <div className="drink-name" onClick={() => setIsNameEditing(true)}>
          {currentDrink.name}
        </div>
      )}

      <div className="drink-price">
        {isEditing ? (
          <input
            type="number"
            className="drink-price-input"
            value={newPrice}
            onChange={(event) => setNewPrice(Number(event.target.value))}
          />
        ) : (
          <span className="price-value" onClick={() => setIsPriceEditing(true)}>
            {currentDrink.price}
          </span>
        )}
        <span className="price-currency">BYN</span>
      </div>
      {isItemChanged && (
        <Button className="drink-card-update-btn" onClick={handleSaveChanges}>
          Обновить напиток
        </Button>
      )}
      {onDelete && !isItemChanged && (
        <Button
          className="drink-card-delete-btn"
          onClick={() => onDelete(currentDrink.id)}
        >
          Удалить напиток
        </Button>
      )}
      {showDetailsLink && (
        <div className="drink-card-link">
          <NavLink to={`/drink/${currentDrink.id}`} className="drink-link">
            Подробнее
          </NavLink>
        </div>
      )}
    </div>
  );
};
