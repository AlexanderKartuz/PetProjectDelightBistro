document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('comment-form');
    if (!form) {
        return;
    }

    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        const entityId = document.getElementById('form-entity-id').value;
        const commentText = document.getElementById('form-comment-text').value;

        if (!commentText.trim()) {
            return;
        }

        const formData = new URLSearchParams();
        formData.append('EntityId', entityId);
        formData.append('NewCommentText', commentText);

        try {
            const response = await fetch('/api/Comments/AddComment', {
                method: 'POST',
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: formData.toString()
            });

            if (response.ok) {
                const result = await response.json();
                const escapedText = result.text
                    .replace(/&/g, '&amp;')
                    .replace(/</g, '&lt;')
                    .replace(/>/g, '&gt;');

                const newCommentHtml = `
                    <div class="animal comment-item-box comment-item-new">
                        <div class="comment-item-header">
                            <strong class="comment-author">${result.author}</strong>
                            <span class="comment-date">${result.createdAt}</span>
                        </div>
                        <p class="comment-text">${escapedText}</p>
                    </div>`;

                const container = document.getElementById('comments-container');
                container.insertAdjacentHTML('afterbegin', newCommentHtml);

                const emptyNote = document.getElementById('empty-note');
                if (emptyNote) {
                    emptyNote.style.display = 'none';
                }

                document.getElementById('form-comment-text').value = '';
            } else {
                alert('Не удалось сохранить комментарий. Проверьте подключение.');
            }
        } catch (error) {
            console.error('Ошибка AJAX:', error);
            alert('Произошла непредвиденная ошибка на клиенте.');
        }
    });
});
