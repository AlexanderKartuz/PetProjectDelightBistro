$(document).ready(function () {
    const url = `https://localhost:7284/my-hub/animal-world-promotions`;
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    hub.on('ZoosPromotions', function (text) {
        const notification = $('<div>').addClass('zoo-notification');
        const title = $('<div>').addClass('zoo-notification-title').text('Акция!');
        const bodyText = text;
        const body = $('<div>').addClass('zoo-notification-body').text(bodyText);

        notification.append(title).append(body);

        $('.notifications-container').append(notification);

        notification.click(function () {
            hideElement($(this));
        });

        setTimeout(() => {
            hideElement(notification);
        }, 5000);
    });

    function hideElement(element) {
        element.fadeOut(400, function () {
            $(this).remove();
        });
    }

    hub.start();
});