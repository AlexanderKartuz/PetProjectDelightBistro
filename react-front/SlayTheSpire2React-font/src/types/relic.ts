export interface Relic {
  id: number
  name: string
  urlImage: string
  rarity: string
}

export interface CreateRelicPayload {
  name: string
  urlImage: string
  rarity: string
}
