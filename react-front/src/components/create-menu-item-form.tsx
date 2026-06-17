import { useState, type SubmitEvent } from 'react'
import { createMenuItem } from '../services/menu-item-service'
import type { MenuItem } from '../types/menu-item'
import { MenuItemCard } from '../components/menu-item-card'

const CATEGORIES = ['Lunch', 'Desserts', 'Specials', 'Drinks', 'Mains'] as const

interface CreateMenuItemFormProps {
    onCreated: (menuItem: MenuItem) => void
}

export const CreateMenuItemForm = function ({ onCreated }: CreateMenuItemFormProps) {
    const [name, setName] = useState('')
    const [imageUrl, setImageUrl] = useState('')
    const [price, setPrice] = useState(0)
    const [description, setDescription] = useState('')
    const [category, setCategory] = useState<string>(CATEGORIES[0])
    const [submitting, setSubmitting] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault()
        setSubmitting(true)
        setError(null)

        try {
            const menuItem = await createMenuItem({
                name,
                imageUrl,
                price,
                description,
                category,
            })
            onCreated(menuItem)

            setName('')
            setImageUrl('')
            setPrice(0)
            setDescription('')
            setCategory(CATEGORIES[0])
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Failed to create menu item')
        } finally {
            setSubmitting(false)
        }
    }

    return (
        <form className="create-menu-item-form" onSubmit={handleSubmit}>
            <h3 className="create-menu-item-form__heading">Add menu item</h3>

            <div className="create-menu-item-form__fields">

                <label className="create-menu-item-form__field">
                    <span>Name</span>
                    <input
                        type="text"
                        value={name}
                        onChange={(event) => setName(event.target.value)}
                        required
                    />
                </label>

                <label className="create-menu-item-form__field">
                    <span>Image URL</span>
                    <input
                        type="url"
                        value={imageUrl}
                        onChange={(event) => setImageUrl(event.target.value)}
                        required
                    />
                </label>

                <label className="create-menu-item-form__field">
                    <span>Price</span>
                    <input
                        type="number"
                        min={0}
                        step={0.01}
                        value={price}
                        onChange={(event) => setPrice(Number(event.target.value))}
                        required
                    />
                </label>

                <label className="create-menu-item-form__field">
                    <span>Description</span>
                    <textarea
                        value={description}
                        onChange={(event) => setDescription(event.target.value)}
                        rows={3}
                        required
                    />
                </label>

                <label className="create-menu-item-form__field">
                    <span>Category</span>
                    <select
                        value={category}
                        onChange={(event) => setCategory(event.target.value)}
                        required
                    >
                        {CATEGORIES.map((item) => (
                            <option key={item} value={item}>
                                {item}
                            </option>
                        ))}
                    </select>
                </label>
            </div>

            <div className="create-menu-item-form__preview">
                <span className="create-menu-item-form__preview-label">Preview</span>
                <MenuItemCard
                    showOrderButton={false}
                    item={{
                        name: name || 'Menu item name',
                        imageUrl,
                        price,
                        description: description || 'Menu item description',
                    }}
                />
            </div>

            {error && <p className="create-menu-item-form__error">{error}</p>}

            <button type="submit" disabled={submitting} className="btn-submit">
                {submitting ? 'Saving...' : 'Create menu item'}
            </button>
        </form>
    )
}
