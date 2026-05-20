$(document).ready(function () {
    const $deleteBtn = $('#delete-selected-btn');

    function toggleDeleteButton() {
        const selectedCount = $('.js-family-card.selected').length;
        if (selectedCount > 0) {
            $deleteBtn.fadeIn(200);
        } else {
            $deleteBtn.fadeOut(200);
        }
    }

    $('.js-family-card').on('click', function () {
        $(this).toggleClass('selected');
        toggleDeleteButton();
    });

    $deleteBtn.on('click', function () {
        const $selectedCards = $('.js-family-card.selected');

        $selectedCards.fadeOut(300, function () {
            $(this).remove();
            toggleDeleteButton();

            if ($('.js-family-card').length === 0) {
                const emptyText = $('.entity-grid').data('empty-text');
                $('.entity-grid').replaceWith(`<p class="empty-list-note">${emptyText}</p>`);
            }
        });
    });
});