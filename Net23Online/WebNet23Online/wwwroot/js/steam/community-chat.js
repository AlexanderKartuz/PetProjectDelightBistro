$(document).ready(function () {
    const url = "/steam/community-chat";
    const currentUserId = parseInt($('.chat-page').data('user-id'), 10);
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    function formatTime(date) {
        return new Date(date).toLocaleTimeString([], {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        });
    }

    function appendMessage(username, message, timestamp, userId) {
        const isOwn = userId === currentUserId;
        const messageClass = isOwn ? 'message-own' : 'message-other';

        const messageHtml = `
            <div class="message ${messageClass}">
                <strong>${username}</strong>
                <span class="time">${formatTime(timestamp || new Date())}</span>
                <div>${message}</div>
            </div>
        `;

        $('.messages').append(messageHtml);

        const messagesDiv = $('.messages')[0];
        messagesDiv.scrollTop = messagesDiv.scrollHeight;
    }

    hub.on('SendChatMessage', function (username, message, userId, timestamp) {
        appendMessage(username, message, timestamp, userId);
    });

    hub.start();

    $('#sendButton').on('click', function () {
        const message = $('#messageInput').val().trim();
        if (!message) {
            return;
        }

        const btn = $(this);

        $.ajax({
            url: `/api/Chat/SendChatMessage?message=${encodeURIComponent(message)}`,
            type: 'POST',
            success: function () {
                $('#messageInput').val('');
            },
            error: function (err) {
                console.error('Error:', err);
            },
            complete: function () {
                btn.prop('disabled', false).text('Send');
            }
        });
    });
});
