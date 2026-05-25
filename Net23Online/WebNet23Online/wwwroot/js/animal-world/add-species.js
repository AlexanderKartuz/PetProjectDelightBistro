$(document).ready(function () {
    const $previewContainer = $('#preview-container');
    const $previewImage = $('#image-preview');

    $previewContainer.hide();

    $('#image-input').on('change', function (event) {
        const input = event.target;

        if (input.files && input.files[0]) {
            const reader = new FileReader();

            reader.onload = function (e) {
                $previewImage.attr('src', e.target.result);
                $previewContainer.fadeIn(200);
            };

            reader.readAsDataURL(input.files[0]);
        } else {
            $previewContainer.fadeOut(150, function () {
                $previewImage.attr('src', '#');
            });
        }
    });
});