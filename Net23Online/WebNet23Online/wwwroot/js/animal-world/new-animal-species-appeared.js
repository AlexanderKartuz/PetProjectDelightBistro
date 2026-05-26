$(document).ready(function () {
    const url = `https://localhost:7284/my-hub/animal-world`;
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    hub.on('NewAnimalInZooAppeared', function (zooName, animalSpeciesName) {
        const notification = $('<div>').addClass('zoo-notification');
        const title = $('<div>').addClass('zoo-notification-title').text('Пополнение в зоопарке!');

        const bodyText = `В зоопарке "${zooName}" появился новый вид животного — ${animalSpeciesName}!`;
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