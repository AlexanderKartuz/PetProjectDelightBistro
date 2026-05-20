$(document).ready(function () {

    $('#band-name').on('change', function () {
        const newName = $('#band-name').val();
        $('.create-rockband .preview .band-thumb img').attr('alt', newName);
        $('.create-rockband .preview .band-body h2').text(newName);
    });

    $('#band-image').on('change', function () {
        const newUrl = $('#band-image').val();
        $('.create-rockband .preview .band-thumb img').attr('src', newUrl);
    });

    $('.create-rockband .genre-filter-checkbox').on('change', function () {
        const genreNames = $('.create-rockband .genre-filter-checkbox:checked')
            .map(function () {
                return $('label[for="' + this.id + '"]').text();
            })
            .get()
            .join(', ');
        $('.create-rockband .preview .band-preview-genres').text(genreNames || '***');
    });
});
