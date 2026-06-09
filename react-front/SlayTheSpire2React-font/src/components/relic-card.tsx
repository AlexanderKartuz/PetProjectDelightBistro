import type { Relic } from '../types/relic'

interface RelicCardProps {
  relic: Relic
  onDelete?: (id: number) => void
  deleting?: boolean
}

export const RelicCard = function ({ relic, onDelete, deleting }: RelicCardProps) {
  const rarity = relic.rarity.trim()

  const getRarityClass = () => {
    const normalized = rarity.toLowerCase()

    if (normalized.includes('starter')) return 'relic-card--starter'
    if (normalized.includes('common')) return 'relic-card--common'
    if (normalized.includes('uncommon')) return 'relic-card--uncommon'
    if (normalized.includes('rare')) return 'relic-card--rare'
    if (normalized.includes('boss')) return 'relic-card--boss'
    if (normalized.includes('shop')) return 'relic-card--shop'

    return 'relic-card--default'
  }

  return (
    <article className={`relic-card ${getRarityClass()}`}>
      <div className="relic-card__frame">
        <div className="relic-card__glow" aria-hidden="true" />
        <div className="relic-card__poster">
          {relic.urlImage ? (
            <img src={relic.urlImage} alt={relic.name} loading="lazy" />
          ) : (
            <div className="relic-card__no-image">?</div>
          )}
        </div>
      </div>
      <div className="relic-card__info">
        <span className="relic-card__badge">{rarity}</span>
        <h3 className="relic-card__title">{relic.name}</h3>
        {onDelete && (
          <button
            type="button"
            className="relic-card__delete"
            onClick={() => onDelete(relic.id)}
            disabled={deleting}
          >
            {deleting ? 'Удаление...' : 'Удалить'}
          </button>
        )}
      </div>
    </article>
  )
}
