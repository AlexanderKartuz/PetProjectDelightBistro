document.addEventListener('DOMContentLoaded', function () {
    const { hub, ready } = window.animeGirlSignalR;

    const toggleButton = document.querySelector('.anime-chat-toggle');
    const panel = document.querySelector('.anime-chat-panel');
    const closeButton = document.querySelector('.anime-chat-panel__close');
    const messagesContainer = document.querySelector('.anime-chat-messages');
    const inputField = document.querySelector('.anime-chat-input');
    const sendButton = document.querySelector('.anime-chat-send');
    const shareButton = document.querySelector('.anime-chat-share');
    const userNameLabel = document.querySelector('.anime-chat-panel__user');
    const messageTemplate = document.getElementById('anime-chat-message-template');
    const shareCardTemplate = document.getElementById('anime-chat-share-card-template');

    let currentUserName = '';
    let isPanelOpen = false;
    let isInChat = false;
    let selectedCount = 0;
    let handlersRegistered = false;

    toggleButton.addEventListener('click', function () {
        if (isPanelOpen) {
            closePanel();
        } else {
            openPanel();
        }
    });

    closeButton.addEventListener('click', closePanel);

    sendButton.addEventListener('click', sendMessage);
    shareButton.addEventListener('click', shareSelectedCharacters);

    inputField.addEventListener('keypress', function (event) {
        if (event.key === 'Enter') {
            sendMessage();
        }
    });

    window.addEventListener('animeGirlSelectionChanged', function (event) {
        selectedCount = event.detail?.count ?? 0;
        updateShareButtonState();
    });

    ready.then(function () {
        if (handlersRegistered) {
            return;
        }

        handlersRegistered = true;

        hub.on('SetUserName', function (userName) {
            currentUserName = userName;
            userNameLabel.textContent = `Вы: ${userName}`;
        });

        hub.on('UserJoinedChat', function (userName) {
            appendSystemMessage(`В чат зашёл ${userName}`);
        });

        hub.on('UserLeftChat', function (userName) {
            appendSystemMessage(`Из чата вышел ${userName}`);
        });

        hub.on('ReceiveMessage', function (senderName, message) {
            const isSent = senderName === currentUserName;
            appendChatMessage(senderName, message, isSent);
        });

        hub.on('ReceiveSharedCharacters', function (senderName, characters) {
            appendSharedCharactersMessage(senderName, characters);
        });
    });

    function openPanel() {
        isPanelOpen = true;
        panel.classList.add('is-open');
        toggleButton.setAttribute('aria-expanded', 'true');

        ready.then(function () {
            if (isInChat) {
                updateShareButtonState();
                return;
            }

            hub.invoke('JoinChat')
                .then(function () {
                    isInChat = true;
                    updateShareButtonState();
                })
                .catch(function (error) {
                    console.error('JoinChat failed', error);
                });
        });
    }

    function closePanel() {
        isPanelOpen = false;
        panel.classList.remove('is-open');
        toggleButton.setAttribute('aria-expanded', 'false');
        updateShareButtonState();

        if (!isInChat) {
            return;
        }

        ready.then(function () {
            hub.invoke('LeaveChat')
                .then(function () {
                    isInChat = false;
                    updateShareButtonState();
                })
                .catch(function (error) {
                    console.error('LeaveChat failed', error);
                });
        });
    }

    function updateShareButtonState() {
        shareButton.disabled = !isInChat || selectedCount === 0;
    }

    function getSelectedCharacterIds() {
        return Array.from(document.querySelectorAll('article.media-card.active'))
            .map(function (card) {
                return parseInt(card.getAttribute('data-id'), 10);
            })
            .filter(function (id) {
                return !Number.isNaN(id);
            });
    }

    function shareSelectedCharacters() {
        const ids = getSelectedCharacterIds();

        if (!isInChat) {
            return;
        }

        if (ids.length === 0) {
            appendSystemMessage('Сначала выберите персонажей на странице');
            return;
        }

        hub.invoke('ShareCharacters', ids)
            .catch(function (error) {
                console.error('ShareCharacters failed', error);
            });
    }

    function sendMessage() {
        const messageText = inputField.value.trim();
        if (!messageText || !isInChat) {
            return;
        }

        hub.invoke('SendMessage', messageText)
            .then(function () {
                inputField.value = '';
            })
            .catch(function (error) {
                console.error('SendMessage failed', error);
            });
    }

    function appendSystemMessage(text) {
        const systemDiv = document.createElement('div');
        systemDiv.classList.add('anime-chat-system');
        systemDiv.textContent = text;
        messagesContainer.appendChild(systemDiv);
        scrollToBottom();
    }

    function appendChatMessage(senderName, messageText, isSent) {
        const messageNode = messageTemplate.content.cloneNode(true);
        const messageDiv = messageNode.querySelector('.anime-chat-message');

        messageDiv.classList.add(isSent ? 'anime-chat-message--sent' : 'anime-chat-message--received');
        messageNode.querySelector('.anime-chat-message__sender').textContent = isSent ? 'Я' : senderName;
        messageNode.querySelector('.anime-chat-message__text').textContent = messageText;

        messagesContainer.appendChild(messageNode);
        scrollToBottom();
    }

    function appendSharedCharactersMessage(senderName, characters) {
        const wrapper = document.createElement('div');
        wrapper.classList.add('anime-chat-share-message');

        const isSent = senderName === currentUserName;
        const header = document.createElement('p');
        header.classList.add('anime-chat-share-message__header');
        header.textContent = isSent
            ? 'Вы поделились персонажами'
            : `${senderName} поделился персонажами`;
        wrapper.appendChild(header);

        const grid = document.createElement('div');
        grid.classList.add('anime-chat-share-grid');

        characters.forEach(function (character) {
            const cardNode = shareCardTemplate.content.cloneNode(true);
            const img = cardNode.querySelector('.anime-chat-share-card__img');
            const title = cardNode.querySelector('.anime-chat-share-card__title');
            const likes = cardNode.querySelector('.anime-chat-share-card__likes');

            img.src = character.url;
            img.alt = `Персонаж ${character.title}`;
            title.textContent = character.title;
            likes.textContent = `Лайки: ${character.likes}`;

            grid.appendChild(cardNode);
        });

        wrapper.appendChild(grid);
        messagesContainer.appendChild(wrapper);
        scrollToBottom();
    }

    function scrollToBottom() {
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    selectedCount = document.querySelectorAll('article.media-card.active').length;
    updateShareButtonState();
});
