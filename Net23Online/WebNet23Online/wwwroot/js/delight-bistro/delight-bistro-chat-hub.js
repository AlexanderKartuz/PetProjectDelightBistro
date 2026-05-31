document.addEventListener('DOMContentLoaded', function () {

    const { hub, ready } = window.delightBistroSignalR;

    const chatRoom = document.querySelector('.chat-room-messages');
    const sendButton = document.querySelector('.btn-send-message');
    const inputField = document.querySelector('.message-input');
    const currentUser = document.querySelector('.current-user');
    let currentUserName = currentUser.textContent.trim();
    const connectedUsers = new Map();

    ready.then(function () {
        sendButton.addEventListener('click', sendMessage);

        inputField.addEventListener('keypress', function (event) {
            if (event.key === 'Enter') {
                sendMessage();
            }
        });
                
        hub.on('ConnectedUsers', function (connectedUserList) {
            console.log('User подключился к чату');
            console.log(connectedUserList);
            connectedUserList.forEach(function (user) {
                addUserToList(user.connectionId, user.userName);
            });
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
            console.log(`${userName} Подключился к хабу (id ${connectionId})`);
            addUserToList(connectionId, userName);
        });

        // add userName
        hub.on('UserDisconnected', function (connectionId, userName) {
            console.log(`${userName} отключился (id ${connectionId})`);
            removeUserFromList(connectionId);
        });

        hub.invoke('JoinChat');

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

            hub.invoke('SendMessage', currentUserName, messageText);
                        
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

            connectedUsers.set(connectionId, userDiv);
        };
        function removeUserFromList(connectionId) {
            const userDiv = connectedUsers.get(connectionId);

            if (userDiv) {
                userDiv.remove();
                connectedUsers.delete(connectionId);
                return;
            }

            const userToRemove = document
                .querySelector(`[data-connection-id="${connectionId}"]`);

            if (userToRemove) {
                userToRemove.remove();
            }
        };

    });
});

