window.animeGirlSignalR = (function () {
    const url = '/my-hub/anime';
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();
    const ready = hub.start();

    return { hub, ready };
})();
