const API_BASE = 'https://localhost:7042'

export interface Quote {
  id?: number;
  name: string;
  url: string;
  quote_text: string;
}

export async function getQuotes(): Promise<Quote[]> {
  const response = await fetch(`${API_BASE}/GetQuotes`)

  if (!response.ok) {
    throw new Error(`Ошибка запроса цитат: ${response.status}`)
  }

  return response.json()
}
