import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import "./Login.css";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await login(email, password);
      navigate("/");
    } catch (error) {
      console.error("Błąd logowania:", error);
      alert("Nie udało się zalogować. Sprawdź e-mail i hasło.");
    }
  };

  return (
    <div className="login-container">
      <div className="login-box">
        <h2>Zaloguj się</h2>
        <form onSubmit={handleSubmit} className="login-form">
          <div className="form-group">
            <label>Adres E-mail / Login: </label>
            <input
              type="text"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Twój e-mail lub login"
              required
            />
          </div>

          <div className="form-group">
            <label>Hasło: </label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="login-btn">
            Zaloguj się do systemu
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;
