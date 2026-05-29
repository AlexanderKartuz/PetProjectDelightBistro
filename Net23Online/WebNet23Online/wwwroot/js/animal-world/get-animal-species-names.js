$(document).ready(function () {
    const $select = $('#animal-type-select');
    const $fb = $('#fact-form-feedback');
    const $textInput = $('#factText');
    const $addBtn = $('#addFactBtn');

    // Настройка начального состояния загрузки видов
    $select.prop('disabled', true).html('<option>Загрузка...</option>');
    $fb.text('Загрузка видов...').attr('class', 'zoo-name-feedback zoo-name-feedback-checking');

    // 1. Загрузка списка животных в select
    $.getJSON('/api/AnimalWorld/GetAnimalSpeciesNames')
        .done(list => {
            $select.empty();

            if (list && list.length > 0) {
                // Заполняем список. Первое животное автоматически станет выбранным по умолчанию
                list.forEach(name => $select.append(new Option(name, name)));
                $fb.text('').attr('class', 'zoo-name-feedback');
            } else {
                $select.html('<option value="">Животные не найдены</option>');
                $fb.text('Список видов пуст').attr('class', 'zoo-name-feedback zoo-name-feedback-invalid');
            }

            // Запускаем загрузку существующих фактов
            loadFacts();
        })
        .fail(() => {
            $fb.text('Ошибка загрузки видов').attr('class', 'zoo-name-feedback zoo-name-feedback-invalid');
            // Всё равно пытаемся загрузить факты, если бэкенд видов недоступен
            loadFacts();
        })
        .always(() => {
            // Активируем select только если в нем есть доступные элементы
            if ($select.find('option[value!=""]').length > 0) {
                $select.prop('disabled', false);
            }
        });

    // 2. Обработчик клика на кнопку «Добавить факт»
    $addBtn.on('click', function () {
        const animalName = $select.val();
        const textValue = $textInput.val().trim();

        // Валидация выбора животного
        if (!animalName) {
            $fb.text('Пожалуйста, выберите вид животного.')
                .attr('class', 'zoo-name-feedback zoo-name-feedback-invalid');
            return;
        }

        // Валидация текста факта
        if (!textValue) {
            $fb.text('Пожалуйста, введите текст факта.')
                .attr('class', 'zoo-name-feedback zoo-name-feedback-invalid');
            $textInput.focus();
            return;
        }

        // Индикация процесса отправки на сервер
        $fb.text('Сохранение факта...')
            .attr('class', 'zoo-name-feedback zoo-name-feedback-checking');
        $addBtn.prop('disabled', true);

        // Сборка JSON по схеме из Swagger
        const requestData = {
            id: 0,
            animalSpeciesName: animalName,
            text: textValue
        };

        // Отправка POST запроса
        $.ajax({
            url: 'https://localhost:7264/AddFact',
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(requestData),
            success: function () {
                // Выводим сообщение об успехе
                $fb.text('Факт успешно добавлен!')
                    .attr('class', 'zoo-name-feedback zoo-name-feedback-valid');

                // Очищаем поле ввода
                $textInput.val('');

                // Прячем текст "Фактов пока нет", если он отображался
                $('#facts-loading-status').hide();

                // Рендерим новую карточку в самый верх списка с CSS-анимацией
                const newFactHtml = `
                    <div class="animal comment-item-box comment-item-new">
                        <div class="comment-item-header">
                            <span class="comment-author">${animalName}</span>
                        </div>
                        <p class="comment-text">${textValue}</p>
                    </div>
                `;
                $('#facts-container').prepend(newFactHtml);
            },
            error: function () {
                $fb.text('Ошибка при отправке запроса на сервер.')
                    .attr('class', 'zoo-name-feedback zoo-name-feedback-invalid');
            },
            complete: function () {
                // В любом случае возвращаем кнопку в рабочее состояние
                $addBtn.prop('disabled', false);
            }
        });
    });
});

// 3. Функция загрузки и отрисовки фактов с сервера
function loadFacts() {
    const $container = $('#facts-container');
    const $status = $('#facts-loading-status');

    $.getJSON('https://localhost:7264/GetFacts')
        .done(facts => {
            if (!facts || facts.length === 0) {
                $status.text('Фактов о животных пока нет. Будьте первым!').show();
                return;
            }

            // Прячем статус и удаляем старые элементы перед рендером
            $status.hide();
            $container.find('.comment-item-box').remove();

            // Выводим полученную коллекцию фактов
            facts.forEach(fact => {
                const factHtml = `
                    <div class="animal comment-item-box">
                        <div class="comment-item-header">
                            <span class="comment-author">${fact.animalSpeciesName}</span>
                        </div>
                        <p class="comment-text">${fact.text}</p>
                    </div>
                `;
                $container.append(factHtml);
            });
        })
        .fail(() => {
            $status.text('Не удалось загрузить факты. Пожалуйста, обновите страницу.')
                .attr('class', 'empty-list-note zoo-name-feedback-invalid');
        });
}