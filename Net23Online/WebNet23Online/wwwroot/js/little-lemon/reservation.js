$(document).ready(function () {

    $('#DessertReferencePhoto').on('change', function () {
        const file = this.files[0];
        if (file) {
            $('.image-preview_container').removeClass('hide');
            const reader = new FileReader();
            reader.onload = function () {
                $('.image-preview_image').attr('src', reader.result);
            };
            reader.readAsDataURL(file);
        }
    });

    $('.image-preview_btn').on('click', function () {
        $('.image-preview_container').addClass('hide');
        $('#DessertReferencePhoto').val('');
    });

    $('#reserve-date, #reserve-time, #seating').on('blur change', function () {
        const date = $('#reserve-date').val();
        const time = $('#reserve-time').val();
        const seating = $('#seating').val();
        const url = `/api/LittleLemonReservation/HasDuplicate?date=${date}&time=${time}&seatingPreference=${seating}`;
        $.get(url)
            .done(function (hasDuplicate) {
                if (hasDuplicate) {
                    $('.reservation-duplicate-warning').removeClass('hide');
                    $('.booking-section .btn').attr('disabled', 'disabled');
                } else {
                    $('.reservation-duplicate-warning').addClass('hide');
                    $('.booking-section .btn').removeAttr('disabled');
                }
            });
    });

});
