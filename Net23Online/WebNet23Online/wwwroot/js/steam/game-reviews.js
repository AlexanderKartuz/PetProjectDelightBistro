$(document).ready(function () {
    function showToast(message, type = 'error') {
        const toast = $(`
            <div class="toast toast--${type}">
                ${message}
            </div>
        `);

        $('#toast-container').append(toast);

        setTimeout(() => {
            toast.addClass('show');
        }, 10);

        setTimeout(() => {
            toast.removeClass('show');
        }, 3000);
    };


    $(document).on('click', '#show-review-form-btn', function () {
        $('#show-review-form-btn').hide();
        $('#review-form-container')
            .removeClass('review-form-hidden')
            .addClass('review-form-visible');
    });

    $(document).on('click', '#cancel-review-btn', function () {
        $('#review-form-container')
            .removeClass('review-form-visible')
            .addClass('review-form-hidden');
        $('#show-review-form-btn').show();
        $('#review-form')[0].reset();
    });

    $(document).on('submit', '#review-form', function (e) {
        e.preventDefault();

        const gameId = $(this).data('game-id');
        const text = $('#review-text').val();
        const rating = Number($('#review-rating').val());

        if (!text || !rating) {
            showToast('Fill all fields');
            return;
        }

        $.ajax({
            url: '/api/GameReview/Add',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                gameId: gameId,
                text: text,
                rating: rating
            }),
            success: function (review) {
                $('#reviews-panel').prepend(`
                    <div class="review-item">
                        <b>${review.author}</b> - ${review.rating}/10
                        <div>${review.text}</div>
                    </div>
                `);

                showToast('Review added', 'success');

                $('#review-text').val('');
                $('#review-rating').val('');
                $('#review-form-container')
                    .removeClass('review-form-visible')
                    .addClass('review-form-hidden');
                $('#show-review-form-btn').show();
            },
            error: function (xhr) {
                showToast(xhr.responseJSON?.error || 'Error sending review');
            }
        });
    });
});