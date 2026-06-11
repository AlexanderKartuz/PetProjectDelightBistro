export interface Drink {
  id: number;
  name: string;
  price: number;
  description?: string | null;
  imgUrl?: string | null;
}
export interface CreateDrinkPayload {
  name: string;
  price: number;
  description?: string | null;
  imgUrl?: string | null;
}
