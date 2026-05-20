$(document).ready(function () {

    $('article.media-card').click(function () {
        const self = $(this);

        // bad way to do it
        // $(this).css('border', '3px red solid'); //inline style

        // if (self.hasClass('active')) {
        //     self.removeClass('active');
        // } else {
        //     self.addClass('active');
        // }

        self.toggleClass('active');

        const atLeastOneItemForRemove = $('article.media-card.active').length > 0

        if (atLeastOneItemForRemove) {
            $('.section-heroes .remove-image').removeAttr('disabled');
        } else {
            $('.section-heroes .remove-image').attr('disabled', 'disabled');
        }
    });

    $('.section-heroes .remove-image').click(function(){
        $('article.media-card.active').remove();
    });

});