$(document).ready(function () {

    $('#Name').on('change', function () {
        const newName = $('#Name').val();
        $('.media-card__img-wrap img').attr('alt', `Персонаж ${newName}`)
        $('.media-card__name').text(newName);
    });

    $('#Url').on('change', function () {
        const newUrl = $('#Url').val();
        $('.media-card__img-wrap img').attr('src', newUrl);
    });

    $('#AnimeId').on('change', function () {
        const newAnimeName = $('#AnimeId option:selected').text();
        $('.media-card__animes').text(newAnimeName);
    });
});