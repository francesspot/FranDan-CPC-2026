import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { ToastProvider } from "./context/ToastContext";
import Navbar from "./components/Navbar";

import Home from "./pages/Home";
import PlansList from "./pages/PlansList";
import AddPlan from "./pages/AddPlan";
import Profile from "./pages/Profile";
import Login from "./pages/Login";
import Friends from "./pages/Friends";
import Register from "./pages/Register";
import Verify from "./pages/Verify";

function App() {
  return (
    <ToastProvider>
      <AuthProvider>
        <BrowserRouter>
          <Navbar />
          <div style={{ padding: "20px" }}>
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/plans" element={<PlansList />} />
              <Route path="/add-plan" element={<AddPlan />} />
              <Route path="/profile" element={<Profile />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/verify" element={<Verify />} />
              <Route path="/friends" element={<Friends />} />
            </Routes>
          </div>
        </BrowserRouter>
      </AuthProvider>
    </ToastProvider>
  );
}

export default App;
