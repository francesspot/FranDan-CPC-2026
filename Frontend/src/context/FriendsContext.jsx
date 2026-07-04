import { createContext, useState, useContext, useEffect } from "react";
import { api } from "../services/api";
import { AuthContext } from "./AuthContext";
import { ToastContext } from "./ToastContext";

export const FriendsContext = createContext();

export const FriendsProvider = ({ children }) => {
  const { user } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);

  const [allUsers, setAllUsers] = useState([]);
  const [friendStatuses, setFriendStatuses] = useState({});
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (user) {
      const newStatuses = {};

      if (user.friends) {
        user.friends.forEach((f) => {
          newStatuses[f.id] = "accepted";
        });
      }

      if (user.friendInvitations) {
        user.friendInvitations.forEach((f) => {
          newStatuses[f.id] = "pending";
        });
      }

      setFriendStatuses(newStatuses);
    } else {
      setFriendStatuses({});
    }
  }, [user]);

  const inviteFriend = async (friendId) => {
    setFriendStatuses((prev) => ({ ...prev, [friendId]: "pending" }));
    try {
      const response = await api.friends.invite(friendId);
      if (!response.ok) throw new Error("Błąd serwera");
    } catch (error) {
      console.error("Błąd wysyłania zaproszenia:", error);
      setFriendStatuses((prev) => {
        const copy = { ...prev };
        delete copy[friendId];
        return copy;
      });
      addToast("Błąd serwera! Nie udało się wysłać zaproszenia.", "error");
    }
  };

  const acceptFriend = async (friendId) => {
    setFriendStatuses((prev) => ({ ...prev, [friendId]: "accepted" }));
    try {
      const response = await api.friends.accept(friendId);
      if (!response.ok) throw new Error("Błąd serwera");
    } catch (error) {
      console.error("Błąd akceptacji:", error);
      addToast("Błąd serwera. Spróbuj ponownie.", "error");
    }
  };

  const rejectFriend = async (friendId) => {
    setFriendStatuses((prev) => ({ ...prev, [friendId]: "rejected" }));
    try {
      const response = await api.friends.reject(friendId);
      if (!response.ok) throw new Error("Błąd serwera");
    } catch (error) {
      console.error("Błąd odrzucania:", error);
      addToast("Błąd serwera. Spróbuj ponownie.", "error");
    }
  };

  const removeFriend = async (friendId) => {
    setFriendStatuses((prev) => {
      const copy = { ...prev };
      delete copy[friendId];
      return copy;
    });
    try {
      const response = await api.friends.delete(friendId);
      if (!response.ok) throw new Error("Błąd serwera");
    } catch (error) {
      console.error("Błąd usuwania:", error);
      addToast("Błąd serwera. Spróbuj ponownie.", "error");
    }
  };

  return (
    <FriendsContext.Provider
      value={{
        allUsers,
        friendStatuses,
        loading,
        inviteFriend,
        acceptFriend,
        rejectFriend,
        removeFriend,
      }}
    >
      {children}
    </FriendsContext.Provider>
  );
};
