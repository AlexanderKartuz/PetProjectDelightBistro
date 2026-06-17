import type { CreateMenuItemPayload, MenuItem } from "../types/menu-item";

const API_BASE = "https://localhost:7100";

export async function getMenuItems(): Promise<MenuItem[]> {
  const url = `${API_BASE}/GetMenuItems`;

  const response = await fetch(url);

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }

  return (await response.json()) as MenuItem[];
}

export async function createMenuItem(
  payload: CreateMenuItemPayload,
): Promise<MenuItem> {
  const response = await fetch(`${API_BASE}/CreateMenuItem`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(payload),
  });

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }

  const menuItem = (await response.json()) as MenuItem;

  return menuItem;
}

export async function deleteMenuItem(id: number): Promise<void> {
  const response = await fetch(`${API_BASE}/DeleteMenuItem`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(id),
  });

  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }
}
