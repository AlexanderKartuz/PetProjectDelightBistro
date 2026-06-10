import type { SteamGame } from "../types/steam-game";

const API_BASE = "https://localhost:7284";

export async function getGames(): Promise<SteamGame[]> {
  const response = await fetch(`${API_BASE}/api/Catalog/GetGames`);

  if (!response.ok) {
    throw new Error(`Request error: ${response.status}`);
  }

  return response.json();
}
