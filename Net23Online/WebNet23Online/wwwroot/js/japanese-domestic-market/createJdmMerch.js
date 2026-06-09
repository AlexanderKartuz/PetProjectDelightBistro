$(document).ready(function () {
    init();

    function init() {
        const url = `https://localhost:7001/GetMerches`;
        $.get(url)
            .done(function (jdmMerches) {
                jdmMerches.forEach((JdmMerchModel) => {
                    drawJdmMerche(JdmMerchModel);
                })
            })
    }

    $('.button-create-new-jdm-merch').click(function () {
        const requestUrl = `https://localhost:7001/CreateMerche`;
        const nameProduct = $('.new-jdm-merch-name').val();
        const description = $('.new-jdm-merch-description').val();
        const price = $('.new-jdm-merch-price').val();
        const url = $('.new-jdm-card-picture').val();

        $.ajax({
            type: "POST",
            url: requestUrl,
            contentType: 'application/json',
            data: JSON.stringify({
                nameProduct: nameProduct,
                description: description,
                price: price,
                url: url   
            })
        }).done(function (JdmMerchModel) {
            drawJdmMerche(JdmMerchModel);
        });
    })

    function drawJdmMerche(JdmMerchModel) {
        const jdmMerchContainer = $('.jdm-merch');
        const divForJdmMerch = $('.jdm-merch-container .product-card.template').clone();
        divForJdmMerch.removeClass('template');
        divForJdmMerch.find('.product-card-title').text(JdmMerchModel.nameProduct);
        divForJdmMerch.find('.product-card-description').text(JdmMerchModel.description);
        divForJdmMerch.find('.product-card-price').text(JdmMerchModel.price);
        divForJdmMerch.find('.product-card-picture img').attr('src', JdmMerchModel.url);
        jdmMerchContainer.append(divForJdmMerch);
    }
});