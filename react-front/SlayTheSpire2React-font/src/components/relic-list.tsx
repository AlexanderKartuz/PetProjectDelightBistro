import { useCallback, useEffect, useState } from 'react'
import type { Relic } from '../types/relic'
import { getRelics, deleteRelic } from '../services/relic-service'
import { CreateRelicForm } from './create-relic-form'
import { RelicCard } from './relic-card'
import './relic-list.css'

export const RelicList = function () {
  const [relics, setRelics] = useState<Relic[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [deletingId, setDeletingId] = useState<number | null>(null)

  useEffect(() => {
    let cancelled = false

    const loadRelics = async () => {
      try {
        const data = await getRelics()

        if (!cancelled) {
          setRelics(data)
          setError(null)
        }
      } catch (err) {
        if (!cancelled) {
          setError(err instanceof Error ? err.message : 'Не удалось загрузить реликвии')
        }
      } finally {
        if (!cancelled) {
          setLoading(false)
        }
      }
    }

    loadRelics()

    return () => {
      cancelled = true
    }
  }, [])

  const handleRelicCreated = useCallback((relic: Relic) => {
    setRelics((old) => [...old, relic])
  }, [])

  const handleDelete = useCallback(async (id: number) => {
    setDeletingId(id)
    setError(null)

    try {
      await deleteRelic(id)
      setRelics((relics) => relics.filter((relic) => relic.id !== id))
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось удалить реликвию')
    } finally {
      setDeletingId(null)
    }
  }, [])

  if (loading) {
    return <p className="relic-list__status">Загрузка реликвий...</p>
  }

  if (error) {
    return <p className="relic-list__status relic-list__status--error">{error}</p>
  }

  return (
    <section className="relic-list">
      <h2 className="relic-list__heading">Реликвии</h2>
      <CreateRelicForm onCreated={handleRelicCreated} />
      <div className="relic-list__grid">
        {relics.map((relic) => (
          <RelicCard
            key={relic.id}
            relic={relic}
            onDelete={handleDelete}
            deleting={deletingId === relic.id}
          />
        ))}
      </div>
    </section>
  )
}
