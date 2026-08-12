$(document).ready(function () {

    const url = `https://localhost:7284/my-hub/notification`;
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    hub.on('NewMessage', function (text) {
        const newNotificationDiv = $('<div>');
        newNotificationDiv.addClass('notification');
        newNotificationDiv.text(text);

        $('.notifications').append(newNotificationDiv);

        setTimeout(() => {
            newNotificationDiv.hide(500);
        }, 5000);

        newNotificationDiv.click(hideNotification);
    });

    function hideNotification() {
        $(this).hide(500);
    }

    hub.start();// call server
});