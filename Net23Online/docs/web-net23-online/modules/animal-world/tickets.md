# Tickets (AnimalWorld)

> Бронирование визитов в зоопарк, просмотр билетов с QR-кодами.

**Контроллер:** `TicketsController`  
**Родительский модуль:** [AnimalWorld](README.md)  
**Точка входа:** `/Tickets/AllMyTickets`  
**Авторизация:** `[Authorize]` на классе

---

## Назначение

Спутник AnimalWorld: авторизованные пользователи бронируют визит в зоопарк и просматривают свои билеты с QR-кодами. Для бронирования требуется заполненный профиль (имя, фамилия, телефон).

---

## Маршруты и страницы

| URL | Action | View / результат | Авторизация |
|-----|--------|------------------|-------------|
| `/Tickets/AllMyTickets` | `AllMyTickets` | `AllMyTickets.cshtml` | Authenticated |
| `/Tickets/ZooReservations` | POST | Redirect | `[CanReserveZooVisit]` |
| `/Tickets/ZooReservationsDenied` | GET | `ZooReservationsDenied.cshtml` | — |

`[CanReserveZooVisit]` — redirect на `ZooReservationsDenied`, если в профиле нет FirstName, LastName, Mobilephone.

---

## Встроенное API

Нет.

---

## SignalR

Нет.

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `ITicketService` / `TicketService` | `BookZoo`, `GetUserZooTickets` |
| `ITicketRepository` | Data access |

---

## Модель данных

| Сущность | Поля |
|----------|------|
| `TicketData` | `UniqueKey`, `PurchaseDate`, `EventDate` (+1 month), `TicketType` (Zoo), `IsUsed`, `UserId`, `ZooId` |

---

## Frontend

- **Views:** `AllMyTickets.cshtml`, `ZooReservations.cshtml`, `ZooReservationsDenied.cshtml`
- **CSS:** `wwwroot/css/tickets/my-tickets.css`
- **JS:** `wwwroot/js/tickets/all-my-tickets.js`, `qrcode.js`

**Точки входа:** `AnimalWorld/Zoos.cshtml` (Book), навигация в `_LayoutAnimalWorld.cshtml` и `_Layout.cshtml`.

---

## Локализация

Нет отдельных `.resx`.

---

## Источники в коде

- `Controllers/TicketsController.cs`
- `Services/TicketService.cs`
- `Controllers/CustomAuthAttribute/CanReserveZooVisitAttribute.cs`
- `Views/Tickets/`
