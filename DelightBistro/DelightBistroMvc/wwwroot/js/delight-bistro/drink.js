document.addEventListener('DOMContentLoaded', function () {

    const urlGet = 'https://localhost:7090/GetDrinks';
    const urlPost = 'https://localhost:7090/CreateDrink';

    const teasCatalogDiv = document.querySelector('.teas-catalog');
    const createButton = document.querySelector('.create-button');
    const createTeaForm = document.querySelector('.create-tea-form');
    const teaFormToggle = document.querySelector('.tea-form-toggle');

    getAllDrinks();

    createButton.addEventListener('click', createTea);

    teaFormToggle.addEventListener('click', function () {
        createTeaForm.classList.toggle('hidden');
    });

    function getAllDrinks() {
        fetch(urlGet)
            .then(function (response) {
                if (!response.ok) {
                    throw new Error(`Error: ${response.status}`);
                }
                return response.json();
            })
            .then(function (drink) {
                teasCatalogDiv.innerHTML = '';

                drink.forEach(function (tea) {
                    drawTeaCard(tea);
                });
                console.log('Все чаев: ' + drink.length);
            })
            .catch((error) => {
                console.error('Ошибка загрузки', error);
            });
    }

    // tea - json
    function drawTeaCard(tea) {

        const teaItemDiv = document.createElement('div');
        teaItemDiv.classList.add('food-item');
        teaItemDiv.dataset.foodItemId = tea.id;

        const namePriceDiv = document.createElement('div');
        namePriceDiv.classList.add('name-price');

        const teaNameDiv = document.createElement('div');
        teaNameDiv.classList.add('food-name');
        teaNameDiv.textContent = tea.name;
        namePriceDiv.appendChild(teaNameDiv);

        const priceDiv = document.createElement('div');
        priceDiv.classList.add('price');
        priceDiv.textContent = (tea.price || 0) + ' р';
        namePriceDiv.appendChild(priceDiv);

        teaItemDiv.appendChild(namePriceDiv);
        teasCatalogDiv.appendChild(teaItemDiv);
    }

    function createTea() {
        const nameInput = document.querySelector('.tea-name-input');
        const priceInput = document.querySelector('.tea-price-input');

        const name = nameInput.value.trim();
        const price = parseInt(priceInput.value) || 0;

        if (!name) {
            return;
        }

        const requestBody = {
            name: name,
            price: price,
        };

        fetch(urlPost, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(requestBody),
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`Error: ${response.status}`);
                }

                return response.json();
            })
            .then(data => {
                drawTeaCard(data);
                nameInput.value = '';
                priceInput.value = '';
            })
            .catch((error) => {
                console.error('Ошибка при добавлении', error);
            });

    }

});
