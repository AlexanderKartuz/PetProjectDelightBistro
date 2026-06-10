import type { SteamGame } from "../types/steam-game";

export async function getGames(): Promise<SteamGame[]> {
  const response = await fetch("/api/Catalog/GetGames");

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
