import type { PaginatedResponse } from "../types/pagination";
import type { SteamGame } from "../types/steam-game";

interface GetGamesParams {
  page?: number;
  pageSize?: number;
  maxPrice?: string;
  genre?: string;
}

export async function getGames(
  params: GetGamesParams = {},
): Promise<PaginatedResponse<SteamGame>> {
  const queryParams = new URLSearchParams();
  if (params.page) queryParams.append("page", params.page.toString());
  if (params.pageSize)
    queryParams.append("pageSize", params.pageSize.toString());
  if (params.genre) queryParams.append("genre", params.genre.toString());
  if (params.maxPrice)
    queryParams.append("maxPrice", params.maxPrice.toString());

  const query = queryParams.toString();
  const response = await fetch(
    `/api/Catalog/GetGames${query ? `?${query}` : ""}`,
  );

  if (!response.ok) {
    throw new Error(`Request error: ${response.status}`);
  }

  return response.json();
}

export async function getGame(id: number): Promise<SteamGame | null> {
  const response = await fetch(`/api/Catalog/GetGameDetails?id=${id}`);

  if (response.status === 404) {
    return null;
  }

  if (!response.ok) {
    throw new Error(`Request error: ${response.status}`);
  }

  return response.json() as Promise<SteamGame>;
}
