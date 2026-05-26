document.addEventListener('DOMContentLoaded', function () {
  const url = `https://localhost:7284/my-hub/delightbistro`;
  const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

  hub.on('NewFoodWasCreated', function (name, price) {
    console.log(`New food was added: ${name}, ${price}`);

    const notificationBox = document.querySelector('.notifications');
    const newNotificationDiv = document.createElement('div');

    newNotificationDiv.textContent = `New food was added: ${name}, price: ${price} BYN`;
    newNotificationDiv.className = 'notification';
    notificationBox.appendChild(newNotificationDiv);

    setTimeout(() => {
      newNotificationDiv.remove();
    }, 5000);
  });

  hub.start();
});
