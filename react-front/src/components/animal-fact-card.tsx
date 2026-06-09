import type { AnimalFact } from '../types/animal-fact'

interface AnimalFactCardProps {
  fact: AnimalFact
}

export const AnimalFactCard = function ({ fact }: AnimalFactCardProps) {
  return (
    <div className="animal fact-item">
      <span className="fact-animal-type">{fact.animalSpeciesName}</span>
      <p className="comment-text">{fact.text}</p>
    </div>
  )
}