document.addEventListener("DOMContentLoaded", function () {
    const qrElements = document.querySelectorAll('.ticket-qrcode');
    qrElements.forEach(element => {
        const qrText = element.getAttribute('data-qr-value');
        if (qrText) {
            new QRCode(element, {
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