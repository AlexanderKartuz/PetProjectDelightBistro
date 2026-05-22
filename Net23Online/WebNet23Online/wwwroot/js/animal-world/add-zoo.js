document.addEventListener('DOMContentLoaded', function () {
    const nameInput = document.getElementById('ZooName');
    const feedback = document.getElementById('zoo-name-feedback');
    const submitButton = document.querySelector('.js-add-zoo-form button[type="submit"]');

    if (!nameInput || !feedback || !submitButton) {
        return;
    }

    let debounceTimer = null;
    let requestId = 0;

    const messages = {
        empty: '',
        checking: 'Проверяем название…',
        free: 'Название свободно',
        taken: 'Такое имя уже используется',
        error: 'Не удалось проверить название. Попробуйте ещё раз.',
    };

    function setFeedback(state) {
        feedback.textContent = messages[state] || '';
        feedback.className = 'zoo-name-feedback';

        nameInput.classList.remove('zoo-name-free', 'zoo-name-taken', 'zoo-name-checking');

        if (state === 'checking') {
            feedback.classList.add('zoo-name-feedback-checking');
            nameInput.classList.add('zoo-name-checking');
            submitButton.disabled = true;
            return;
        }

        if (state === 'free') {
            feedback.classList.add('zoo-name-feedback-valid');
            nameInput.classList.add('zoo-name-free');
            submitButton.disabled = false;
            return;
        }

        if (state === 'taken') {
            feedback.classList.add('zoo-name-feedback-invalid');
            nameInput.classList.add('zoo-name-taken');
            submitButton.disabled = true;
            return;
        }

        if (state === 'error') {
            feedback.classList.add('zoo-name-feedback-invalid');
            submitButton.disabled = false;
            return;
        }

        submitButton.disabled = false;
    }

    async function checkZooName(name) {
        const currentRequest = ++requestId;
        setFeedback('checking');

        try {
            const url = `/api/AnimalWorld/IsZooNameFree?zooName=${encodeURIComponent(name)}`;
            const response = await fetch(url);

            if (currentRequest !== requestId) {
                return;
            }

            if (!response.ok) {
                setFeedback('error');
                return;
            }

            const isFree = await response.json();
            setFeedback(isFree ? 'free' : 'taken');
        } catch {
            if (currentRequest === requestId) {
                setFeedback('error');
            }
        }
    }

    nameInput.addEventListener('input', function () {
        clearTimeout(debounceTimer);
        const name = nameInput.value.trim();

        if (!name) {
            requestId++;
            setFeedback('empty');
            return;
        }

        debounceTimer = setTimeout(function () {
            checkZooName(name);
        }, 400);
    });

    if (nameInput.value.trim()) {
        checkZooName(nameInput.value.trim());
    }
});
