import { useState, useContext, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { ToastContext } from "../context/ToastContext";
import { api } from "../services/api";
import "./Login.css";

const Verify = () => {
  const [usernameOrEmail, setUsernameOrEmail] = useState("");
  const [code, setCode] = useState("");
  const { addToast } = useContext(ToastContext);
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    if (location.state && location.state.savedUsername) {
      setUsernameOrEmail(location.state.savedUsername);
    }
  }, [location]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await api.auth.verify({
        usernameOrEmail: usernameOrEmail,
        code: code,
      });

      if (response.ok) {
        addToast("Konto zweryfikowane! Możesz się teraz zalogować.", "success");
        navigate("/login");
      } else {
        addToast("Błędny kod weryfikacyjny.", "error");
      }
    } catch (error) {
      console.error("Błąd weryfikacji:", error);
      addToast("Błąd serwera. Spróbuj ponownie.", "error");
    }
  };

  return (
    <div className="login-container">
      <div className="login-box">
        <h2>Weryfikacja konta</h2>
        <p
          style={{
            textAlign: "center",
            marginBottom: "20px",
            fontSize: "14px",
            color: "#666",
          }}
        >
          Wpisz kod weryfikacyjny, który otrzymałeś.
        </p>
        <form onSubmit={handleSubmit} className="login-form">
          <div className="form-group">
            <label>E-mail lub Login: </label>
            <input
              type="text"
              value={usernameOrEmail}
              onChange={(e) => setUsernameOrEmail(e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <label>Kod weryfikacyjny: </label>
            <input
              type="text"
              value={code}
              onChange={(e) => setCode(e.target.value)}
              placeholder="np. 123456"
              required
            />
          </div>

          <button type="submit" className="login-btn">
            Potwierdź konto
          </button>
        </form>
      </div>
    </div>
  );
};

export default Verify;
