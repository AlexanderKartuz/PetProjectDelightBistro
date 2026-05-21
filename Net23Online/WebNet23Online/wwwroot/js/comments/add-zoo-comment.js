document.getElementById('comment-form').addEventListener('submit', async function (e) {
    e.preventDefault();

    const commentsType = document.getElementById('form-comments-type').value;
    const entityId = document.getElementById('form-entity-id').value;
    const commentText = document.getElementById('form-comment-text').value;

    if (!commentText.trim()) return;

    // Формируем данные формы для отправки
    const formData = new URLSearchParams();
    formData.append('CommentsType', commentsType);
    formData.append('EntityId', entityId);
    formData.append('NewCommentText', commentText);

    try {
        // Отправляем запрос на универсальный эндпоинт комментариев
        const response = await fetch('/Comments/AddComment', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: formData.toString()
        });

        if (response.ok) {
            const result = await response.json();

            // Рендерим новый комментарий в вашем зеленом стиле
            const newCommentHtml = `
                        <div class="animal comment-item-box" style="background: white; border-left: 5px solid #2e7d32; box-shadow: 0 2px 5px rgba(0,0,0,0.05); opacity: 0; transform: translateY(-10px); transition: all 0.3s ease;">
                            <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 8px;">
                                <strong style="color: #2e7d32; font-size: 18px;">${result.author}</strong>
                                <span style="font-size: 14px; color: #666;">${result.createdAt}</span>
                            </div>
                            <p style="margin: 0; font-size: 16px; color: #333; line-height: 1.5;">${result.text}</p>
                        </div>
                    `;

            const container = document.getElementById('comments-container');
            container.insertAdjacentHTML('afterbegin', newCommentHtml);

            // Запуск плавной анимации появления
            setTimeout(() => {
                const firstChild = container.firstElementChild;
                if (firstChild) {
                    firstChild.style.opacity = '1';
                    firstChild.style.transform = 'translateY(0)';
                }
            }, 50);

            // Скрываем заглушку "нет отзывов" и очищаем поле
            document.getElementById('empty-note').style.display = 'none';
            document.getElementById('form-comment-text').value = '';
        } else {
            alert('Не удалось сохранить комментарий. Проверьте подключение.');
        }
    } catch (error) {
        console.error('Ошибка AJAX:', error);
        alert('Произошла непредвиденная ошибка на клиенте.');
    }
});