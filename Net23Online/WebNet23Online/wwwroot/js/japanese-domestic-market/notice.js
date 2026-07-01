$(document).ready(function () {

    const url = `https://localhost:7284/my-hub/jdm`;
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    hub.on('NewJdmCarsCreated', function (model, price, url) {
        console.log(`new cars by added: ${model} ${price} ${url}`);

        const newNotificationDiv = $('<div>');
        newNotificationDiv.addClass('notice');
        newNotificationDiv.text('new cars added');

        const img = $('<img>');
        img.attr('src', url);
        img.addClass('preview')
        newNotificationDiv.append(img);

        $('.notice-box').append(newNotificationDiv);

        setTimeout(() => {
            card.fadeOut(400, function () { $(this).remove(); });
        }, 8000);
    })

    function hidenNotification() {
        $(this).hide(500);
    }

    hub.start();
});