import { useState } from 'react';
import './App.css';
import { DrinksList } from './components/drink-list';

function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <section id="center">
        <DrinksList />
      </section>

      <section id="spacer"></section>
    </>
  );
}

export default App;
