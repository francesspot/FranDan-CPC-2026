🇵🇱 **Polski** | 🇬🇧 [English](README.en.md)

# 🎂 FranDan — urodziny i plany ze znajomymi

![C#](https://img.shields.io/badge/C%23-512BD4?logo=dotnet&logoColor=white) ![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core_8-512BD4?logo=dotnet&logoColor=white) ![React](https://img.shields.io/badge/React-20232A?logo=react&logoColor=61DAFB) ![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white)

> 🎓 Projekt z kursu programowania webowego **CPC 2026** koła naukowego **KREDEK** (Politechnika Wrocławska)

Aplikacja webowa przypominająca o **urodzinach znajomych**. Pokazuje, czyje urodziny wypadają najwcześniej i za ile dni, a dodatkowo pozwala planować wspólne wydarzenia.

## ✨ Funkcje

- **Nadchodzące urodziny** — znajomi posortowani według tego, ile dni zostało do ich urodzin; w dniu urodzin użytkownika aplikacja wyświetla baner z życzeniami
- **Znajomi** — zaproszenia, akceptowanie, odrzucanie i usuwanie
- **Plany** — wydarzenia z kategorią (sport, nauka, zwiedzanie, impreza…), datą i opisem, na które zapraszasz znajomych; role uczestnika i administratora
- **Konta** — rejestracja z kodem weryfikacyjnym wysyłanym mailem, logowanie przez **JWT**, hasła hashowane **BCryptem**
- **Powiadomienia e-mail** o zaproszeniach (można je wyłączyć)

## 🛠️ Technologie

| Część | Technologie |
|---|---|
| Backend | ASP.NET Core 8 Web API, Entity Framework Core, SQLite, JWT, BCrypt, SMTP, Swagger |
| Frontend | React, Vite, React Router, Context API |

Backend ma podział na kontrolery, serwisy, modele i DTO.

## 🚀 Uruchomienie

**Backend** — w `Backend/FranDanBackend/` utwórz `appsettings.json` z kluczem `JwtSecretKey` oraz `config.json` z danymi konta Gmail do wysyłki maili (`email`, `password` — hasło do aplikacji), a potem:

```bash
dotnet run
```

Baza SQLite utworzy się sama, Swagger jest pod `/swagger`.

**Frontend** (oczekuje API pod `https://localhost:7074`):

```bash
cd Frontend
npm install
npm run dev
```
