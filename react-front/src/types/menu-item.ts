export interface MenuItem {
    id: number
    name: string
    price: number
    description: string
    imageUrl: string
    category: string
}

export type MenuItemCardData = Pick<MenuItem, 'name' | 'price' | 'description' | 'imageUrl'>

export interface CreateMenuItemPayload {
    name: string
    price: number
    description: string
    imageUrl: string
    category: string
}
