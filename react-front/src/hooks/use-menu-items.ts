import { useEffect, useState } from 'react'
import type { MenuItem } from '../types/menu-item'
import { getMenuItems } from '../services/menu-item-service'

export function useMenuItems() {
    const [items, setItems] = useState<MenuItem[]>([])
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        let cancelled = false

        async function loadMenu() {
            setLoading(true)

            try {
                const data = await getMenuItems()

                if (!cancelled) {
                    setItems(data)
                    setError(null)
                }
            } catch (err) {
                if (!cancelled) {
                    setError(err instanceof Error ? err.message : 'Failed to load menu')
                }
            } finally {
                if (!cancelled) {
                    setLoading(false)
                }
            }
        }

        loadMenu()

        return () => {
            cancelled = true
        }
    }, [])

    function addMenuItem(menuItem: MenuItem) {
        setItems((prev) => [...prev, menuItem])
    }

    return { items, loading, error, addMenuItem }
}
