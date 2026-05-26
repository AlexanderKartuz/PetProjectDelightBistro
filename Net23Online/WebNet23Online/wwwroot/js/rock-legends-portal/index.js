$(document).ready(function () {

    $('.rockStar').click(function () {
        const self = $(this);

        const videoUrl = self.data('video-url');

        const currentDescription = self.find('.rockStar-description');

        const videoElement = $('#groupVideo')[0];
        const videoSource = $('#videoSource')[0];

        const isOpening = currentDescription.css('display') !== 'block';

        $('.rockStar-description').css('display', 'none');

        if (isOpening) {
            currentDescription.css('display', 'block');

            if (videoSource && videoElement) {
                videoSource.src = videoUrl;
                videoElement.load();
                videoElement.play();
            }
        } else {
            currentDescription.css('display', 'none');
            if (videoElement) {
                videoElement.pause();
            }
            if (videoSource) {
                videoSource.src = "";
            }
        }
    });
    $('#btn-ajax-like').click(function () {
        const selectedBandId = $('#rock-poll').val();

        $.ajax({
            url: '/api/rock-legends/like/' + selectedBandId,
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    $('#like-status-msg')
                        .text('🔥 Голос учтен! Всего лайков: ' + response.newLikes)
                        .css('color', '#00ffcc')
                        .fadeIn(300);

                    $('#ajax-like-form').fadeOut(300);
                }
            },
            error: function (xhr) {
                const errorData = xhr.responseJSON;
                const errorMsg = errorData ? errorData.message : 'Ошибка при голосовании.';

                $('#like-status-msg')
                    .text(errorMsg)
                    .css('color', '#ff4d4d') 
                    .fadeIn(300);
            }
        });
    });

});