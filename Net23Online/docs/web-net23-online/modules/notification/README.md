# Notification

> Админ-панель для мгновенных и отложенных site-wide уведомлений через SignalR.

**Контроллер:** `NotificationController`  
**Layout:** `_Layout.cshtml`  
**Точка входа:** `/Notification/Index`  
**Авторизация:** `[IsAdmin]` на классе

---

## Назначение

Администратор может отправить уведомление всем пользователям сайта мгновенно или запланировать его на определённое время. Отложенные уведомления обрабатывает фоновый сервис.

---

## Маршруты и страницы

| URL | Action | View / результат | Авторизация |
|-----|--------|------------------|-------------|
| `/Notification/Index` | `Index` GET | `Views/Notification/Index.cshtml` | Admin |
| `/Notification/SendInstantNotification` | POST | SignalR broadcast → redirect Index | Admin |
| `/Notification/SavePreparedNotification` | POST | Сохранение в БД → redirect Index | Admin |

Параметры `SavePreparedNotification`: `text`, `date`, `time`.

---

## Встроенное API

Нет.

---

## SignalR

| Параметр | Значение |
|----------|----------|
| Hub | `NotificationHub` |
| Маршрут | `/my-hub/notification` |
| Server → Client | `NewMessage(string text)` |

Клиент: `wwwroot/js/commonNotification.js` (подключён в `_Layout.cshtml`).

Hub-класс пустой — серверных методов от клиента нет.

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IHubContext<NotificationHub, INotificationHub>` | Мгновенная рассылка |
| `INotificationRepository` | CRUD отложенных уведомлений |
| `IAuthService` | Автор отложенного уведомления |

**Фоновый сервис:** `NotificationBackgroundService` — опрос каждые 30 сек, отправка просроченных уведомлений.

---

## Модель данных

| Сущность | Поля |
|----------|------|
| `NotificationData` | `Text`, `TimeToPublish`, `IsActive`, `Author` |

---

## Frontend

- **View:** `Views/Notification/Index.cshtml` — две POST-формы
- **Глобальный клиент:** `wwwroot/js/commonNotification.js`, `wwwroot/css/site.css` (`.notifications`)

---

## Локализация

Отдельных `.resx` нет. UI на английском.

---

## Фоновые задачи

| Сервис | Интервал | Назначение |
|--------|----------|------------|
| `NotificationBackgroundService` | 30 сек | `GetByLastNotifications()` → broadcast → `IsActive = false` |

---

## Источники в коде

- `Controllers/NotificationController.cs`
- `Hubs/NotificationHub.cs`, `Hubs/Interfaces/INotificationHub.cs`
- `Services/BackgroundServices/NotificationBackgroundService.cs`
- `Views/Notification/Index.cshtml`
- `wwwroot/js/commonNotification.js`
