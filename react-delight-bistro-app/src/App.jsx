import './App.css';
import { DrinksList } from './components/drink-list';
import { CreteDrinkForm } from './components/create-drinks-form';
import { DrinkPage } from './pages/drink-page';
import { NavLink, Route, Routes } from 'react-router-dom';

function App() {
  return (
    <>
      <section id="center">
        <header>
          <NavLink to="/" className="header-link" end>
            Drink list
          </NavLink>
          <NavLink to="/drink/create" className="header-link">
            Create Drink
          </NavLink>
        </header>
        <Routes>
          <Route path="/" element={<DrinksList />} />
          <Route path="/drink/create" element={<CreteDrinkForm />} />
          <Route path="/drink/:id" element={<DrinkPage />} />
        </Routes>
      </section>
      <section id="spacer"></section>
    </>
  );
}

export default App;
