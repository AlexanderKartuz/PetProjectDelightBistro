$(document).ready(function () {

    const url = `https://localhost:7284/my-hub/rock-legends`;
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    hub.on('NewGenreCreated', function (name, url) {
        console.log(`new genre was created. Genre name: ${name}`);

        const newNotificationDiv = $('<div>');
        newNotificationDiv.addClass('notification');
        newNotificationDiv.text(`New genre name: ${name}`);

        const img = $('<img>');
        img.attr('src', url);
        img.addClass('preview');
        newNotificationDiv.append(img);

        $('.notifications').append(newNotificationDiv);


        newNotificationDiv.click(hideNotification);
    });

    function hideNotification() {
        $(this).hide(500);
    }

    hub.start();
});