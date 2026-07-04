import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { Navigate, Link } from "react-router-dom";
import "./Home.css";

const Home = () => {
  const { token, user } = useContext(AuthContext);

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  if (!user) {
    return (
      <div
        className="home-container"
        style={{ textAlign: "center", paddingTop: "50px" }}
      >
        <div className="spinner"></div>
        <p style={{ color: "#868e96", marginTop: "10px" }}>
          Ładowanie kokpitu...
        </p>
      </div>
    );
  }

  const checkIsBirthday = (dateString) => {
    if (!dateString) return false;
    const datePart = dateString.includes("T")
      ? dateString.split("T")[0]
      : dateString;
    const [, month, day] = datePart.split("-");
    const today = new Date();
    const currentMonth = String(today.getMonth() + 1).padStart(2, "0");
    const currentDay = String(today.getDate()).padStart(2, "0");
    return month === currentMonth && day === currentDay;
  };

  const isBirthdayToday = checkIsBirthday(user.birthday);

  const myPlans = user.plans || [];
  const upcomingPlans = myPlans
    .filter((plan) => {
      const validDateString = plan.startTime
        .replace(" ", "T")
        .replace(/\./g, ":");
      return new Date(validDateString) > new Date();
    })
    .sort((a, b) => {
      const dateA = new Date(a.startTime.replace(" ", "T").replace(/\./g, ":"));
      const dateB = new Date(b.startTime.replace(" ", "T").replace(/\./g, ":"));
      return dateA - dateB;
    })
    .slice(0, 3);

  const myFriends = user.friends || [];
  const upcomingBirthdays = [...myFriends]
    .filter(
      (friend) =>
        friend.days_to_birthday !== null &&
        friend.days_to_birthday !== undefined,
    )
    .sort((a, b) => a.days_to_birthday - b.days_to_birthday)
    .slice(0, 3);
  return (
    <div className="home-container">
      {isBirthdayToday && (
        <div className="birthday-banner">
          🎉 Wszystkiego najlepszego, {user.username.split(" ")[0]}! Spełnienia
          marzeń i świetnej zabawy! 🎁
        </div>
      )}

      <h2 className="section-title">Twoje Najbliższe Plany 📅</h2>
      {upcomingPlans.length > 0 ? (
        <div className="cards-grid">
          {upcomingPlans.map((plan) => (
            <div key={plan.id} className="dashboard-card plan-card">
              <h3>{plan.title}</h3>
              <p className="date-text">
                <strong>Kiedy:</strong> {plan.startTime.replace("T", " ")}
              </p>
              <p className="category-text">
                <strong>Kategoria:</strong> {plan.category}
              </p>
              <div
                className="participants-text"
                style={{ marginTop: "10px", fontSize: "14px" }}
              >
                <strong>Twoja rola:</strong>{" "}
                {plan.creator
                  ? "Twórca"
                  : plan.admin
                    ? "Administrator"
                    : "Uczestnik"}
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="empty-state">
          <p>Nie masz jeszcze żadnych nadchodzących planów.</p>
          <Link to="/add-plan" className="empty-state-link">
            Stwórz nowy plan
          </Link>
        </div>
      )}

      <h2 className="section-title" style={{ marginTop: "40px" }}>
        Nadchodzące Urodziny Znajomych 🎂
      </h2>
      {upcomingBirthdays.length > 0 ? (
        <div className="cards-grid">
          {upcomingBirthdays.map((friend) => (
            <div key={friend.id} className="dashboard-card birthday-card">
              <h3>{friend.username}</h3>
              <p className="date-text">
                <strong>Data urodzin:</strong>{" "}
                {friend.birthday
                  ? friend.birthday.split("T")[0]
                  : "Brak danych"}
              </p>
              <div className="countdown">
                {friend.days_to_birthday === 0 ? (
                  <strong>🎉 To dzisiaj!</strong>
                ) : (
                  <>
                    Za <strong>{friend.days_to_birthday}</strong> dni
                  </>
                )}
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div className="empty-state">
          <p>Brak nadchodzących urodzin w kalendarzu.</p>
          <Link to="/friends" className="empty-state-link">
            Przejdź do znajomych
          </Link>
        </div>
      )}
    </div>
  );
};

export default Home;
