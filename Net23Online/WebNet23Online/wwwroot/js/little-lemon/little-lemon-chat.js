$(document).ready(function () {
    const $page = $('.chat-page');
    if (!$page.length) {
        return;
    }

    const userId = parseInt($page.data('user-id'), 10);
    const isAdmin = $page.data('is-admin').toString().toLowerCase() === 'true';
    const userName = $page.data('user-name');
    const $messages = $page.find('.chat-messages');
    const $input = $page.find('.chat-input');
    const $target = $page.find('.chat-target-user-id');
    const { hub, ready } = window.littleLemonSignalR;
    const messagesEl = $messages[0];

    function setChatField(root, name, text) {
        root.querySelector(`[data-field="${name}"]`).textContent = text;
    }

    function appendMessage(senderUserId, senderName, message) {
        const sent = senderUserId === userId;
        const template = document.getElementById('chat-message-template');
        const root = template.content.firstElementChild.cloneNode(true);

        root.classList.add(sent ? 'sent' : 'received');
        setChatField(root, 'sender', sent ? 'Me' : userName);
        setChatField(root, 'text', message);

        messagesEl.appendChild(root);
        messagesEl.scrollTop = messagesEl.scrollHeight;
    }

    function sendMessage() {
        const message = $input.val().trim();
        if (!message) {
            return;
        }

        let url = '/api/LittleLemonChat/SendMessageToAdmin?message=' + encodeURIComponent(message);
        if (isAdmin) {
            const targetUserId = parseInt($target.val(), 10);
            if (!targetUserId) {
                return;
            }
            url =
                '/api/LittleLemonChat/SendMessageToUser?targetUserId=' +
                targetUserId +
                '&message=' +
                encodeURIComponent(message);
        }

        $.post(url, function () {
            $input.val('');
        });
    }

    ready.then(function () {
        hub.on('ReceivePrivateMessage', function (senderUserId, senderName, message) {
            appendMessage(senderUserId, senderName, message);
            if (isAdmin && senderUserId !== userId) {
                $target.val(senderUserId);
            }
        });

        $page.on('click', '.chat-send', sendMessage);
        $input.on('keypress', function (e) {
            if (e.key === 'Enter') {
                sendMessage();
            }
        });
    });
});
