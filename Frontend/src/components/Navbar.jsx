import { Link } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";

const Navbar = () => {
  const { token, logout, user } = useContext(AuthContext);

  const imie = user?.username?.split(" ")[0] || "Nieznajomy";

  return (
    <nav
      style={{
        padding: "1rem",
        background: "#333",
        color: "white",
        display: "flex",
        justifyContent: "space-between",
      }}
    >
      <div>
        {token && (
          <>
            <Link
              to="/"
              style={{
                color: "white",
                marginRight: "15px",
                textDecoration: "none",
              }}
            >
              Urodziny
            </Link>
            <Link
              to="/friends"
              style={{
                color: "white",
                marginRight: "15px",
                textDecoration: "none",
              }}
            >
              Znajomi
            </Link>
            <Link
              to="/plans"
              style={{
                color: "white",
                marginRight: "15px",
                textDecoration: "none",
              }}
            >
              Plany
            </Link>
            <Link
              to="/add-plan"
              style={{
                color: "white",
                marginRight: "15px",
                textDecoration: "none",
              }}
            >
              Dodaj Plan
            </Link>
          </>
        )}
      </div>

      <div>
        {token ? (
          <>
            <span style={{ marginRight: "15px" }}>Witaj, {imie}!</span>
            <Link
              to="/profile"
              style={{
                color: "white",
                marginRight: "15px",
                textDecoration: "none",
              }}
            >
              Profil
            </Link>
            <button onClick={logout} style={{ cursor: "pointer" }}>
              Wyloguj
            </button>
          </>
        ) : (
          <>
            <Link
              to="/login"
              style={{
                color: "white",
                textDecoration: "none",
                marginRight: "15px",
              }}
            >
              Zaloguj
            </Link>
            <Link
              to="/register"
              style={{
                color: "white",
                textDecoration: "none",
              }}
            >
              Zarejestruj się
            </Link>
          </>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
