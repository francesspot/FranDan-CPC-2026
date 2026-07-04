import { useState, useContext } from "react";
import { useNavigate, Link } from "react-router-dom";
import { ToastContext } from "../context/ToastContext";
import { api } from "../services/api";
import "./Login.css";

const Register = () => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [occupation, setOccupation] = useState("");
  const [birthday, setBirthday] = useState("");
  const [emailNotifications, setEmailNotifications] = useState(true);

  const { addToast } = useContext(ToastContext);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await api.auth.register({
        username: username,
        email: email,
        password: password,
        occupation: occupation,
        birthday: birthday,
        emailNotifications: emailNotifications, // Wysyłamy decyzję
      });

      if (response.ok) {
        addToast("Konto utworzone! Sprawdź kod weryfikacyjny.", "success");
        navigate("/verify", { state: { savedUsername: username } });
      } else {
        const errorData = await response.text();
        addToast(`Błąd rejestracji: ${errorData || "Sprawdź dane"}`, "error");
      }
    } catch (error) {
      console.error("Błąd podczas rejestracji:", error);
      addToast("Błąd połączenia z serwerem.", "error");
    }
  };

  return (
    <div className="login-container">
      <div className="login-box">
        <h2>Zarejestruj się</h2>
        <form onSubmit={handleSubmit} className="login-form">
          <div className="form-group">
            <label>Nazwa użytkownika (Login): </label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="np. Kredek123"
              required
            />
          </div>

          <div className="form-group">
            <label>Adres E-mail: </label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="name@example.com"
              required
            />
          </div>

          <div className="form-group">
            <label>Rola / Zawód: </label>
            <input
              type="text"
              value={occupation}
              onChange={(e) => setOccupation(e.target.value)}
              placeholder="np. Student"
              required
            />
          </div>

          <div className="form-group">
            <label>Data urodzin: </label>
            <input
              type="date"
              value={birthday}
              onChange={(e) => setBirthday(e.target.value)}
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

          <div
            className="form-group"
            style={{
              display: "flex",
              alignItems: "center",
              gap: "10px",
              marginTop: "10px",
              marginBottom: "15px",
            }}
          >
            <input
              type="checkbox"
              id="regEmailNotif"
              checked={emailNotifications}
              onChange={(e) => setEmailNotifications(e.target.checked)}
              style={{ width: "auto", margin: 0, cursor: "pointer" }}
            />
            <label
              htmlFor="regEmailNotif"
              style={{
                margin: 0,
                fontSize: "14px",
                fontWeight: "normal",
                cursor: "pointer",
              }}
            >
              Zgadzam się na powiadomienia e-mail
            </label>
          </div>

          <button type="submit" className="login-btn">
            Utwórz konto
          </button>
        </form>

        <p style={{ marginTop: "15px", textAlign: "center" }}>
          Masz już konto? <Link to="/login">Zaloguj się</Link>
        </p>
      </div>
    </div>
  );
};

export default Register;
