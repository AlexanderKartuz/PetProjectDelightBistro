$(document).ready(function () {
    const $select = $('#animal-type-select')
        .prop('disabled', true)
        .html('<option>Загрузка...</option>');
    const $fb = $('#fact-form-feedback')
        .text('Загрузка видов...')
        .attr('class', 'zoo-name-feedback zoo-name-feedback-checking');

    $.getJSON('/api/AnimalWorld/GetAnimalSpeciesNames')
        .done(list => {
            $select.html('<option value="">-- Выберите вид животного --</option>');
            list.forEach(name => $select.append(new Option(name, name)));
            $fb.text('').attr('class', 'zoo-name-feedback');
        })
        .fail(() => $fb.text('Ошибка загрузки').attr('class', 'zoo-name-feedback zoo-name-feedback-invalid'))
        .always(() => $select.prop('disabled', false));
});