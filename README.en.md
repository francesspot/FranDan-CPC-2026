🇵🇱 [Polski](README.md) | 🇬🇧 **English**

# 🎂 FranDan — Birthdays & Plans with Friends

![C#](https://img.shields.io/badge/C%23-512BD4?logo=dotnet&logoColor=white) ![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core_8-512BD4?logo=dotnet&logoColor=white) ![React](https://img.shields.io/badge/React-20232A?logo=react&logoColor=61DAFB) ![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white)

> 🎓 Project from the **CPC 2026** web development course run by the **KREDEK** student research group (Wrocław University of Science and Technology)

A web app that reminds you of **your friends' birthdays**. It shows whose birthday comes next and in how many days, and also lets you plan events together.

## ✨ Features

- **Upcoming birthdays** — friends sorted by days left until their birthday; on the user's birthday the app shows a birthday banner
- **Friends** — send, accept, reject and remove friend requests
- **Plans** — events with a category (sport, study, sightseeing, party…), date and description that you invite friends to; participant and admin roles
- **Accounts** — sign-up with a verification code sent by email, **JWT** login, passwords hashed with **BCrypt**
- **Email notifications** about invitations (can be turned off)

## 🛠️ Tech

| Part | Technologies |
|---|---|
| Backend | ASP.NET Core 8 Web API, Entity Framework Core, SQLite, JWT, BCrypt, SMTP, Swagger |
| Frontend | React, Vite, React Router, Context API |

The backend is split into controllers, services, models and DTOs.

## 🚀 Run it

**Backend** — in `Backend/FranDanBackend/` create `appsettings.json` with a `JwtSecretKey` and `config.json` with the Gmail account used for sending emails (`email`, `password` — an app password), then:

```bash
dotnet run
```

The SQLite database is created automatically; Swagger is at `/swagger`.

**Frontend** (expects the API at `https://localhost:7074`):

```bash
cd Frontend
npm install
npm run dev
```

The interface is in Polish.
