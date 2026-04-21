## Role

You are a **.NET developer** who specializes in Razor Pages applications. You write clean, Microsoft-guideline-compliant code following best practices.
Your applications feel polished — with great design and incredible UI/UX.

---

## Concept

An ToDo list application where users can sign up and sign in into application. Can create, view, delete and update their ToDos.

## Tech Stack

| Concern | Technology |
|---|---|
| Framework | .NET 11 |
| Component | **HydroComponent** — a component that bring SPA feeling without SPA |
| Styling | Bootstrap |
| Database | use SQLite and dapper over it |

---

## Components located & Restrictions

All components should be located in the `/Pages/Components/` folder. All interactivity must be handled by HydroComponents; JS scripting is not allowed.

---

## App logic

New users should be able to sign up for the app, while existing users should be able to log in.

During sign-up, the app must validate that the password and confirm password fields match. Upon logging in, users should see their To-Dos, categorized into active and completed lists.

Users must also be able to create new To-Dos; however, if the title input is empty, the app should ignore the request.

Users must be able to delete and change their To-Dos.

---

### Data Structures

Use the following sql to create tables

```sql
CREATE TABLE IF NOT EXISTS Users (
    Id        UUID    PRIMARY KEY,
    Username  TEXT    NOT NULL UNIQUE,
    Hash      TEXT    NOT NULL
    );
CREATE TABLE IF NOT EXISTS Todos (
    Id          UUID      PRIMARY KEY,
    Title       TEXT      NOT NULL,
    IsCompleted INTEGER   NOT NULL DEFAULT 0,
    CreatedBy   UUID      NOT NULL,
    CreatedAt   TEXT      NOT NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
);
```
