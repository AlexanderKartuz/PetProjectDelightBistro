document.addEventListener('DOMContentLoaded', function () {
    const buyButtons = document.querySelectorAll('.buy-button');
    const counterDisplay = document.querySelector('.counter-display');
    const orderBox = document.querySelector('.order-box');
    const orderList = document.querySelector('#orderList');
    const totalPriceDiv = document.querySelector('.total-price');
    const postOrderButton = document.querySelector('#post-order-btn');

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

        // Очистка списка
        const oldItems = orderList.querySelectorAll('.order-food-item');
        oldItems.forEach((item) => item.remove());

        chossenItems.forEach((item) => {
            const li = document.createElement('li');
            li.className = 'order-food-item';
            li.textContent = `${item.name} - ${item.price}`;
            orderList.appendChild(li);
        });

        const totalPrice = chossenItems.reduce(
            (summ, item) => summ + item.price,
            0,
        );

        if (totalPriceDiv) {
            totalPriceDiv.textContent = `Общая цена ${totalPrice} BYN`;
        }

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
            // тип number для связи с бд, запарсить в число?
            const id = button.dataset.foodItemId;
            const foodItem = button.closest('.food-item');

            const foodItemName = foodItem.querySelector('.food-name');
            const name = foodItemName ? foodItemName.textContent.trim() : 'No name';

            // запарсить в число?
            const price = parseInt(button.dataset.foodItemPrice, 10) || 0;

            orderFoodItems.push({ id, name, price });
        });

        return orderFoodItems;
    }
});
