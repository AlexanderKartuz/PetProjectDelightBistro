import { useState, type KeyboardEvent } from 'react'
import { Link } from 'react-router-dom'

interface MovieTagsProps {
  tags: string[]
  clickable?: boolean
  editable?: boolean
  onAdd?: (tagName: string) => void | Promise<void>
  onRemove?: (tagName: string) => void | Promise<void>
  adding?: boolean
  removingTag?: string | null
}

export const MovieTags = function ({
  tags,
  clickable = false,
  editable = false,
  onAdd,
  onRemove,
  adding = false,
  removingTag = null,
}: MovieTagsProps) {
  const [input, setInput] = useState('')

  const handleAdd = async () => {
    const trimmed = input.trim()
    if (!trimmed || !onAdd) {
      return
    }

    await onAdd(trimmed)
    setInput('')
  }

  const handleKeyDown = (event: KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') {
      event.preventDefault()
      void handleAdd()
    }
  }

  return (
    <div className="movie-tags">
      <div className="movie-tags__list">
        {tags.length === 0 && !editable && (
          <span className="movie-tags__empty">Нет тегов</span>
        )}
        {tags.map((tag) => (
          <span key={tag} className="movie-tags__chip">
            {clickable ? (
              <Link
                to={`/movies?tag=${encodeURIComponent(tag)}`}
                className="movie-tags__link"
              >
                {tag}
              </Link>
            ) : (
              <span className="movie-tags__label">{tag}</span>
            )}
            {editable && onRemove && (
              <button
                type="button"
                className="movie-tags__remove"
                onClick={() => onRemove(tag)}
                disabled={removingTag === tag}
                aria-label={`Удалить тег ${tag}`}
              >
                ×
              </button>
            )}
          </span>
        ))}
      </div>

      {editable && onAdd && (
        <div className="movie-tags__editor">
          <input
            type="text"
            className="movie-tags__input"
            value={input}
            onChange={(event) => setInput(event.target.value)}
            onKeyDown={handleKeyDown}
            placeholder="Новый тег"
            disabled={adding}
          />
          <button
            type="button"
            className="movie-tags__add"
            onClick={() => void handleAdd()}
            disabled={adding || !input.trim()}
            aria-label="Добавить тег"
          >
            +
          </button>
        </div>
      )}
    </div>
  )
}
