$(document).ready(function () {
    const url = "/steam/community-chat";
    const hub = new signalR.HubConnectionBuilder().withUrl(url).build();

    hub.on('SendChatMessage', function (username, message) {

        const newMessageDiv = $('<div>');
        newMessageDiv.addClass('message');
        newMessageDiv.text(message + "by" + username);

        $('.messages').append(newMessageDiv)
    });


    hub.start();

    $('#sendButton').on('click', function () {
        //const username = $('#currentUserName').val();
        //const userId = $('#currentUserId').val();
        const message = $('#messageInput').val();
        const url = `/api/Chat/SendChatMessage?message=${message}`;
        $.get(url)
    });
});