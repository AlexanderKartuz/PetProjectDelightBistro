document.addEventListener('DOMContentLoaded', function () {
    const url = `https://localhost:7284/my-hub/delightbistro`;
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();
    const chatRoom = document.querySelector('.chat-room-messages');
    const sendButton = document.querySelector('.btn-send-message');
    const inputField = document.querySelector('.message-input');
    const currentUser = document.querySelector('.current-user');
    let currentUserName = currentUser.textContent.trim();

    sendButton.addEventListener('click', sendMessage);

    inputField.addEventListener('keypress', function (event) {
        if (event.key === 'Enter') {
            sendMessage();
        }
    });

    hub.on('SetUserName', function (userName) {
        currentUser.textContent = userName;
        currentUserName = userName;
    });

    hub.on('ReceiveMessage', function (senderName, message) {
        console.log(`New message: ${senderName}, ${message}`);

        const isSend = (senderName === currentUserName)
        addMessageToChat(senderName, message, isSend);

    });


    function addMessageToChat(senderName, messageText, isSent) {

        //messageBox
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('message');
        messageDiv.classList.add(isSent ? 'sent' : 'recived');

        // senderName
        const messageSenderDiv = document.createElement('div');
        messageSenderDiv.classList.add('message-sender');
        messageSenderDiv.textContent = isSent ? 'Me' : senderName;

        const messageContentDiv = document.createElement('div');
        messageContentDiv.classList.add('message-text');
        messageContentDiv.textContent = messageText;

        messageDiv.appendChild(messageSenderDiv);
        messageDiv.appendChild(messageContentDiv);

        chatRoom.appendChild(messageDiv);
    }

    function sendMessage() {
        const messageText = inputField.value.trim();
        if (!messageText) {
            return;
        }

        // hub
        hub.invoke('SendMessage', currentUserName, messageText);

        // или добавить локально, но вызывать в хабе Clients.Other
        // addMessageToChat(messageSender, messageText, true) 
        inputField.value = '';
        console.log('Сообщение оправлено' + messageText);

    }

    hub.start();
});

