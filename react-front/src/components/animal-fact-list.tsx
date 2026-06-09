import { useCallback, useEffect, useState } from 'react'
import type { AnimalFact } from '../types/animal-fact'
import { getFacts, createFact } from '../services/animal-fact-service'
import { CreateAnimalFactForm } from './create-animal-fact-form'
import { AnimalFactCard } from './animal-fact-card'

export const AnimalFactsList = function () {
  const [facts, setFacts] = useState<AnimalFact[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false

    const fetchFacts = async () => {
      try {
        const data = await getFacts()
        if (!cancelled) {
          setFacts(data)
          setError(null)
        }
      } catch (err) {
        if (!cancelled) {
          setError(err instanceof Error ? err.message : 'Не удалось загрузить факты')
        }
      } finally {
        if (!cancelled) {
          setLoading(false)
        }
      }
    }

    fetchFacts()

    return () => {
      cancelled = true
    }
  }, [])

  const handleFactCreated = useCallback(async (newFactData: AnimalFact) => {
    try {
      await createFact(newFactData)
      setFacts((oldFacts) => [newFactData, ...oldFacts])
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось сохранить факт')
    }
  }, [])

  if (loading) {
    return <p className="empty-list-note">Загрузка фактов...</p>
  }

  if (error) {
    return <p className="empty-list-note" style={{ color: 'red' }}>{error}</p>
  }

  return (
    <div className="main-place" style={{ padding: '20px' }}>
      <h2 className="green-title page-title">Факты о животных</h2>
      <div className="green-line"></div>

      <CreateAnimalFactForm onCreated={handleFactCreated} />

      <h3 className="green-title section-title">Известные факты</h3>
      <div className="green-line"></div>

      <div className="facts-list">
        {facts.length === 0 ? (
          <p className="empty-list-note">Фактов пока нет.</p>
        ) : (
          facts.map((fact, index) => (
            <AnimalFactCard key={index} fact={fact} />
          ))
        )}
      </div>
    </div>
  )
}