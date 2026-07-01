$(document).ready(function () {
    document.querySelectorAll('.btn-more-details').forEach(btn => {
        btn.addEventListener('click', async () => {
            const carsId = btn.dataset.carId;
            const result = await fetch(`/Jdm/GetJdmCarsContact?id=${carsId}`);
            const modal = document.getElementById('jdm-contact-modal');
            const message = document.getElementById('jdm-contact-message');
            const phoneLink = document.getElementById('jdm-contact-phone');
            const noPhone = document.getElementById('jdm-contact-no-phone');

            if (!result.ok) {
                message.textContent = 'Не удалось загрузить контакт';
                phoneLink.hidden = true;
                noPhone.hidden = true;
            } else {
                const data = await result.json();
                message.textContent = data.message;

                if (data.hasPhone) {
                    phoneLink.textContent = data.mobilePhone;
                    phoneLink.href = `tel:${data.mobilePhone}`;
                    phoneLink.hidden = false;
                    noPhone.hidden = true;
                } else {
                    phoneLink.hidden = true;
                    noPhone.hidden = false;
                }
            }
            modal.hidden = false;
            modal.classList.add('is-open');
        });
    });

    document.querySelector('.jdm-contact-modal__close')?.addEventListener('click', closeModal);
    document.querySelector('.jdm-contact-modal__overlay')?.addEventListener('click', closeModal);

    function closeModal() {
        const modal = document.getElementById('jdm-contact-modal');
        modal.classList.remove('is-open');
        modal.hidden = true;
    }
});