document.addEventListener('DOMContentLoaded', function () {
  const buyButtons = document.querySelectorAll('.buy-button');
  const counterDisplay = document.querySelector('.counter-display');
  const orderBox = document.querySelector('.order-box');
  const orderList = document.querySelector('#orderList');
  const totalPriceDiv = document.querySelector('.total-price');
  const postOrderButton = document.querySelector('#post-order-btn');
  const orderSuccessResponse = document.querySelector('#order-success');

  buyButtons.forEach((button) => {
    button.addEventListener('click', function () {
      const self = this;
      self.classList.toggle('choose-to-buy');

      updateOrderBox();
    });
  });

  function updateOrderBox() {
    const chosenItems = getChosenItems();
    counterDisplay.textContent = chosenItems.length;

    // Очистка списка
    const oldItems = orderList.querySelectorAll('.order-food-item');
    oldItems.forEach((item) => item.remove());

    chosenItems.forEach((item) => {
      const li = document.createElement('li');
      li.className = 'order-food-item';
      li.textContent = `${item.name} - ${item.price}`;
      orderList.appendChild(li);
    });

    const totalPrice = chosenItems.reduce((summ, item) => summ + item.price, 0);

    if (totalPriceDiv) {
      totalPriceDiv.textContent = `Общая цена ${totalPrice} BYN`;
    }

    if (chosenItems.length > 0) {
      orderBox.classList.remove('hidden');
    } else {
      orderBox.classList.add('hidden');
    }
    console.log('Выбранные элементы:', chosenItems);
  }

  function getChosenItems() {
    const chosenButtons = document.querySelectorAll(
      '.buy-button.choose-to-buy',
    );
    const orderFoodItems = [];

    chosenButtons.forEach((button) => {
      // тип number для связи с бд, запарсить в число?
      const id = button.dataset.foodItemId;
      const foodItem = button.closest('.food-item');

      const foodItemName = foodItem.querySelector('.food-name');
      const name = foodItemName ? foodItemName.textContent.trim() : 'No name';

      const price = parseInt(button.dataset.foodItemPrice, 10) || 0;

      orderFoodItems.push({ id, name, price });
    });

    return orderFoodItems;
  }

  postOrderButton.addEventListener('click', function () {
    const chosenItems = getChosenItems();

    // json key
    const requestBody = {
      foodItemIds: chosenItems.map((item) => parseInt(item.id, 10)),
    };

    console.log('Отправка запроса', requestBody);

    fetch('/api/DelightBistro/CreateOrder', {
      method: 'Post',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(requestBody),
    })
      .then((response) => {
        //Authorize
        if (!response.ok) {
          throw new Error(`Error: ${response.status}`);
        }
        return response.json(); //Показать ответ
      })
      .then((data) => {
        console.log('Ответ сервера, заказ создан:', data);

        // закрытие панели заказов
        const chosenButtons = document.querySelectorAll(
          '.buy-button.choose-to-buy',
        );
        chosenButtons.forEach((button) => {
          button.classList.remove('choose-to-buy');
        });

        updateOrderBox();
        showOrderSuccess(data);
      })
      .catch((error) => {
        console.error('Ошибка при заказе', error);
      });
  });

  function showOrderSuccess(data) {
    document.querySelector('#order-success-message').textContent =
      `${data.message}`;
    document.querySelector('#order-success-order-id').textContent =
      `Id заказа: ${data.orderId}`;
    document.querySelector('#order-success-time').textContent =
      `Время создания заказа: ${data.createdTime}`;
    document.querySelector('#order-success-price').textContent =
      `Стоимость заказа: ${data.totalPrice} BYN`;
    orderSuccessResponse.classList.remove('hidden');
  }
});
