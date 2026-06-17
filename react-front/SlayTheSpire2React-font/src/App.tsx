import { useState } from 'react'
import { RelicList } from './components/relic-list'
import './App.css'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <section id="center">

        <div>
          <h1>Hello World!</h1>
        </div>
        <button
          type="button"
          className="counter"
          onClick={() => setCount((count) => count + 1)}
        >
          Click me {count}
        </button>
      </section>

      <RelicList />

      <section id="spacer"></section>
    </>
  )
}

export default App
