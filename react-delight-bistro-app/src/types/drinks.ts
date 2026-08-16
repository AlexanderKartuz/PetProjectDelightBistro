export interface Drink {
  id: number;
  name: string;
  price: number;
  description?: string | null;
  imgUrl?: string | null;
}

/** Body Create/Change — как DrinkRequest в Minimal API (без id). */
export interface CreateDrinkPayload {
  name: string;
  price: number;
  description?: string | null;
  imgUrl?: string | null;
}
