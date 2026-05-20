$(document).ready(function () {
    $('article.card').click(function () {
        const self = $(this);
        self.toggleClass('active');
        const atLeastOneItemForRemove = $('article.card.active').length > 0

        if (atLeastOneItemForRemove) {
            $('.form-actions .remove-game-card').removeAttr('disabled');
        } else {
            $('.form-actions .remove-game-card').attr('disabled', 'disabled');
        }
    })

    $('.form-actions .remove-game-card').click(function () {
        $('article.card.active').remove();
    });
});
