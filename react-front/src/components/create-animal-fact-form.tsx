import { useState, useEffect } from 'react'
import { getAnimalSpeciesNames } from '../services/animal-fact-service'
import type { AnimalFact } from '../types/animal-fact'

interface CreateAnimalFactFormProps {
  onCreated: (fact: AnimalFact) => Promise<void>
}

export const CreateAnimalFactForm = function ({ onCreated }: CreateAnimalFactFormProps) {
  const [speciesList, setSpeciesList] = useState<string[]>([])
  const [selectedSpecies, setSelectedSpecies] = useState('')
  const [inputText, setInputText] = useState('')
  const [loadingSpecies, setLoadingSpecies] = useState(true)
  const [formError, setFormError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false

    const fetchSpecies = async () => {
      try {
        const data = await getAnimalSpeciesNames()
        if (!cancelled) {
          setSpeciesList(data)
          if (data.length > 0) {
            setSelectedSpecies(data[0])
          }
        }
      } catch (err) {
        console.error(err)
      } finally {
        if (!cancelled) {
          setLoadingSpecies(false)
        }
      }
    }

    fetchSpecies()

    return () => {
      cancelled = true
    }
  }, [])

  const handleButtonClick = async () => {
    if (!inputText.trim()) {
      setFormError('Пожалуйста, введите текст факта.')
      return
    }

    try {
      setFormError(null)
      await onCreated({
        animalSpeciesName: selectedSpecies,
        text: inputText.trim(),
      })
      setInputText('')
    } catch (err) {
      setFormError(err instanceof Error ? err.message : 'Не удалось сохранить факт')
    }
}

  return (
    <div className="facts-form-container">
      <div className="form-group">
        <label className="form-label">Вид животного:</label>
        <select
          className="form-input"
          value={selectedSpecies}
          onChange={(e) => setSelectedSpecies(e.target.value)}
          disabled={loadingSpecies}
        >
          {loadingSpecies ? (
            <option>Загрузка...</option>
          ) : (
            speciesList.map((name) => (
              <option key={name} value={name}>
                {name}
              </option>
            ))
          )}
        </select>
      </div>

      <div className="form-group">
        <label className="form-label">Текст факта:</label>
        <textarea
          className="form-input comment-textarea"
          rows={4}
          placeholder="Введите интересный факт..."
          value={inputText}
          onChange={(e) => setInputText(e.target.value)}
          required
        ></textarea>
      </div>

      <div className="find-animal align-items-center">
        <button 
          type="button" 
          className="find-animal-button"
          onClick={handleButtonClick}
        >
          Добавить факт
        </button>
        
        {formError && (
          <span className="zoo-name-feedback zoo-name-feedback-invalid">
            {formError}
          </span>
        )}
      </div>
    </div>
  )
}