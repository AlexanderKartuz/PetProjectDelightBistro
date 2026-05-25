$(document).ready(function () {
    const $input = $('#ZooName');
    const $feedback = $('#zoo-name-feedback');
    const $submit = $('.js-add-zoo-form button[type="submit"]');

    $input.on('input', function () {
        $submit.prop('disabled', true);

        $feedback
            .text('Проверяем…')
            .css('color', 'gray');

        $.getJSON('/api/AnimalWorld/IsZooNameFree', {
            zooName: $.trim($input.val())
        })
            .done(function (isFree) {
                $feedback
                    .text(isFree ? 'Свободно' : 'Заняto')
                    .css('color', isFree ? 'green' : 'red');

                $submit.prop('disabled', !isFree);
            });
    });
});