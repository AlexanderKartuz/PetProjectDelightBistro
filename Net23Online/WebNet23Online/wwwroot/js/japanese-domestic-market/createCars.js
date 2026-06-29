$(document).ready(function () {
    function updateCard() {
        const marka = $('#Marka').val();
        const model = $('#Model').val();
        const price = $('#Price').val();
        const url = $('#Url').val();

        $('#preview-title').text(`${marka} ${model}`.trim() || '***');
        $('#preview-price').text(price || '***');
        $('#preview-img').attr({ src: url, alt: marka });
    }

    $('#Marka, #Model, #Price, #Url').on('input', updateCard);
    $('#ManufactureId').on('change', updateCard);
});