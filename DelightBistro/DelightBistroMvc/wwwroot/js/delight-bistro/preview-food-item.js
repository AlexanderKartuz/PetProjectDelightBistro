$(document).ready(function () {
  $('#Name').on('change', function () {
    const newName = $('#Name').val();
    $('.preview-image-container img').attr('alt', `${newName}`);
    $('.preview-food-name').text(newName);
  });

  $('#ImgURL').on('change', function () {
    const newUrl = $('#ImgURL').val();
    $('.preview-image-container img').attr('src', newUrl);
  });

  $('#MenuId').on('change', function () {
    const newMenuName = $('#MenuId option:selected').text();
    $('.preview-menu-name').text(newMenuName);
  });

  $('#Price').on('change', function () {
    const newPrice = $('#Price').val();
    $('.preview-price').text(newPrice);
  });

  function updateIngredientsPreview() {
    const previewSelectedList = $('.preview-recept ul');
    previewSelectedList.empty();

    $('input[type="checkbox"][name*="IsSelected"]:checked').each(function () {
      const ingredientName = $(this).data('ingredient-name');
      previewSelectedList.append(`<li> ${ingredientName}</li>`);
    });
  }

  $(document).on(
    'change',
    'input[type=checkbox][name*="IsSelected"]',
    updateIngredientsPreview,
  );
});
