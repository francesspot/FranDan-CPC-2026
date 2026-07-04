import { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { ToastContext } from "../context/ToastContext";
import { Navigate } from "react-router-dom";
import { api } from "../services/api";
import "./Friends.css";

const Friends = () => {
  const { token, user, fetchUserData } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);
  const [inviteValue, setInviteValue] = useState("");

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  const friendsList = user?.friends || [];
  const invitationsList = user?.friendInvitations || [];

  const handleInvite = async (e) => {
    e.preventDefault();
    if (!inviteValue.trim()) return;

    try {
      const response = await api.friends.invite(inviteValue);
      if (response.ok) {
        addToast("Wysłano zaproszenie do znajomych!", "success");
        setInviteValue("");
        await fetchUserData();
      } else {
        addToast("Nie udało się wysłać zaproszenia. Sprawdź login.", "error");
      }
    } catch (error) {
      addToast("Błąd serwera podczas wysyłania zaproszenia.", "error");
    }
  };

  const handleAccept = async (id) => {
    try {
      const response = await api.friends.accept(id);
      if (response.ok) {
        addToast("Zaproszenie zostało zaakceptowane!", "success");
        await fetchUserData();
      }
    } catch (error) {
      addToast("Błąd akceptacji zaproszenia.", "error");
    }
  };

  const handleReject = async (id) => {
    try {
      const response = await api.friends.reject(id);
      if (response.ok) {
        addToast("Zaproszenie zostało odrzucone.", "info");
        await fetchUserData();
      }
    } catch (error) {
      addToast("Błąd odrzucania zaproszenia.", "error");
    }
  };

  const handleRemove = async (id) => {
    if (!window.confirm("Na pewno chcesz usunąć tego znajomego?")) return;

    try {
      const response = await api.friends.delete(id);
      if (response.ok) {
        addToast("Znajomy został usunięty.", "info");
        await fetchUserData();
      }
    } catch (error) {
      addToast("Błąd usuwania znajomego.", "error");
    }
  };

  if (!user) {
    return (
      <div
        className="friends-container"
        style={{ textAlign: "center", paddingTop: "50px" }}
      >
        <div className="spinner"></div>
        <p style={{ color: "#868e96", marginTop: "10px" }}>
          Ładowanie danych...
        </p>
      </div>
    );
  }

  return (
    <div className="friends-container">
      <h2>Znajomi</h2>

      <div className="search-box">
        <form onSubmit={handleInvite} style={{ display: "flex", gap: "10px" }}>
          <input
            type="text"
            placeholder="Wpisz login lub e-mail znajomego..."
            value={inviteValue}
            onChange={(e) => setInviteValue(e.target.value)}
          />
          <button type="submit" className="btn-invite">
            Zaproś
          </button>
        </form>
      </div>

      {invitationsList.length > 0 && (
        <div style={{ marginBottom: "40px" }}>
          <h3 style={{ borderBottom: "2px solid #eee", paddingBottom: "10px" }}>
            Oczekujące zaproszenia ({invitationsList.length})
          </h3>
          <div className="friends-grid">
            {invitationsList.map((person) => (
              <div key={person.id} className="friend-card">
                <div className="friend-info">
                  <div className="friend-avatar">
                    {person.username
                      ? person.username.charAt(0).toUpperCase()
                      : "?"}
                  </div>
                  <div>
                    <h3>{person.username}</h3>
                    <p>{person.occupation || "Brak roli"}</p>
                  </div>
                </div>
                <div className="friend-actions">
                  <button
                    className="btn-accept"
                    onClick={() => handleAccept(person.id)}
                    title="Akceptuj"
                  >
                    ✔️
                  </button>
                  <button
                    className="btn-remove"
                    onClick={() => handleReject(person.id)}
                    title="Odrzuć"
                  >
                    ❌
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      <div>
        <h3 style={{ borderBottom: "2px solid #eee", paddingBottom: "10px" }}>
          Moi Znajomi ({friendsList.length})
        </h3>

        {friendsList.length > 0 ? (
          <div className="friends-grid">
            {friendsList.map((person) => (
              <div key={person.id} className="friend-card">
                <div className="friend-info">
                  <div className="friend-avatar">
                    {person.username
                      ? person.username.charAt(0).toUpperCase()
                      : "?"}
                  </div>
                  <div>
                    <h3>{person.username}</h3>
                    <p>{person.occupation || "Brak roli"}</p>
                  </div>
                </div>
                <div className="friend-actions">
                  <span className="status-badge accepted">Znajomy</span>
                  <button
                    className="btn-remove"
                    onClick={() => handleRemove(person.id)}
                    title="Usuń"
                  >
                    ❌
                  </button>
                </div>
              </div>
            ))}
          </div>
        ) : (
          <p className="no-friends-message">
            Nie masz jeszcze żadnych znajomych. Zaproś kogoś!
          </p>
        )}
      </div>
    </div>
  );
};

export default Friends;
