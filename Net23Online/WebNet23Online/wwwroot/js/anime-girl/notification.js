$(document).ready(function () {

    const url = `https://localhost:7284/my-hub/anime`;
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    hub.on('NewAnimeCreated', function (name, url) {
        console.log(`new anime was created. Anime name: ${name}`);

        const newNotificationDiv = $('<div>');
        newNotificationDiv.addClass('notification');
        newNotificationDiv.text(`New anime name: ${name}`);

        const img = $('<img>');
        img.attr('src', url);
        img.addClass('preview');
        newNotificationDiv.append(img);

        $('.notifications').append(newNotificationDiv);

        //setTimeout(() => {
        //    newNotificationDiv.hide(500);
        //}, 5000);

        newNotificationDiv.click(hideNotification);
    });

    function hideNotification() {
        $(this).hide(500);
    }

    hub.start();// call server
});