import { createContext, useState, useCallback } from "react";
import "./Toast.css";

export const ToastContext = createContext();

export const ToastProvider = ({ children }) => {
  const [toasts, setToasts] = useState([]);

  const addToast = useCallback((message, type = "info") => {
    const id = Date.now();
    setToasts((prevToasts) => [...prevToasts, { id, message, type }]);

    setTimeout(() => {
      setToasts((prev) => prev.filter((toast) => toast.id !== id));
    }, 3000);
  }, []);

  return (
    <ToastContext.Provider value={{ toasts, addToast }}>
      {children}
      <div className="toast-container">
        {toasts.map((toast) => (
          <div key={toast.id} className={`toast toast-${toast.type}`}>
            {toast.type === "success"
              ? "Sukces! "
              : toast.type === "error"
                ? "Błąd! "
                : "Informacja! "}
            {toast.message}
          </div>
        ))}
      </div>
    </ToastContext.Provider>
  );
};
