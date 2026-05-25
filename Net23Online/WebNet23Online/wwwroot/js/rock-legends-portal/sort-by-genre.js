$(document).ready(function () {

    $('.genre-content-box').hide();

    $('.js-genre-toggle-btn').click(function () {
        const genreId = $(this).data('id');
        const contentBox = $('#genre-' + genreId);
        contentBox.slideToggle(300);
    });

    $('.js-visual-remove-btn').click(function (e) {
        e.preventDefault();

        const genreGroup = $(this).closest('.genre-group-wrapper');

        genreGroup.fadeOut(400, function () {
            $(this).remove();
        });
    });

});