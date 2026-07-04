import { createContext, useState, useEffect, useContext } from "react";
import { api } from "../services/api";
import { AuthContext } from "./AuthContext";
import { ToastContext } from "./ToastContext";

export const PlansContext = createContext();

export const PlansProvider = ({ children }) => {
  const { token } = useContext(AuthContext);
  const { addToast } = useContext(ToastContext);
  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(false);

  const fetchPlans = async () => {
    try {
      const response = await api.plans.getFull();

      if (response.status === 400) {
        setPlans([]);
        return;
      }

      if (response.ok) {
        const data = await response.json();
        setPlans(data);
      } else {
        // Inne, prawdziwe błędy
        console.error("Wystąpił inny błąd podczas pobierania planów");
      }
    } catch (error) {
      console.error("Błąd połączenia z serwerem:", error);
    }
  };

  useEffect(() => {
    fetchPlans();
  }, [token]);

  const addPlan = async (newPlan) => {
    const tempPlan = { ...newPlan, id: Date.now() };
    setPlans((prev) => [...prev, tempPlan]);

    try {
      const payload = {
        title: newPlan.title,
        category: newPlan.category,
        startTime: newPlan.date.replace(" ", "T"),
        description: "",
      };

      const response = await api.plans.create(payload);

      if (!response.ok) {
        throw new Error("Błąd API podczas tworzenia planu");
      }

      fetchPlans();
    } catch (error) {
      console.error("Błąd zapisu planu:", error);
      setPlans((prev) => prev.filter((p) => p.id !== tempPlan.id));
      addToast("Błąd serwera. Nie udało się zapisać planu.", "error");
    }
  };

  const updatePlan = async (planId, updatedData) => {
    const previousPlans = [...plans];

    setPlans(
      plans.map((plan) =>
        plan.id === planId ? { ...plan, ...updatedData } : plan,
      ),
    );

    try {
      const payload = {
        id: planId,
        title: updatedData.title,
        category: updatedData.category,
        startTime: updatedData.date ? updatedData.date.replace(" ", "T") : "",
        description: updatedData.description || "",
      };

      const response = await api.plans.edit(payload);

      if (!response.ok) {
        throw new Error("Błąd API podczas edycji planu");
      }
      addToast("Plan został zaktualizowany!", "success");
    } catch (error) {
      console.error("Błąd edycji planu:", error);
      setPlans(previousPlans);
      addToast("Błąd serwera. Nie udało się zaktualizować planu.", "error");
    }
  };

  const deletePlan = async (planId) => {
    const previousPlans = [...plans];
    setPlans(plans.filter((plan) => plan.id !== planId));

    try {
      const response = await api.plans.delete(planId);

      if (!response.ok) {
        throw new Error("Błąd API podczas usuwania planu");
      }
      addToast("Plan został usunięty!", "success");
    } catch (error) {
      console.error("Błąd usuwania planu:", error);
      setPlans(previousPlans);
      addToast("Błąd serwera. Nie udało się usunąć planu.", "error");
    }
  };

  return (
    <PlansContext.Provider
      value={{ plans, loading, addPlan, deletePlan, updatePlan, fetchPlans }}
    >
      {children}
    </PlansContext.Provider>
  );
};
