import { useState, useContext } from "react";
import { useNavigate, Navigate } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import { ToastContext } from "../context/ToastContext";
import { api } from "../services/api";
import "./AddPlan.css";

const AddPlan = () => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [date, setDate] = useState("");
  const [category, setCategory] = useState("inny");

  const { token, user, fetchUserData } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);
  const navigate = useNavigate();

  const [selectedFriendsIds, setSelectedFriendsIds] = useState([]);

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  const myFriends = user?.friends || [];

  const handleCheckboxChange = (friendId) => {
    if (selectedFriendsIds.includes(friendId)) {
      setSelectedFriendsIds(selectedFriendsIds.filter((id) => id !== friendId));
    } else {
      setSelectedFriendsIds([...selectedFriendsIds, friendId]);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!title.trim()) {
      addToast("Tytuł planu nie może być pusty!", "error");
      return;
    }

    const selectedDate = new Date(date);
    const now = new Date();

    if (selectedDate < now) {
      addToast("Nie możesz zaplanować wydarzenia w przeszłości!", "error");
      return;
    }

    const formattedDate = date.replace("T", " ");

    const newPlanDTO = {
      title: title.trim(),
      category: category,
      description: description.trim(),
      startTime: formattedDate,
    };

    try {
      const response = await api.plans.create(newPlanDTO);

      if (response.ok) {
        const userResp = await api.user.getFull();

        if (userResp.ok) {
          const updatedUser = await userResp.json();

          const createdPlan = updatedUser.plans.find(
            (p) =>
              p.title === newPlanDTO.title &&
              p.startTime === newPlanDTO.startTime,
          );

          if (createdPlan && selectedFriendsIds.length > 0) {
            for (const friendId of selectedFriendsIds) {
              await api.participation.add({
                planId: createdPlan.id,
                userId: friendId,
                admin: false,
              });
            }
          }
        }

        await fetchUserData();
        addToast("Plan i zaproszenia zostały pomyślnie wysłane!", "success");
        navigate("/plans");
      } else {
        addToast("Błąd podczas dodawania planu.", "error");
      }
    } catch (error) {
      console.error("Błąd sieci:", error);
      addToast("Błąd serwera przy tworzeniu planu.", "error");
    }
  };

  return (
    <div className="addplan-form-container">
      <h2>Dodaj nowy plan</h2>
      <form onSubmit={handleSubmit} className="plan-form">
        <div className="form-group">
          <label>Tytuł planu:</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>

        <div className="form-group">
          <label>Opis planu:</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            placeholder="Krótki opis wydarzenia..."
            style={{
              width: "100%",
              padding: "10px",
              borderRadius: "5px",
              border: "1px solid #ccc",
            }}
            required
          />
        </div>

        <div className="form-group">
          <label>Data i godzina:</label>
          <input
            type="datetime-local"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Kategoria:</label>
          <select
            value={category}
            onChange={(e) => setCategory(e.target.value)}
          >
            <option value="sport">Sport</option>
            <option value="nauka">Nauka</option>
            <option value="zwiedzanie">Zwiedzanie</option>
            <option value="impreza">Impreza</option>
            <option value="inny">Inny</option>
          </select>
        </div>

        <div className="form-group">
          <label>Zaproś znajomych:</label>
          {myFriends.length > 0 ? (
            <div className="checkbox-friends-list">
              {myFriends.map((friend) => (
                <label key={friend.id} className="checkbox-friend-item">
                  <input
                    type="checkbox"
                    checked={selectedFriendsIds.includes(friend.id)}
                    onChange={() => handleCheckboxChange(friend.id)}
                  />
                  <span>{friend.username}</span>
                </label>
              ))}
            </div>
          ) : (
            <p>Brak dostępnych znajomych do zaproszenia.</p>
          )}
        </div>

        <button type="submit" className="submit-btn">
          Utwórz Plan
        </button>
      </form>
    </div>
  );
};

export default AddPlan;
