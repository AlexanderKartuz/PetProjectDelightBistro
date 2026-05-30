document.addEventListener('DOMContentLoaded', function () {
    const url = `https://localhost:7284/my-hub/delightbistro`;
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();
    const chatRoom = document.querySelector('.chat-room-messages');
    const sendButton = document.querySelector('.btn-send-message');
    const inputField = document.querySelector('.message-input');
    const currentUser = document.querySelector('.current-user');
    let currentUserName = currentUser.textContent.trim();
    const connectedUsers = new Map();

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

        const isSend = (senderName === currentUserName);
        addMessageToChat(senderName, message, isSend);

    });

    hub.on('UserConnected', function (connectionId, userName) {
        console.log(`${userName} Подключился к чату (id ${connectionId})`);
        // Add user to userlist
        addUserToList(connectionId, userName);
    });

    // add userName
    hub.on('UserDisconnected', function (connectionId, userName) {
        console.log(`${userName} отключился (id ${connectionId})`);
        // delete user from UserList
        removeUserFromList(connectionId);
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
    };

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
        console.log(`${currentUserName} Отправил сообщение:` + messageText);
    };

    function addUserToList(connectionId, userName) {

        if (connectedUsers.has(connectionId)) {
            return;
        }

        const userList = document.querySelector('.user-list');

        const userDiv = document.createElement('div');
        userDiv.dataset.connectionId = connectionId;
        userDiv.classList.add('chat-user');

        const nameDiv = document.createElement('div');
        nameDiv.classList.add('user-name');
        nameDiv.textContent = userName;

        userDiv.appendChild(nameDiv);
        userList.appendChild(userDiv);

        // для быстрого удаления
        connectedUsers.set(connectionId, userDiv);
    };
    //Исправить удаление и добавление
    // Use connectionId
    function removeUserFromList(connectionId) {
        const userToRemove = document
            .querySelector(`[data-connection-id="${connectionId}"]`);

        if (userToRemove) {
            userToRemove.remove();
        }
    };

    hub.start();
});

