
$(document).ready(function () {
    const { hub, ready } = window.littleLemonSignalR;
    const $container = $('.notifications');

    ready.then(function () {
        hub.on(
            'NewReservationCreated',
            function (
                reservationId,
                guestName,
                reservationDate,
                time,
                numberOfGuests,
                seating,
                occasion,
                userComments,
                cakePhotoUrl
            ) {
                showReservationNotification(
                    {
                        reservationId,
                        guestName,
                        reservationDate,
                        time,
                        numberOfGuests,
                        seating,
                        occasion,
                        userComments,
                        cakePhotoUrl,
                    },
                    $container
                );
            }
        );
    });
});
const autoHideMs = 9000;

function setNotificationField($root, name, text) {
    $root.find(`[data-field="${name}"]`).text(text);
}

function dismissReservationNotification($root, autoHideTimer) {
    clearTimeout(autoHideTimer);
    $root.fadeOut(500, function () {
        $root.remove();
    });
}

function showReservationNotification(data, $container) {
    const template = $('#reservation-notification-template')[0];
    const $root = $(template.content.firstElementChild.cloneNode(true));

    $root
        .find('.notification-title')
        .text(`New reservation #${data.reservationId} from ${data.guestName}`);

    setNotificationField($root, 'name', `Name: ${data.guestName}`);
    setNotificationField($root, 'date', `Date: ${data.reservationDate}`);
    setNotificationField($root, 'time', `Time: ${data.time}`);
    setNotificationField($root, 'guests', `Guests: ${data.numberOfGuests}`);
    setNotificationField($root, 'seating', `Seating: ${data.seating}`);
    setNotificationField($root, 'occasion', `Occasion: ${data.occasion}`);
    setNotificationField($root, 'notes', `Notes: ${data.userComments || ''}`);

    if (data.cakePhotoUrl) {
        $root.find('[data-field="cake"]').prop('hidden', false);
        $root.find('[data-field="cake"] img').attr('src', data.cakePhotoUrl);
    }

    $container.append($root);

    const autoHideTimer = setTimeout(function () {
        dismissReservationNotification($root, autoHideTimer);
    }, autoHideMs);

    $root.on('click', function () {
        dismissReservationNotification($root, autoHideTimer);
    });
}