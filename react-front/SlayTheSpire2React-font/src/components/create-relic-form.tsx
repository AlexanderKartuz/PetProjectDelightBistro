import { useState, type FormEvent } from 'react'
import { createRelic } from '../services/relic-service'
import type { Relic } from '../types/relic'
import { RelicCard } from './relic-card'

interface CreateRelicFormProps {
  onCreated: (relic: Relic) => void
}

const RARITY_OPTIONS = [
  'Starter Relic',
  'Common Relic',
  'Uncommon Relic',
  'Rare Relic',
  'Boss Relic',
  'Shop Relic',
] as const

export const CreateRelicForm = function ({ onCreated }: CreateRelicFormProps) {
  const [isOpen, setIsOpen] = useState(false)
  const [name, setName] = useState('')
  const [urlImage, setUrlImage] = useState('')
  const [rarity, setRarity] = useState<string>(RARITY_OPTIONS[1])
  const [submitting, setSubmitting] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const previewRelic: Relic = {
    id: 0,
    name: name || 'Название реликвии',
    urlImage,
    rarity: rarity || 'Common Relic',
  }

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setSubmitting(true)
    setError(null)

    try {
      const relic = await createRelic({ name, urlImage, rarity })
      onCreated(relic)

      setName('')
      setUrlImage('')
      setRarity(RARITY_OPTIONS[1])
      setIsOpen(false)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Не удалось создать реликвию')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <div className={`create-relic-form ${isOpen ? 'create-relic-form--open' : ''}`}>
      <button
        type="button"
        className="create-relic-form__toggle"
        onClick={() => setIsOpen((open) => !open)}
        aria-expanded={isOpen}
      >
        <span className="create-relic-form__toggle-content">
          <span className="create-relic-form__emblem" aria-hidden="true">
            ✦
          </span>
          <span className="create-relic-form__toggle-text">
            <span className="create-relic-form__toggle-title">Добавить реликвию</span>
            <span className="create-relic-form__toggle-subtitle">
              {isOpen ? 'Скрыть форму' : 'Открыть форму создания'}
            </span>
          </span>
        </span>
        <span className="create-relic-form__chevron" aria-hidden="true" />
      </button>

      <div className="create-relic-form__dropdown">
        <div className="create-relic-form__dropdown-inner">
          <form className="create-relic-form__content" onSubmit={handleSubmit}>
            <div className="create-relic-form__panel">
              <header className="create-relic-form__header">
                <div className="create-relic-form__intro">
                  <h3 className="create-relic-form__heading">Новая реликвия</h3>
                  <p className="create-relic-form__subtitle">
                    Добавьте артефакт в коллекцию и проверьте, как он будет выглядеть
                  </p>
                </div>
              </header>

              <div className="create-relic-form__body">
                <div className="create-relic-form__fields">
                  <label className="create-relic-form__field">
                    <span className="create-relic-form__label">Название</span>
                    <input
                      type="text"
                      value={name}
                      onChange={(event) => setName(event.target.value)}
                      placeholder="Burning Blood"
                      required
                    />
                  </label>

                  <label className="create-relic-form__field">
                    <span className="create-relic-form__label">URL изображения</span>
                    <input
                      type="url"
                      value={urlImage}
                      onChange={(event) => setUrlImage(event.target.value)}
                      placeholder="https://..."
                      required
                    />
                  </label>

                  <fieldset className="create-relic-form__rarity">
                    <legend className="create-relic-form__label">Редкость</legend>
                    <div className="create-relic-form__rarity-options">
                      {RARITY_OPTIONS.map((option) => (
                        <button
                          key={option}
                          type="button"
                          className={`create-relic-form__rarity-chip ${
                            rarity === option ? 'create-relic-form__rarity-chip--active' : ''
                          }`}
                          onClick={() => setRarity(option)}
                        >
                          {option.replace(' Relic', '')}
                        </button>
                      ))}
                    </div>
                  </fieldset>

                  {error && <p className="create-relic-form__error">{error}</p>}

                  <button
                    type="submit"
                    className="create-relic-form__submit"
                    disabled={submitting}
                  >
                    {submitting ? 'Сохранение...' : 'Создать реликвию'}
                  </button>
                </div>

                <aside className="create-relic-form__preview">
                  <span className="create-relic-form__preview-label">Превью</span>
                  <div className="create-relic-form__preview-stage">
                    <RelicCard relic={previewRelic} />
                  </div>
                </aside>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
