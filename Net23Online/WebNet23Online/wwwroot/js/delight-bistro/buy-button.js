document.addEventListener('DOMContentLoaded', function () {
  const buyButtons = document.querySelectorAll('.buy-button');

  buyButtons.forEach((button) => {
    button.addEventListener('click', function () {
      const self = this;
      self.classList.toggle('choose-to-buy');

      const choosenFoodIds = getChosenIds();
      console.log('выбранные id:', choosenFoodIds);
    });
  });
  // список всех кнопки которые выбраны
  // const foodIds = choosenButtons.querySelectorAll('data-food-item-id');

  function getChosenIds() {
    const choosenButtons = document.querySelectorAll(
      '.buy-button.choose-to-buy',
    );

    const ids = [];

    choosenButtons.forEach((button) => {
      // получаем id foodItem по атрибуту кнопки
      const id = button.dataset.foodItemId;
      ids.push(parseInt(id));
    });

    return ids;
  }
});
