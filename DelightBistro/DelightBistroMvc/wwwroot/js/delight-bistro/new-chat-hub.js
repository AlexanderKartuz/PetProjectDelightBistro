document.addEventListener('DOMContentLoaded', function () {
    const hub = new signalR.HubConnectionBuilder()
        .withUrl('/my-hub/new-chat')
        .withAutomaticReconnect()
        .build();

    const chatRoom = document.querySelector('.chat-room-messages');
    const sendBytton = document.querySelector('.btn-send-message');
    const inputField = document.querySelector('.message-input');
    const currentUserEl = document.querySelector('.current-user');
    const userList = document.querySelector('.user-list');

    let currentUserName = '';
    const connectedUsers = new Map();

    // Server => Client
    hub.on('SetUserName', function (userName) {
        currentUserName = userName;
        currentUserEl.textContent = 'You: ' + userName;
    });

    hub.on('ReceiveHistory', function (messages) {
        chatRoom.innerHTML = '';
        (messages || []).forEach(function (msg) {
            addMessageToChat(msg.sendernName, msg.text, msg.senderName == currentUserName);
        });
    });

    hub.on('ReceiveMessage', function (message) {
        const isSent = message.senderName == currentUserName;
        addMessageToChat(message.senderName, message.text, isSent);
    });

    hub.on('ConnectedUsers', function (users) {
        (users || []).forEach(function (user) {
            addUserToList(user.connectionId, user.userName);
        });
    });

    hub.on('UserConnected', function (connetionId, userName) {
        addUserToList(connetionId, userName);
    });

    hub.on('UserDisconnected', function (connetionId, userName) {
        removeUserFromList(connetionId);
    });

    // try reconnect
    hub.onreconnected(function () {
        connectedUsers.clear();
        userList.innerHTML = '';
        return hub.invoke('JoinChat');
    });

    // clent => Server
    hub.start().then(function () {
        sendBytton.addEventListener('click', sendMessage);

        inputField.addEventListener('keypress', function (event) {
            if (event.key == 'Enter') {
                sendMessage();
            }
        });

        return hub.invoke('JoinChat');
    });

    function sendMessage() {
        const text = inputField.value.trim();
        if (!text) {

            return;
        }

        hub.invoke('SendMessage', text);
        inputField.value = '';
    }

    function addMessageToChat(senderName, messageText, isSent) {
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('message');
        messageDiv.classList.add(isSent ? 'sent' : 'recived');

        const messageSenderDiv = document.createElement('div');
        messageSenderDiv.classList.add('message-sender');
        messageSenderDiv.textContent = isSent ? 'Me' : senderName;

        const messageContentDiv = document.createElement('div');
        messageContentDiv.classList.add('message-text');
        messageContentDiv.textContent = messageText;

        messageDiv.appendChild(messageSenderDiv);
        messageDiv.appendChild(messageContentDiv);
        chatRoom.appendChild(messageDiv);

        chatRoom.scrollTop = chatRoom.scrollHeight;
    }

    function addUserToList(connectionId, userName) {
        if (connectedUsers.has(connectionId)) {
            return;
        }

        const userDiv = document.createElement('div');
        userDiv.dataset.connectionId = connectionId;
        userDiv.classList.add('chat-user');

        const nameDiv = document.createElement('div');
        nameDiv.classList.add('user-name');
        nameDiv.textContent = userName;

        userDiv.appendChild(nameDiv);
        userList.appendChild(userDiv);
        connectedUsers.set(connectionId, userDiv);
    }

    function removeUserFromList(connectionId) {
        const userDiv = connectedUsers.get(connectionId);
        if (userDiv) {
            userDiv.remove();
            connectedUsers.delete(connectionId);
        }
    }

});
