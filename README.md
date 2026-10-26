**Polski** | [English](README.en.md)

# FranDan — CPC 2026

Aplikacja webowa full-stack do **planowania spotkań ze znajomymi**, wykonana jako zadanie w ramach kursu **CPC 2026** organizowanego przez koło naukowe **KREDEK** Politechniki Wrocławskiej. Kurs dotyczy programowania webowego.

## Funkcje

- **Konta** — rejestracja z **weryfikacją adresu e-mail** (kod wysyłany mailem), logowanie z użyciem **JWT**, edycja profilu i usuwanie konta.
- **Znajomi** — wysyłanie, akceptowanie i odrzucanie zaproszeń, usuwanie znajomych.
- **Plany** — tworzenie wydarzeń z tytułem, kategorią (sport, nauka, zwiedzanie, impreza, inny), opisem oraz datą i godziną, a także zapraszanie na nie znajomych.
- **Uczestnictwo** — przyjęcie lub odrzucenie zaproszenia, wyjście z planu. Administratorzy mogą edytować plan i zapraszać kolejne osoby; tylko twórca może nadać komuś rolę administratora.
- **Strona główna** — Twoje najbliższe plany i nadchodzące urodziny znajomych.
- **Powiadomienia e-mail** (opcjonalne dla każdego użytkownika) o zaproszeniach do znajomych i do planów.

## Technologie

| Część | Technologie |
|-------|-------------|
| Backend | ASP.NET Core 8 Web API, Entity Framework Core + SQLite, uwierzytelnianie JWT Bearer, hashowanie haseł BCrypt, SMTP (Gmail), Swagger |
| Frontend | React + Vite, React Router, Context API (autoryzacja, znajomi, plany, powiadomienia) |

## Struktura projektu

```
Backend/FranDanBackend/
├── Controllers/   – endpointy: auth, user, friend, plan, participation
├── Services/      – logika biznesowa
├── Models/        – User, Plan, Participation, Friendship, Verifier
├── DTO/           – obiekty żądań i odpowiedzi
└── Migrations/    – migracje EF Core
Frontend/src/
├── pages/         – Home, Login, Register, Verify, PlansList, AddPlan, Friends, Profile
├── context/       – stan globalny
└── services/api.js
```

## Uruchomienie lokalne

### Backend

Wymagany [.NET 8 SDK](https://dotnet.microsoft.com/download). Pliki konfiguracyjne nie są w repozytorium, więc utwórz je w `Backend/FranDanBackend/`:

`appsettings.json`
```json
{
  "JwtSecretKey": "<losowy ciąg co najmniej 32 znaków>",
  "ConnectionStrings": { "DefaultConnection": "Data Source=FranDanDB.db" }
}
```

`config.json` — konto Gmail, z którego wysyłane są maile (użyj [hasła do aplikacji](https://support.google.com/accounts/answer/185833)):
```json
{ "email": "twoj.adres@gmail.com", "password": "<hasło do aplikacji>" }
```

Następnie:

```bash
cd Backend/FranDanBackend
dotnet run
```

Baza SQLite tworzy się automatycznie przy pierwszym uruchomieniu. W trybie deweloperskim Swagger jest dostępny pod `/swagger`.

### Frontend

Wymagany Node.js. Frontend oczekuje API pod adresem `https://localhost:7074` (ustawione w `src/services/api.js`), a backend przyjmuje żądania z `http://localhost:5173`.

```bash
cd Frontend
npm install
npm run dev
```
