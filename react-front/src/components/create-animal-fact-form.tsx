import React, { useState, useEffect } from 'react'
import { getAnimalSpeciesNames } from '../services/animal-fact-service'
import type { AnimalFact } from '../types/animal-fact'

interface CreateAnimalFactFormProps {
  onCreated: (fact: AnimalFact) => void
}

export const CreateAnimalFactForm = function ({ onCreated }: CreateAnimalFactFormProps) {
  const [speciesList, setSpeciesList] = useState<string[]>([])
  const [selectedSpecies, setSelectedSpecies] = useState('')
  const [inputText, setInputText] = useState('')
  const [loadingSpecies, setLoadingSpecies] = useState(true)

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

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault()
    if (!inputText.trim()) {
      return
    }

    onCreated({
      animalSpeciesName: selectedSpecies,
      text: inputText.trim(),
    })

    setInputText('')
  }

  return (
    <form className="facts-form-container" onSubmit={handleSubmit}>
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
        <button type="submit" className="find-animal-button">
          Добавить факт
        </button>
      </div>
    </form>
  )
}