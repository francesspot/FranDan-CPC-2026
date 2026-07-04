import { useState, useContext, useEffect } from "react";
import { AuthContext } from "../context/AuthContext";
import { ToastContext } from "../context/ToastContext";
import { useNavigate } from "react-router-dom";
import { api } from "../services/api";
import "./Profile.css";

const Profile = () => {
  const { user, fetchUserData, logout } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);
  const navigate = useNavigate();

  const [username, setUsername] = useState("");
  const [occupation, setOccupation] = useState("");
  const [myBirthday, setMyBirthday] = useState("");
  const [emailNotifications, setEmailNotifications] = useState(false);

  useEffect(() => {
    if (user) {
      setUsername(user.username || "");
      setOccupation(user.occupation || "");

      if (user.birthday) {
        if (user.birthday.includes(".")) {
          const [day, month, year] = user.birthday.split(".");
          setMyBirthday(`${year}-${month}-${day}`);
        } else if (user.birthday.includes("T")) {
          setMyBirthday(user.birthday.split("T")[0]);
        } else {
          setMyBirthday(user.birthday);
        }
      }

      setEmailNotifications(!!user.emailNotifications);
    }
  }, [user]);

  const handleSave = async (e) => {
    e.preventDefault();
    const payload = {
      username,
      occupation,
      birthday: myBirthday,
      emailNotifications,
    };

    try {
      const response = await api.user.update(payload);
      if (response.ok) {
        addToast("Profil zaktualizowany!", "success");
        await fetchUserData();
      } else {
        addToast("Nie udało się zapisać zmian.", "error");
      }
    } catch (error) {
      console.error("Błąd zapisu:", error);
      addToast("Błąd serwera podczas zapisu.", "error");
    }
  };

  const handleDeleteAccount = async () => {
    if (
      window.confirm("UWAGA! Czy na pewno chcesz TRWALE usunąć swoje konto?")
    ) {
      try {
        const response = await api.user.delete();
        if (response.ok) {
          logout();
          addToast("Twoje konto zostało usunięte.", "info");
          navigate("/login");
        } else {
          addToast("Nie udało się usunąć konta.", "error");
        }
      } catch (error) {
        addToast("Błąd serwera przy usuwaniu konta.", "error");
      }
    }
  };

  const checkIsBirthdayToday = () => {
    if (!myBirthday) return false;
    const [, month, day] = myBirthday.split("-");
    const today = new Date();
    const currentMonth = String(today.getMonth() + 1).padStart(2, "0");
    const currentDay = String(today.getDate()).padStart(2, "0");
    return month === currentMonth && day === currentDay;
  };

  if (!user)
    return (
      <p style={{ textAlign: "center", marginTop: "50px" }}>
        Musisz się zalogować.
      </p>
    );

  return (
    <div
      className={`profile-container ${checkIsBirthdayToday() ? "birthday-bg" : ""}`}
    >
      {checkIsBirthdayToday() ? (
        <h2 className="party-title">Czas na imprezę! 🥳</h2>
      ) : (
        <h2>Ustawienia Profilu ⚙️</h2>
      )}

      <div className="profile-card">
        <div className="avatar-placeholder">
          {user.username ? user.username.charAt(0).toUpperCase() : "U"}
        </div>
        <div className="user-details">
          <h3>{user.username || "Użytkownik"}</h3>
          <p className="role-text">Rola: {user.occupation || "Brak"}</p>
        </div>
      </div>

      <form onSubmit={handleSave} className="profile-form">
        <div className="form-group">
          <label>Nazwa użytkownika (Login):</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>

        <div className="form-group">
          <label>Czym się zajmujesz (Zawód/Hobby):</label>
          <input
            type="text"
            value={occupation}
            onChange={(e) => setOccupation(e.target.value)}
            required
          />
        </div>

        <div className="form-group">
          <label>Moja data urodzin:</label>
          <input
            type="date"
            value={myBirthday}
            onChange={(e) => setMyBirthday(e.target.value)}
            required
          />
        </div>

        <div
          className="form-group"
          style={{
            display: "flex",
            alignItems: "center",
            gap: "10px",
            margin: "15px 0",
          }}
        >
          <input
            type="checkbox"
            id="emailNotif"
            checked={user.emailNotifications}
            onChange={(e) => setEmailNotifications(e.target.checked)}
            style={{ width: "auto", margin: 0, cursor: "pointer" }}
          />
          <label
            htmlFor="emailNotif"
            style={{ margin: 0, fontWeight: "normal", cursor: "pointer" }}
          >
            Chcę otrzymywać powiadomienia e-mail
          </label>
        </div>

        <button type="submit" className="save-btn">
          Zapisz zmiany
        </button>
      </form>

      <div
        style={{
          marginTop: "40px",
          borderTop: "1px solid #ffcccc",
          paddingTop: "20px",
        }}
      >
        <h3
          style={{ color: "#dc3545", fontSize: "16px", marginBottom: "10px" }}
        >
          Strefa niebezpieczna
        </h3>
        <button
          onClick={handleDeleteAccount}
          className="btn-delete"
          style={{ width: "100%", padding: "12px" }}
        >
          Usuń konto bezpowrotnie
        </button>
      </div>
    </div>
  );
};

export default Profile;
