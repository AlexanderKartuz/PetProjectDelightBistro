
$(document).ready(function ()
{
    const previewName = $('.js-preview-name');
    const previewMana = $('.js-preview-mana');
    const previewType = $('.js-preview-type');
    const previewDescription = $('.js-preview-description');
    const previewRarity = $('.js-preview-rarity');
    const previewUpgraded = $('.js-preview-upgraded');
    const previewUpgradedBadge = $('.js-preview-upgraded-badge');
    const previewImage = $('.js-preview-image');

    function updatePreview()
    {
        const name = $('#Name').val()?.trim() || '—';
        const mana = $('#ManaCost').val();
        const typeText = $('#TypeOfCard option:selected').text()?.trim();
        const rarityText = $('#Rarity option:selected').text()?.trim();
        const description = $('#Description').val()?.trim();
        const imageUrl = $('#ImageUrl').val()?.trim();
        const isUpgraded = $('#Upgraded').is(':checked');

        previewName.text(name);
        previewMana.text(mana === '' || mana == null ? '0' : mana);

        if (typeText) 
        {
            previewType.text(typeText);
        } 
        else 
        {
            previewType.text('');
        }

        if (description) 
        {
            previewDescription.text(description);
        } 
        else
        {
            previewDescription.text('');
        }

        previewRarity.text(rarityText || '—');

        if (isUpgraded) 
        {
            previewUpgradedBadge.removeAttr('hidden');
            previewUpgraded.text('Да');
        } 
        else 
        {
            previewUpgradedBadge.attr('hidden', 'hidden');
            previewUpgraded.text('Нет');
        }

        if (imageUrl) 
        {
            previewImage.attr('src', imageUrl).attr('alt', name).removeAttr('hidden');
        } 
        else 
        {
            previewImage.attr('src', '').attr('alt', '').attr('hidden', 'hidden');
        }
    }

    $('#Name, #Description, #ImageUrl, #ManaCost').on('input', updatePreview);
    $('#TypeOfCard, #Rarity, #Upgraded').on('change', updatePreview);

    updatePreview();
});
