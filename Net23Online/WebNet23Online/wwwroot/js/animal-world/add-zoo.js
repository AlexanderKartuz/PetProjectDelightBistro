$(document).ready(function () {
    const $input = $('#ZooName'), $feedback = $('#zoo-name-feedback');
    const $submit = $('.js-add-zoo-form button[type="submit"]');

    if (!$input.length || !$feedback.length || !$submit.length) {
        return;
    }

    let debounceTimer = null, requestId = 0;

    const messages = {
        empty: '',
        checking: 'Проверяем название…',
        free: 'Название свободно',
        taken: 'Такое имя уже используется',
        error: 'Не удалось проверить название. Попробуйте ещё раз.',
    };

    function setFeedback(state) {
        $feedback.text(messages[state] || '').attr('class', 'zoo-name-feedback');
        $input.removeClass('zoo-name-free zoo-name-taken zoo-name-checking');
        $submit.prop('disabled', ['checking', 'taken'].includes(state));
        if (state === 'checking') {
            $feedback.addClass('zoo-name-feedback-checking');
            $input.addClass('zoo-name-checking');
        } else if (state === 'free') {
            $feedback.addClass('zoo-name-feedback-valid');
            $input.addClass('zoo-name-free');
        } else if (state === 'taken' || state === 'error') {
            $feedback.addClass('zoo-name-feedback-invalid');
            if (state === 'taken') $input.addClass('zoo-name-taken');
        }
    }

    function checkZooName(name) {
        const currentRequest = ++requestId;
        setFeedback('checking');
        $.getJSON(`/api/AnimalWorld/IsZooNameFree`, { zooName: name })
            .done(function (isFree) {
                if (currentRequest === requestId) {
                    setFeedback(isFree ? 'free' : 'taken');
                }
            })
            // .fail(function () {
            //     if (currentRequest === requestId) {
            //         setFeedback('error');
            //     }
            // });
    }

    $input.on('input', function () {
        clearTimeout(debounceTimer);
        const name = $.trim($input.val());

        if (!name) {
            requestId++;
            setFeedback('empty');
            return;
        }

        debounceTimer = setTimeout(() => checkZooName(name), 400);
    });

    const initialName = $.trim($input.val());
    if (initialName) checkZooName(initialName);
});