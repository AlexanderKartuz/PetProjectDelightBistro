$(document).ready(function () {
    const $select = $('#animal-type-select');
    const $text = $('#factText');
    const $container = $('#facts-container');

    $.getJSON('/api/AnimalWorld/GetAnimalSpeciesNames')
        .done(function (list) {
            $select.html(list.map(function (name) {
                return new Option(name, name);
            }));
            loadFacts();
        });

    $('#addFactBtn').on('click', function () {
        const animal = $select.val();
        const val = $text.val().trim();

        $.ajax({
            url: 'https://localhost:7264/AddFact',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                animalSpeciesName: animal,
                text: val
            }),
            success: function () {
                $text.val('');

                $(`<div class="animal fact-item comment-item-new">
                    <span class="fact-animal-type">${animal}</span>
                    <p class="comment-text">${val}</p>
                </div>`).prependTo($container);
            }
        });
    });

    function loadFacts() {
        $.getJSON('https://localhost:7264/GetFacts')
            .done(function (facts) {
                $('#facts-loading-status').hide();
                $container.find('.fact-item').remove();
                facts.forEach(function (fact) {
                    $(`<div class="animal fact-item">
                        <span class="fact-animal-type">${fact.animalSpeciesName}</span>
                        <p class="comment-text">${fact.text}</p>
                    </div>`).appendTo($container);
                });
            });
    }
});