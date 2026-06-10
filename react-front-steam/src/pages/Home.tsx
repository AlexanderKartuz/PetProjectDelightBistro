import { Link } from "react-router-dom";

export const Home = () => {
  return (
    <section className="home">
      <h1 className="home__title">Steam Store Front</h1>
      <p className="home__text">React-app for game catalog</p>
      <Link to="/gameCatalog" className="home__link">
        Open catalog
      </Link>
    </section>
  );
};
