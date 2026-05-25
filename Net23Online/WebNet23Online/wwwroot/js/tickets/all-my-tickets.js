$(document).ready(function () {
    $('.ticket-qrcode').each(function () {
        const $element = $(this);
        const qrText = $element.data('qr-value');
        if (qrText) {
            new QRCode(this, {
                text: qrText,
                width: 90,
                height: 90,
                colorDark: "#000000",
                colorLight: "#ffffff",
                correctLevel: QRCode.CorrectLevel.H
            });
        }
    });
});