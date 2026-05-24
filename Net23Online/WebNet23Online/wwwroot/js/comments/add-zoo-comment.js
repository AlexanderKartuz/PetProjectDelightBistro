$(document).ready(function () {
    const $form = $('#comment-form');
    if (!$form.length) {
        return;
    }

    $form.on('submit', function (e) {
        e.preventDefault();

        const commentText = $.trim($('#form-comment-text').val());
        if (!commentText) {
            return;
        }

        const formData = $form.serialize();
        $.post('/api/Comments/AddComment', formData)
            .done(function (result) {
                const escapedText = $('<div/>').text(result.text).html();
                const newCommentHtml = `
                    <div class="animal comment-item-box comment-item-new" style="display: none;">
                        <div class="comment-item-header">
                            <strong class="comment-author">${result.author}</strong>
                            <span class="comment-date">${result.createdAt}</span>
                        </div>
                        <p class="comment-text">${escapedText}</p>
                    </div>`;
                $(newCommentHtml).prependTo('#comments-container').slideDown(200);
                $('#empty-note').hide();
                $('#form-comment-text').val('');
            })
            // .fail(function (xhr) {
            //     console.error('Ошибка AJAX:', xhr);
            //     alert('Не удалось сохранить комментарий. Проверьте подключение.');
            // });
    });
});