# LittleLemon

> Ресторан в стиле Meta Front-End: меню, отзывы, подписка, бронирование столиков, чат с админом.

**Контроллер:** `LittleLemonController`  
**Layout:** `_LayoutLittleLemon.cshtml`  
**Точка входа:** `/LittleLemon/Index`

---

## Назначение

Демо-ресторан с полным UX: главная с меню и отзывами, email-подписка, бронирование столиков с загрузкой фото торта, история бронирований, приватный чат пользователь ↔ админ через SignalR.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/LittleLemon/Index?category=` | `Index` | `Index.cshtml` | — |
| `/LittleLemon/Subscribe` | GET/POST | Redirect Index | — |
| `/LittleLemon/Reservation` | GET/POST | `Reservation.cshtml` | `[CanAccessLittleLemonReservation]` |
| `/LittleLemon/CreateGuest` | POST | — | — |
| `/LittleLemon/LinkReservationToGuest` | POST | — | `[CanAccessLittleLemonReservation]` |
| `/LittleLemon/Confirmation?reservationId=` | `Confirmation` | `Confirmation.cshtml` | Reservation access |
| `/LittleLemon/Chat` | `Chat` | `Chat.cshtml` | Reservation access |
| `/LittleLemon/History` | `History` | `History.cshtml` | Reservation access |
| `/LittleLemon/HistoryPrint` | GET | CSV export | — |

`[CanAccessLittleLemonReservation]` — authenticated + role `Admin` или `User`.

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| GET | `/api/LittleLemonReservation/HasDuplicate?date=&time=&seatingPreference=` | Проверка дубликата | — |
| POST | `/api/LittleLemonChat/SendMessageToAdmin?message=` | Сообщение админу | `[Authorize]` |
| POST | `/api/LittleLemonChat/SendMessageToUser?targetUserId=&message=` | Сообщение пользователю | `[Authorize]` |

---

## SignalR

| Параметр | Значение |
|----------|----------|
| Hub | `LittleLemonHub` |
| Маршрут | `/my-hub/little-lemon` |

**Server → Client:**
- `NewReservationCreated(reservationId, guestName, date, time, guests, seating, occasion, comments, cakePhotoUrl)`
- `ReceivePrivateMessage(senderUserId, senderName, message)`

**OnConnected:** Admin → group `little-lemon-admins`; User → `little-lemon-user-{userId}`.

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `ILittleLemonMenuService` | Hardcoded menu cards |
| `ILittleLemonTestimonialService` | Отзывы |
| `ILittleLemonSubscribeService` | Email-подписка |
| `ILittleLemonReservationService` | CRUD бронирований + hub notify |
| `ILittleLemonChatService` | Приватный чат через groups |
| `FakeRestaurantApi` | `https://fakerestaurantapi.runasp.net/api/Restaurant/5/menu` |

---

## Модель данных

| Сущность | DbSet |
|----------|-------|
| `LittleLemonData` | `LittleLemon` |
| `LittleLemonGuestData` | `LittleLemonGuests` |

FK: `GuestId`, optional `CreatedByUserId` → `UserData`.

---

## Frontend

- **CSS:** `home-page-styles.css`, `reservation.css`, `notifications.css`, `chat.css`
- **JS:** `little-lemon-signalr.js`, `reservation-notification.js`, `reservation.js`, `little-lemon-chat.js`
- **Images:** `wwwroot/images/little-lemon/`
- **Partial:** `_LittleLemonHeroSection.cshtml`

---

## Локализация

| Файл | Языки |
|------|-------|
| `Localizations/LittleLemon.resx` | EN |
| `LittleLemon.Ru.resx` | RU |

~37 ключей: `Layout_*`, `Reservation_*`, `Reservation_DuplicateWarning`.

---

## Фоновые задачи

Нет.

---

## Внешние API-проекты

[LittleLemonMinimalApi](../../../minimal-apis/little-lemon/README.md) (порт 7100) — standalone, интеграции с MVC нет.

---

## Источники в коде

- `Controllers/LittleLemonController.cs`
- `Controllers/ApiControllers/LittleLemonReservationController.cs`, `LittleLemonChatController.cs`
- `Hubs/LittleLemonHub.cs`
- `Services/LittleLemon/`
- `Views/LittleLemon/`
