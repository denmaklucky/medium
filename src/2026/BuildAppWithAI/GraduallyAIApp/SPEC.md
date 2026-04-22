# ToDo-List App — Specification

## Overview

A production-grade ToDo-List web application built with ASP.NET Razor Pages and Hydro interactive components. Single-user with authentication, backed by SQLite.

## Tech Stack

| Layer         | Choice                                      |
|---------------|---------------------------------------------|
| Framework     | ASP.NET Razor Pages (latest .NET)           |
| Interactivity | Hydro components (server-rendered, no SPA)  |
| Database      | SQLite via Entity Framework Core            |
| Auth          | ASP.NET Identity (cookie-based)             |
| Styling       | Bootstrap                                   |
| Deployment    | TBD                                         |

## Data Model

### Todo

| Field       | Type       | Notes                                      |
|-------------|------------|---------------------------------------------|
| Id          | int (PK)   | Auto-increment                              |
| Title       | string     | No max length constraint                    |
| IsCompleted | bool       | Default: false                              |
| CreatedAt   | DateTime   | UTC, set on creation, used for ordering     |
| UserId      | string (FK)| Links to ASP.NET Identity user              |

### User

Standard ASP.NET Identity `IdentityUser`. No additional profile fields.

All todo queries are scoped by `UserId` — a single shared SQLite database with user-level isolation.

## Authentication

### Scope

- **Register**: Email + password. No email confirmation required.
- **Login**: Email + password, cookie-based session.
- **Logout**: Clear session cookie.
- **No password reset** in initial version.
- **No external OAuth providers** in initial version.

### Security

- All todo endpoints require authentication (`[Authorize]`).
- Unauthenticated users redirect to login page.
- Anti-forgery tokens on all forms.
- Password requirements: ASP.NET Identity defaults.

## Features & UX

### Todo List (Main View)

- **Default view**: Shows only **active** (not completed) todos.
- **Ordering**: Newest first (by `CreatedAt` descending).
- **Filter**: Toggle between "Active" and "Completed" views. Filter state is component-level (Hydro) — not reflected in the URL, resets on page refresh.

### Adding a Todo

- Always-visible **inline text input at the top** of the list.
- Submit by pressing **Enter**.
- **Empty/whitespace-only input is silently ignored** — no error message, no submission, input stays focused.
- New todo appears at the top of the active list immediately.

### Completing a Todo

- **Checkbox** next to each todo.
- Clicking the checkbox marks the todo as completed.
- Completed todo **disappears from the active view** immediately.
- Visible when user switches to the "Completed" filter.

### Editing a Todo

- **Click on the todo title** to enter inline edit mode.
- Title becomes an editable text input.
- **Enter** saves the change.
- **Escape** cancels and reverts to the original title.
- Same empty/whitespace validation as adding — silently revert if edited to empty.

### Deleting a Todo

- Delete button/icon on each todo item.
- **Confirmation prompt** before permanent deletion (browser `confirm()` dialog is acceptable).
- Deletion is **permanent** — no soft delete, no trash, no undo.
- **No bulk "clear completed" action**.

### Error Handling

- **Optimistic UI updates** — the UI updates immediately on user action without waiting for server confirmation.
- **No rollback on server failure** — if the server request fails silently, the UI may be out of sync. Acceptable tradeoff for simplicity.

## Pages / Routes

| Route             | Purpose                     | Auth Required |
|-------------------|-----------------------------|---------------|
| `/`               | Redirect to `/todos`        | Yes           |
| `/todos`          | Main todo list (Hydro)      | Yes           |
| `/Identity/Account/Login`    | Login page       | No            |
| `/Identity/Account/Register` | Registration page | No            |
| `/Identity/Account/Logout`   | Logout action     | Yes           |

## Visual Design

- **Bootstrap** for layout, components, and responsive design.
- Clean, functional appearance — not heavily customized.
- Semantic HTML: proper `<label>`, `<button>`, heading hierarchy.
- Basic accessibility: semantic elements, visible focus states (Bootstrap defaults).
- **No dark mode** in initial version.

## Non-Functional Requirements

- **Performance**: SQLite is single-writer; acceptable for expected single-user load. No caching layer needed.
- **Concurrency**: Single SQLite file with EF Core. Use `WAL` journal mode for better read concurrency.
- **Data safety**: SQLite file stored on local disk. No automated backups in initial version.
- **Browser support**: Modern evergreen browsers (Chrome, Firefox, Edge, Safari latest 2 versions).

## Out of Scope (Initial Version)

- Email confirmation / password reset
- OAuth / external login providers
- Due dates, priorities, tags, notes, or subtasks
- Multiple lists or projects
- Drag-and-drop reordering
- Dark mode
- Bulk operations (clear completed, select multiple)
- Real-time sync / push notifications
- API endpoints (REST/GraphQL)
- Offline support / PWA
- Automated backups
- Mobile-specific optimizations
