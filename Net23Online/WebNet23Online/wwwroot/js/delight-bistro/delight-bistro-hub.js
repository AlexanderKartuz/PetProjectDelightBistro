document.addEventListener('DOMContentLoaded', function () {
    // одно подключение 
    const { hub, ready } = window.delightBistroSignalR;
    ready.then(function () {

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
    });
});
