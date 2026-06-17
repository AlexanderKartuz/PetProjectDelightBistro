import { useEffect, useState } from 'react'
import { RockQuoteCard } from '../components/rock-quote-card'
import { getQuotes, type Quote } from '../services/quote-service'

export const RockQuotes = () => 
{
  const [quotes, setQuotes] = useState<Quote[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => 
  {
    let cancelled = false

    const fetchQuotes = async () => 
    {
      setLoading(true)
      setError(null)

      try 
      {
        const data = await getQuotes()

        if (!cancelled) 
        {
          setQuotes(data)
        }
      } catch (err) 
      {
        if (err instanceof Error && err.message === 'Failed to fetch') 
        {
          if (!cancelled) 
          {
            setError('Ты забыл включить QuotesMinimalApi с цитатами')
          }
        } else if (!cancelled) 
        {
          setError(
            err instanceof Error ? err.message : 'Не удалось загрузить рок-мудрость',
          )
        }
      } finally 
      {
        if (!cancelled) 
        {
          setLoading(false)
        }
      }
    }

    fetchQuotes()

    return () => 
    {
      cancelled = true
    }
  }, [])

  if (loading) 
  {
    return <p className="movie-list__status">Загрузка мудрости...</p>
  }

  if (error) 
  {
    return (
      <p className="movie-list__status movie-list__status--error">{error}</p>
    )
  }

  return (
    <section className="quotes-page-container">
      <h2 className="quote-page-title">Мудрость Рок-Легенд</h2>

      <div className="quotes-catalog-grid">
        {
        quotes.length === 0 ? (
          <p className="movie-list__status">Цитат пока нет. Добавь их через Swagger!</p>
        ) : (
          quotes.map((quote, index) => (
            <RockQuoteCard
              key={quote.id || index}
              name={quote.name}
              url={quote.url}
              quote_text={quote.quote_text}
            />
          ))
        )
        }
      </div>
    </section>
  )
}
