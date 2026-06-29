export interface SteamGame {
  id: number;
  title: string;
  imageUrl: string;
  price: number;
  averageRating?: number | null;
  reviewsCount: number;
  genres: string[];
  description?: string;
}
