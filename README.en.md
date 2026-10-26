[Polski](README.md) | **English**

# FranDan — CPC 2026

A full-stack web app for **planning meetups with friends**, built as the assignment for the **CPC 2026** web development course run by the **KREDEK** student research group at Wrocław University of Science and Technology.

## Features

- **Accounts** — registration with **email verification** (a code sent by email), login with **JWT**, profile editing and account deletion.
- **Friends** — sending, accepting and rejecting friend requests, removing friends.
- **Plans** — creating events with a title, category (sport, study, sightseeing, party, other), description and date/time, and inviting friends to them.
- **Participation** — accepting or declining an invitation, leaving a plan. Admins can edit the plan and invite more people; only the creator can make someone an admin.
- **Home page** — your upcoming plans and your friends' upcoming birthdays.
- **Email notifications** (optional per user) about friend requests and plan invitations.

The interface is in Polish.

## Tech stack

| Part | Technologies |
|------|--------------|
| Backend | ASP.NET Core 8 Web API, Entity Framework Core + SQLite, JWT Bearer authentication, BCrypt password hashing, SMTP (Gmail), Swagger |
| Frontend | React + Vite, React Router, Context API (auth, friends, plans, toasts) |

## Project structure

```
Backend/FranDanBackend/
├── Controllers/   – auth, user, friend, plan, participation endpoints
├── Services/      – business logic
├── Models/        – User, Plan, Participation, Friendship, Verifier
├── DTO/           – request/response objects
└── Migrations/    – EF Core migrations
Frontend/src/
├── pages/         – Home, Login, Register, Verify, PlansList, AddPlan, Friends, Profile
├── context/       – global state
└── services/api.js
```

## Running locally

### Backend

Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download). Configuration files are not committed, so create them in `Backend/FranDanBackend/`:

`appsettings.json`
```json
{
  "JwtSecretKey": "<random string of at least 32 characters>",
  "ConnectionStrings": { "DefaultConnection": "Data Source=FranDanDB.db" }
}
```

`config.json` — the Gmail account used to send emails (use an [app password](https://support.google.com/accounts/answer/185833)):
```json
{ "email": "your.address@gmail.com", "password": "<app password>" }
```

Then:

```bash
cd Backend/FranDanBackend
dotnet run
```

The SQLite database is created automatically on first start. In development mode Swagger is available at `/swagger`.

### Frontend

Requires Node.js. The frontend expects the API at `https://localhost:7074` (set in `src/services/api.js`), and the backend allows requests from `http://localhost:5173`.

```bash
cd Frontend
npm install
npm run dev
```
