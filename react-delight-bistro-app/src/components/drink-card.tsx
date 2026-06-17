import { useCallback, useEffect, useRef, useState } from 'react';
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
  const [newDescription, setNewDescription] = useState(drink.description ?? '');
  const [newImgUrl, setNewImgUrl] = useState(drink.imgUrl ?? '');

  const [currentDrink, setCurrentDrink] = useState<Drink>(drink);
  const cardRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    setCurrentDrink(drink);
    setNewName(drink.name);
    setNewPrice(drink.price);
    setNewDescription(drink.description ?? '');
    setNewImgUrl(drink.imgUrl ?? '');
  }, [drink.id, drink.name, drink.price, drink.description, drink.imgUrl]);

  const isItemChanged =
    newName !== currentDrink.name ||
    newPrice !== currentDrink.price ||
    newDescription !== (currentDrink.description ?? '') ||
    newImgUrl !== (currentDrink.imgUrl ?? '');

  const isEditing = isNameEditing || isPriceEditing;

  const resetEditing = useCallback(() => {
    setIsNameEditing(false);
    setIsPriceEditing(false);
    setNewName(currentDrink.name);
    setNewPrice(currentDrink.price);
    setNewDescription(currentDrink.description ?? '');
    setNewImgUrl(currentDrink.imgUrl ?? '');
  }, [currentDrink]);

  useEffect(() => {
    if (!isEditing || isItemChanged) {
      return;
    }

    const handleClickOutside = (event: MouseEvent) => {
      if (
        cardRef.current &&
        !cardRef.current.contains(event.target as Node)
      ) {
        resetEditing();
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [isEditing, isItemChanged, resetEditing]);

  const handleSaveChanges = async () => {
    if (!isItemChanged) {
      resetEditing();
      return;
    }

    const changedDrink: CreateDrinkPayload = {
      name: newName.trim(),
      price: newPrice,
      description: newDescription.trim(),
      imgUrl: newImgUrl.trim(),
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
    <div className="drink-card" ref={cardRef}>
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
      <div className="drink-img-url">
        {currentDrink.imgUrl ? (
          <img
            src={currentDrink.imgUrl}
            alt={currentDrink.name}
            className="drink-image"
          />
        ) : (
          <div className="drink-image-placeholder">Изображение отсутствует</div>
        )}
      </div>
      <div className="drink-card-description">
        {currentDrink.description || 'Описание отсутствует'}
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
        <div>
          <div className="drink-card-link">
            <NavLink to={`/drink/${currentDrink.id}`} className="drink-link">
              Подробнее
            </NavLink>
          </div>
        </div>
      )}
    </div>
  );
};
