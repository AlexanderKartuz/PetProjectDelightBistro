$(document).ready(function () {

    const getQuotesUrl = "https://localhost:7042/GetQuotes";
    const createQuoteUrl = "https://localhost:7042/CreateQuote";

    // Запускаем загрузку старых цитат из базы Minimal API
    init();

    $('.create-quote-button').click(function () {
        const name = $('.quote-author-input').val();
        const url = $('.quote-url-input').val();
        const quote_text = $('.quote-text-input').val();

        // ИСПРАВЛЕНО: Добавлены операторы || (ИЛИ)
        if (name.trim() === "" || url.trim() === "" || quote_text.trim() === "") {
        alert("Заполните все поля цитаты!");
        return;
    }

    const data = {
        name: name,
        url: url,
        quote_text: quote_text
    };

    $.ajax({
        type: 'POST',
        url: createQuoteUrl,
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).done(function (quote) {
        drawQuote(quote);

        // Очищаем инпуты
        $('.quote-author-input').val('');
        $('.quote-url-input').val('');
        $('.quote-text-input').val('');
    }).fail(function () {
        alert("Ошибка при сохранении цитаты! Проверь, запущен ли Minimal API на порту 7042 и включен ли там CORS.");
    });
});

function init() {
    $.get(getQuotesUrl)
        .done(function (quotes) {
            quotes.forEach((quote) => {
                drawQuote(quote);
            });
        });
}

function drawQuote(quote) {
    const quotesContainer = $('.quotes-catalog-grid');
    const divForQuote = $('.quote-card-template').clone();

    divForQuote.removeClass('quote-card-template');
    divForQuote.css('display', 'block');

    // ИСПРАВЛЕНО: Свойства приведены к camelCase, как их возвращает сервер
    divForQuote.find('.quote-author').text(quote.name);
    divForQuote.find('.image-container img').attr('src', quote.url);
    divForQuote.find('.quote-text-content').text('"' + quote.quote_text + '"');

    quotesContainer.append(divForQuote);
}
});