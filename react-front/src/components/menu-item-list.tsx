import { useState } from "react";
import { useMenuItems } from "../hooks/use-menu-items";
import { deleteMenuItem } from "../services/menu-item-service";
import { CreateMenuItemForm } from "./create-menu-item-form";
import { MenuItemCard } from "./menu-item-card";
import type { MenuItem } from "../types/menu-item";

const MAINS_CATEGORY = "Mains";

function getCategoryFilters(items: MenuItem[]): string[] {
  const categories = [
    ...new Set(
      items
        .map((item) => item.category)
        .filter(
          (category) => category.length > 0 && category !== MAINS_CATEGORY,
        ),
    ),
  ];

  return [MAINS_CATEGORY, ...categories];
}

export function MenuItemList() {
  const [category, setCategory] = useState(MAINS_CATEGORY);
  const [deletingId, setDeletingId] = useState<number | null>(null);
  const { items, loading, error, setError, addMenuItem, removeMenuItem } =
    useMenuItems();

  async function handleDelete(id: number) {
    setDeletingId(id);
    setError(null);

    try {
      await deleteMenuItem(id);
      removeMenuItem(id);
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Failed to delete menu item",
      );
    } finally {
      setDeletingId(null);
    }
  }

  const filters = getCategoryFilters(items);

  const visibleItems =
    category === MAINS_CATEGORY
      ? items
      : items.filter((item) => item.category === category);

  return (
    <section id="menu" className="menu-section">
      <CreateMenuItemForm onCreated={addMenuItem} />
      <div className="menu-container">
        <h2 className="menu-heading-lg">This week specials!</h2>

        {!error && (
          <div className="menu-filter-items">
            {filters.map((filterCategory) => (
              <button
                key={filterCategory}
                type="button"
                className={`btn-filter${category === filterCategory ? " btn-filter--active" : ""}`}
                onClick={() => setCategory(filterCategory)}
              >
                {filterCategory}
              </button>
            ))}
          </div>
        )}

        {loading && <p className="menu-list__status">Loading menu...</p>}
        {error && (
          <p className="menu-list__status menu-list__status--error">{error}</p>
        )}
        {!loading && !error && visibleItems.length === 0 && (
          <p className="menu-list__status">No menu items yet.</p>
        )}
        {!loading && visibleItems.length > 0 && (
          <div className="menu-item-container">
            {visibleItems.map((item) => (
              <MenuItemCard
                key={item.id}
                item={item}
                onDelete={handleDelete}
                deleting={deletingId === item.id}
              />
            ))}
          </div>
        )}
      </div>
    </section>
  );
}
