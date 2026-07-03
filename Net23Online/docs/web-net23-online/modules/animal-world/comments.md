# Comments (AnimalWorld)

> Комментарии к зоопаркам — просмотр и добавление отзывов.

**Контроллер:** `CommentsController`  
**Родительский модуль:** [AnimalWorld](README.md)  
**Точка входа:** `/Comments/ZooCommentsIndex?zooId=`

---

## Назначение

Спутник AnimalWorld: пользователи могут читать и оставлять комментарии к конкретному зоопарку. Точка входа — кнопка «Отзывы» на странице `AnimalWorld/Zoos`.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/Comments/ZooCommentsIndex?zooId=` | `ZooCommentsIndex` | `ZooCommentsIndex.cshtml` | — |

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| POST | `/api/Comments/AddComment` | Form: `EntityId`, `NewCommentText` | `[Authorize]` |

---

## SignalR

Нет.

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `ICommentsService` / `CommentsService` | `GetZooComments`, `AddZooComment` |
| `ICommentsMapper` / `CommentMapper` | Маппинг |
| `ICommentsRepository` | Data access |

---

## Модель данных

| Сущность | Поля |
|----------|------|
| `CommentData` | `AuthorId`, `Text`, `CreatedAt`, `CommentType` (`EntityType.Zoo`), `ZooId` |

---

## Frontend

- **View:** `Views/Comments/ZooCommentsIndex.cshtml`
- **JS:** `wwwroot/js/comments/add-zoo-comment.js`

---

## Локализация

Нет отдельных `.resx`.

---

## Источники в коде

- `Controllers/CommentsController.cs`
- `Controllers/ApiControllers/CommentsController.cs`
- `Services/CommentsService.cs`, `CommentMapper.cs`
- `Views/Comments/ZooCommentsIndex.cshtml`
