$(document).ready(function () {
    $('#Name').on('input', function () {
        const newName = $(this).val();
        if (newName.trim() === "") {
            $('.preview-genre-title').text("Название жанра");
        } else {
            $('.preview-genre-title').text(newName);
        }
    });

    $('#CoverUrl').on('input', function () {
        const newUrl = $(this).val();
        if (newUrl.trim() !== "") {
            $('.preview-genre-img').attr('src', newUrl);
        }
    });

    $('#Image').on('change', function (event) {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $('.preview-genre-img').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });
    $('#Name').on('blur', function () { 
        const genreName = $(this).val();
        const msgElement = $('#genre-name-validation-msg');
        const submitBtn = $('button[type="submit"]');

        if (genreName.trim() === "")
        {
            return;
        }

        $.ajax({
            url: '/api/rock-legends/validate-genre',
            type: 'GET',
            data: { name: genreName },
            success: function (response) {
                if (!response.isValid) {
                    if (msgElement.length === 0) {
                        $('#Name').after('<span id="genre-name-validation-msg" style="color: #ff4d4d; display:block; margin-top:5px;">' + response.message + '</span>');
                    } else {
                        msgElement.text(response.message).show();
                    }
                    $('#Name').css('border-color', '#ff4d4d');
                    submitBtn.attr('disabled', 'disabled'); 

                    if (msgElement.length > 0) msgElement.hide();
                    $('#Name').css('border-color', '#00ffcc');
                    submitBtn.removeAttr('disabled'); 
                }
            }
        });
    });
});