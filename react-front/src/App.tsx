import { NavLink, Route, Routes } from "react-router-dom";
import "./App.css";
import './animal-world.css'
import { Movie } from "./pages/Movie";
import { Home } from "./pages/Home";
import { MovieDetail } from "./pages/MovieDetail";
import { RockQuotes } from "./pages/RockQuotes";
import { AnimalFacts } from "./pages/AnimalFacts";
import { LittleLemon } from "./pages/LittleLemon";
function App() {
  return (
    <>
      <header className="app-header">
        <NavLink to="/" className="app-header__brand" end>
          Smile
        </NavLink>
        <nav className="app-nav" aria-label="Main navigation">
          <NavLink
            to="/"
            className={({ isActive }) =>
              `app-nav__link${isActive ? " app-nav__link--active" : ""}`
            }
            end
          >
            Home
          </NavLink>
          <NavLink
            to="/movies"
            className={({ isActive }) =>
              `app-nav__link${isActive ? " app-nav__link--active" : ""}`
            }
          >
            Movies
          </NavLink>
          <NavLink
            to="/animal-facts"
            className={({ isActive }) =>
              `app-nav__link${isActive ? " app-nav__link--active" : ""}`
            }
          >
            Animal Facts
          </NavLink>
          <NavLink
            to="/rock-quotes"
            className={({ isActive }) =>
              `app-nav__link${isActive ? " app-nav__link--active" : ""}`
            }
          >
            Rock Quotes
          </NavLink>
          <NavLink
            to="/menu"
            className={({ isActive }) =>
              `app-nav__link${isActive ? " app-nav__link--active" : ""}`
            }
          >
            LLMenu
          </NavLink>
        </nav>
      </header>
      <main className="app-main">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/movies" element={<Movie />} />
          <Route path="/movie/:id" element={<MovieDetail />} />
          <Route path="/animal-facts" element={<AnimalFacts />} />
          <Route path="/rock-quotes" element={<RockQuotes />} />
          <Route path="/menu" element={<LittleLemon />} />
        </Routes>
      </main>
    </>
  );
}

export default App;
