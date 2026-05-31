$(document).ready(function () {

    const url = `https://localhost:7284/my-hub/rock-band`;
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    hub.on('NewRockBandWasCreated', function (name, imageUrl) {
        console.log(`new rock band was created. Band name: ${name}`);

        const newNotificationDiv = $('<div>');
        newNotificationDiv.addClass('notification');
        newNotificationDiv.text(`New rock band: ${name}`);

        if (imageUrl) {
            const img = $('<img>');
            img.attr('src', imageUrl);
            img.addClass('preview');
            newNotificationDiv.append(img);
        }

        $('.notifications').append(newNotificationDiv);

        newNotificationDiv.click(hideNotification);
    });

    function hideNotification() {
        $(this).hide(500);
    }

    hub.start();
});
