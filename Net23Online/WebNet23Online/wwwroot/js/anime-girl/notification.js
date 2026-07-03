$(document).ready(function () {
    const l10nEl = document.getElementById('anime-girl-l10n');
    const newAnimeLabel = l10nEl?.dataset.notificationNewAnime || 'New anime name:';
    const { hub, ready } = window.animeGirlSignalR;

    ready.then(function () {
        hub.on('NewAnimeCreated', function (name, url) {
            console.log(`new anime was created. Anime name: ${name}`);

            const newNotificationDiv = $('<div>');
            newNotificationDiv.addClass('notification');
            newNotificationDiv.text(`${newAnimeLabel} ${name}`);

            const img = $('<img>');
            img.attr('src', url);
            img.addClass('preview');
            newNotificationDiv.append(img);

            $('.notifications').append(newNotificationDiv);
            newNotificationDiv.click(hideNotification);
        });
    });

    function hideNotification() {
        $(this).hide(500);
    }
});
