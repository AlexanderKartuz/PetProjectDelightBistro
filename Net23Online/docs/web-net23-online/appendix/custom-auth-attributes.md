# Кастомные атрибуты авторизации

Атрибуты из `Controllers/CustomAuthAttribute/`. Все MVC-атрибуты при отказе redirect на `/Auth/Deny` (кроме указанных).

**UserRole enum:** `User=1`, `RockBandOwner=6`, `Employee=9`, `Moderator=10`, `Admin=99`, `JdmOwner=626`.

---

## Общие (MVC)

| Атрибут | Проверка | Модули |
|---------|----------|--------|
| `IsAdminAttribute` | `UserRole.Admin` | User, JDM, Notification |
| `IsModeratorAttribute` | `AtLeastModerator()` | AnimalWorld, DelightBistro, Steam, HabitTrackerAdmin, RockLegendsPortal |
| `IsEmployeeAttribute` | `IsCurrentUserAtLeastEmployee()` | DelightBistro |
| `IsNotBlockedInTrackerAttribute` | authenticated + not blocked in tracker | HabitTracker |
| `IsJdmOwnerAttribute` | `UserRole.JdmOwner` | JDM |
| `IsRockLegendsModeratorAttribute` | Moderator or Admin | RockLegendsPortal |
| `IsRockBandOwnerAttribute` | `UserRole.RockBandOwner` → `ForbidResult` | RockBands |
| `IsSlayTheSpire2CreatorOrAdminAttribute` | card creator or Admin (by CardId) | SlayTheSpire2 |
| `CanReserveZooVisitAttribute` | FirstName + LastName + Mobilephone filled | Tickets |
| `CanAccessLittleLemonReservationAttribute` | authenticated + Admin or User | LittleLemon |

---

## Steam (API и MVC)

| Атрибут | Поведение при отказе | Использование |
|---------|---------------------|---------------|
| `IsAdminApiAttribute` | 403 JSON | Catalog delete |
| `IsAuthenticatedApiAttribute` | 401 JSON | Не используется |
| `EditForCreatorWithRequiredRoleAttribute` | Redirect Deny | Steam EditGame (Admin or owner + RequiredRole) |
| `DeleteWithRoleAndTimeRestrictionAttribute` | Redirect Deny | Steam DeleteGame (Admin or owner ≤3 days) |

---

## Источники в коде

- `Controllers/CustomAuthAttribute/` — все атрибуты
- `Controllers/CustomAuthAttribute/Steam/` — Steam-specific
- `WebNet23Online.Data/Enums/UserRole.cs`
