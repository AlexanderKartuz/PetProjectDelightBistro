# Кастомные атрибуты авторизации

Атрибуты из `Controllers/CustomAuthAttribute/`. При отказе — redirect на `/Auth/Deny`.

**UserRole enum:** `User=1`, `Employee=9`, `Moderator=10`, `Admin=99`.

---

## MVC-атрибуты

| Атрибут | Проверка | Модули |
|---------|----------|--------|
| `IsAdminAttribute` | `UserRole.Admin` | Notification, User (`DeleteUser`) |
| `IsModeratorAttribute` | `AtLeastModerator()` | DelightBistro (создание), User (`Index`) |
| `IsEmployeeAttribute` | `IsCurrentUserAtLeastEmployee()` | DelightBistro (`AllFoodItems`, удаление) |

---

## Источники в коде

- `Controllers/CustomAuthAttribute/`
- `WebNet23Online.Data/Enums/UserRole.cs`
