import { NavLink, Route, Routes } from "react-router-dom";
import "./App.css";
import { Movie } from "./pages/Movie";
import { Home } from "./pages/Home";
import { MovieDetail } from "./pages/MovieDetail";
import { RockQuotes } from "./pages/RockQuotes";

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
            to="/rock-quotes"
            className={({ isActive }) =>
              `app-nav__link${isActive ? " app-nav__link--active" : ""}`
            }
          >
            Rock Quotes
          </NavLink>
        </nav>
      </header>
      <main className="app-main">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/movies" element={<Movie />} />
          <Route path="/movie/:id" element={<MovieDetail />} />
          <Route path="/rock-quotes" element={<RockQuotes />} />
        </Routes>
      </main>
    </>
  );
}

export default App;
