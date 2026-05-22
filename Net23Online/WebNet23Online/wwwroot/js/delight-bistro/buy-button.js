document.addEventListener('DOMContentLoaded', function () {
  const buyButtons = document.querySelectorAll('.buy-button');
  const counterDisplay = document.querySelector('.counter-display');
  const orderBox = document.querySelector('.order-box');
  const orderList = document.querySelector('#orderList');

  buyButtons.forEach((button) => {
    button.addEventListener('click', function () {
      const self = this;
      self.classList.toggle('choose-to-buy');

      updateOrderBox();
    });
  });

  function updateOrderBox() {
    const chossenItems = getChosenItems();
    counterDisplay.textContent = chossenItems.length;
    const oldItemNames = orderList.querySelectorAll('.order-food-name');
    oldItemNames.forEach((item) => item.remove());

    chossenItems.forEach((item) => {
      const li = document.createElement('li');
      li.className = 'order-food-name';
      li.textContent = item.name;
      orderList.appendChild(li);
    });

    if (chossenItems.length > 0) {
      orderBox.classList.remove('hidden');
    } else {
      orderBox.classList.add('hidden');
    }
    console.log(chossenItems);
  }

  function getChosenItems() {
    const choosenButtons = document.querySelectorAll(
      '.buy-button.choose-to-buy',
    );
    const orderFoodItems = [];

    choosenButtons.forEach((button) => {
      const id = button.dataset.foodItemId;
      const foodItem = button.closest('.food-item');
      const foodItemName = foodItem.querySelector('.food-name');
      const name = foodItemName ? foodItemName.textContent.trim() : 'No name';

      orderFoodItems.push({ id, name });
    });

    return orderFoodItems;
  }
});
