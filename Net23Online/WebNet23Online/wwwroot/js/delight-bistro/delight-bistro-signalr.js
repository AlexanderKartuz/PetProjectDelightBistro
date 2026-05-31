window.delightBistroSignalR = (function () {
  const url = `/my-hub/delightbistro`;

  const hub = new signalR.HubConnectionBuilder().withUrl(url).build();
  const ready = hub.start();

  return { hub, ready };
})();
