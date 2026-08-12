$(document).ready(function () {
    $('#Login').on('change', function () {
        $('.icon').hide();
        $('.icon.wait').show();

        $('#Login').removeClass('free')
        $('#Login').removeClass('used');
        $('button').removeAttr('disabled');

        const userLogin = $('#Login').val();

        const url = `/api/Auth/IsLoginFree?login=${userLogin}`;
        $.get(url)
            .done(function (asnwer) {
                $('.icon.wait').hide();
                if (asnwer) {
                    $('#Login').addClass('free');
                    $('.icon.ok').show();
                } else {
                    $('#Login').addClass('used');
                    $('.icon.deny').show();
                    $('button').attr('disabled', 'disabled');
                }
            });
    })
});