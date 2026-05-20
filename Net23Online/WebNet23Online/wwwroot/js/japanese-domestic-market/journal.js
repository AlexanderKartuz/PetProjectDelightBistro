$(document).ready(function () {
    $('article.comment').click(function () {
        const self = $(this);
        self.toggleClass('active');
        const hasActiveComments = $('article.comment.active').length > 0;

        if (hasActiveComments){
            $('.delete-comments').removeAttr('disabled');
        }else {
            $('.delete-comments').attr('disabled', 'disabled');
        }

        $('.delete-comments').click(function () {
            $('article.comment.active').remove();
        });
    });
})     

$(document).ready(function () {
    document.querySelectorAll('.container-content img').forEach(img => {
        img.onclick = () => {
            document.querySelector('.pop-up').style.display = 'block';
            document.querySelector('.pop-up img').src = img.getAttribute('src')
        }
    });

    document.querySelector('.pop-up span').onclick = () => {
        document.querySelector('.pop-up').style.display = 'none';
    }
});

