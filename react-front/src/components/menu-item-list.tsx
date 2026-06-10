import { useMemo, useState } from 'react'
import { useMenuItems } from '../hooks/use-menu-items'
import { CreateMenuItemForm } from './create-menu-item-form'
import { MenuItemCard } from './menu-item-card'
import type { MenuItem } from '../types/menu-item'

const MAINS_CATEGORY = 'Mains'

function getCategoryFilters(items: MenuItem[]) {
    const categories = [
        ...new Set(
            items
                .map((item) => item.category)
                .filter((category) => category.length > 0 && category !== MAINS_CATEGORY),
        ),
    ]

    return [
        { label: MAINS_CATEGORY, category: MAINS_CATEGORY },
        ...categories.map((category) => ({ label: category, category })),
    ]
}

export const MenuItemList = function () {
    const [category, setCategory] = useState(MAINS_CATEGORY)
    const { items, loading, error, addMenuItem } = useMenuItems()

    const filters = useMemo(() => getCategoryFilters(items), [items])

    const visibleItems =
        category === MAINS_CATEGORY
            ? items
            : items.filter((item) => item.category === category)

    return (
        <section id="menu" className="menu-section">
            <CreateMenuItemForm onCreated={addMenuItem} />
            <div className="menu-container">
                <h2 className="menu-heading-lg">This week specials!</h2>

                <div className="menu-filter-items">
                    {filters.map((filter) => (
                        <button
                            key={filter.category}
                            type="button"
                            className={`btn-filter${category === filter.category ? ' btn-filter--active' : ''}`}
                            onClick={() => setCategory(filter.category)}
                        >
                            {filter.label}
                        </button>
                    ))}
                </div>

                {loading && <p className="menu-list__status">Loading menu...</p>}
                {error && <p className="menu-list__status menu-list__status--error">{error}</p>}

                {!loading && !error && visibleItems.length === 0 && (
                    <p className="menu-list__status">No menu items yet.</p>
                )}

                {!loading && !error && visibleItems.length > 0 && (
                    <div className="menu-item-container">
                        {visibleItems.map((item) => (
                            <MenuItemCard key={item.id} item={item} />
                        ))}
                    </div>
                )}
            </div>
        </section>
    )
}
