window.littleLemonSignalR = (function () {
    const url = `/my-hub/little-lemon`;

    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();
    const ready = hub.start();

    return { hub, ready };
})();
