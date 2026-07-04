import { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { ToastContext } from "../context/ToastContext";
import { Navigate } from "react-router-dom";
import { api } from "../services/api";
import "./PlansList.css";

const PlansList = () => {
  const { token, user, fetchUserData } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);

  const [editingPlanId, setEditingPlanId] = useState(null);
  const [editFormData, setEditFormData] = useState({
    title: "",
    startTime: "",
    category: "",
    description: "",
  });

  const [currentParticipants, setCurrentParticipants] = useState([]);

  if (!token) return <Navigate to="/login" replace />;

  if (!user) {
    return (
      <div
        className="plans-container"
        style={{ textAlign: "center", paddingTop: "50px" }}
      >
        <div className="spinner"></div>
        <p style={{ color: "#868e96", marginTop: "10px" }}>
          Ładowanie harmonogramu...
        </p>
      </div>
    );
  }

  const myPlans = user.plans || [];
  const invitations = user.planInvitations || [];
  const myFriends = user.friends || [];

  const loadPlanParticipants = async (planId) => {
    try {
      const response = await api.plans.getFull(planId);
      if (response.ok) {
        const fullPlanData = await response.json();
        setCurrentParticipants(fullPlanData.participants || []);
      }
    } catch (error) {
      console.error("Błąd pobierania uczestników planu:", error);
    }
  };

  const handleEditClick = async (plan) => {
    setEditingPlanId(plan.id);
    setEditFormData({
      title: plan.title,
      startTime: plan.startTime ? plan.startTime.replace(" ", "T") : "",
      category: plan.category,
      description: plan.description || "",
    });
    await loadPlanParticipants(plan.id);
  };

  const handleAddParticipant = async (friendId) => {
    try {
      const response = await api.participation.add({
        planId: editingPlanId,
        userId: friendId,
        admin: false,
      });
      if (response.ok) {
        addToast("Zaproszono znajomego!", "success");
        await loadPlanParticipants(editingPlanId);
      }
    } catch (error) {
      addToast("Błąd dodawania uczestnika.", "error");
    }
  };

  const handleRemoveParticipant = async (participantUserId) => {
    try {
      const response = await api.participation.remove({
        planId: editingPlanId,
        userId: participantUserId,
      });
      if (response.ok) {
        addToast("Uczestnik usunięty.", "info");
        await loadPlanParticipants(editingPlanId);
      }
    } catch (error) {
      addToast("Błąd podczas usuwania.", "error");
    }
  };

  const handleToggleAdmin = async (participantUserId, currentAdminStatus) => {
    try {
      const response = await api.participation.setAdmin({
        planId: editingPlanId,
        userId: participantUserId,
        admin: !currentAdminStatus,
      });
      if (response.ok) {
        await loadPlanParticipants(editingPlanId);
      }
    } catch (error) {
      addToast("Błąd uprawnień.", "error");
    }
  };

  const handleAcceptInvite = async (planId) => {
    if ((await api.participation.accept({ id: planId })).ok) {
      addToast("Dołączyłeś!", "success");
      await fetchUserData();
    }
  };

  const handleRejectInvite = async (planId) => {
    if ((await api.participation.reject({ id: planId })).ok) {
      addToast("Odrzucono.", "info");
      await fetchUserData();
    }
  };

  const handleDelete = async (planId) => {
    if (window.confirm("Usunąć ten plan?")) {
      if ((await api.plans.delete(planId)).ok) {
        addToast("Plan usunięty.", "info");
        await fetchUserData();
      }
    }
  };

  const handleSaveEdit = async (planId) => {
    if ((await api.plans.edit({ id: planId, ...editFormData })).ok) {
      addToast("Zapisano!", "success");
      setEditingPlanId(null);
      await fetchUserData();
    } else {
      addToast("Błąd zapisu.", "error");
    }
  };

  const friendsAvailableToInvite = myFriends.filter(
    (friend) => !currentParticipants.some((p) => p.userId === friend.id),
  );

  return (
    <div className="plans-container">
      <h2>Harmonogram Wydarzeń</h2>

      {invitations.length > 0 && (
        <div
          style={{
            marginBottom: "40px",
            padding: "15px",
            backgroundColor: "#fff3cd",
            borderRadius: "8px",
          }}
        >
          <h3>Nowe zaproszenia ({invitations.length})</h3>
          {invitations.map((invite) => (
            <div key={invite.id} className="plan-item">
              <h3>{invite.title}</h3>
              <p>
                <strong>Kiedy:</strong> {invite.startTime.replace("T", " ")}
              </p>
              <button onClick={() => handleAcceptInvite(invite.id)}>
                ✔️ Akceptuj
              </button>
              <button onClick={() => handleRejectInvite(invite.id)}>
                ❌ Odrzuć
              </button>
            </div>
          ))}
        </div>
      )}

      <h3>Moje Wydarzenia ({myPlans.length})</h3>
      <div className="plans-list">
        {myPlans.map((plan) => (
          <div key={plan.id} className="plan-item">
            {editingPlanId === plan.id ? (
              <div className="edit-mode">
                <input
                  type="text"
                  value={editFormData.title}
                  onChange={(e) =>
                    setEditFormData({ ...editFormData, title: e.target.value })
                  }
                />
                <input
                  type="datetime-local"
                  value={editFormData.startTime}
                  onChange={(e) =>
                    setEditFormData({
                      ...editFormData,
                      startTime: e.target.value.replace("T", " "),
                    })
                  }
                />
                <textarea
                  value={editFormData.description}
                  onChange={(e) =>
                    setEditFormData({
                      ...editFormData,
                      description: e.target.value,
                    })
                  }
                />

                {currentParticipants.map((p) => (
                  <div key={p.userId}>
                    {p.username} {p.admin ? "👑" : "👤"}
                    <button
                      onClick={() => handleToggleAdmin(p.userId, p.admin)}
                    >
                      👑
                    </button>
                    <button onClick={() => handleRemoveParticipant(p.userId)}>
                      ❌
                    </button>
                  </div>
                ))}

                <select
                  onChange={(e) =>
                    e.target.value &&
                    handleAddParticipant(Number(e.target.value))
                  }
                >
                  <option value="">Dodaj znajomego...</option>
                  {friendsAvailableToInvite.map((f) => (
                    <option key={f.id} value={f.id}>
                      {f.username}
                    </option>
                  ))}
                </select>

                <button onClick={() => handleSaveEdit(plan.id)}>Zapisz</button>
                <button onClick={() => setEditingPlanId(null)}>Anuluj</button>
              </div>
            ) : (
              <>
                <h3>
                  {plan.title} {plan.creator && "👑"}
                </h3>
                <p>
                  <strong>Kiedy:</strong> {plan.startTime.replace("T", " ")}
                </p>
                {(plan.creator || plan.admin) && (
                  <>
                    <button onClick={() => handleEditClick(plan)}>
                      Edytuj
                    </button>
                    <button onClick={() => handleDelete(plan.id)}>Usuń</button>
                  </>
                )}
              </>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default PlansList;
