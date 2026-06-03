
$(document).ready(function ()
{
    const relicsApiUrl = 'https://localhost:7050';
    const grid = $('.spire-relics-grid');
    const template = $('.spire-relic-template');

    init();

    $('.create-relic-button').click(function ()
    {
        const requestUrl = `${relicsApiUrl}/CreatRelic`;
        const name = $('.relic-name-input').val();
        const urlImage = $('.relic-url-input').val();
        const rarity = $('.relic-rarity-input').val();

        const data = { name, urlImage, rarity };

        $.ajax({
            type: 'POST',
            url: requestUrl,
            contentType: 'application/json',
            data: JSON.stringify(data)
        }).done(function (relic)
        {
            drawRelic(relic);
        });
    });

    function init()
    {
        if (!template.length)
        {
            console.error('Relics: шаблон .spire-relic-template не найден на странице.');
            return;
        }

        const url = `${relicsApiUrl}/GetRelics`;
        $.get(url)
            .done(function (relics)
            {
                relics.forEach((relic) =>
                {
                    drawRelic(relic);
                });
            })
            .fail(function (xhr, status, error)
            {
                console.error('Relics: не удалось загрузить GetRelics.', status, error, xhr);
            });
    }

    function drawRelic(relic)
    {
        const card = template.clone();
        card.removeClass('spire-relic-template');
        card.removeAttr('hidden').removeAttr('aria-hidden');
        card.attr('data-id', relic.id);

        const name = relic.name?.trim() || '—';
        const rarity = relic.rarity?.trim() || '—';
        const imageUrl = relic.urlImage?.trim() || '';

        card.find('.spire-relic-card-name').text(name);
        card.find('.spire-relic-card-rarity').text(rarity);

        const image = card.find('.spire-relic-card-image');
        if (imageUrl)
        {
            image.attr('src', imageUrl).attr('alt', name);
        }
        else
        {
            image.remove();
        }

        grid.append(card);
    }
});
