$(document).ready(function () {

    const $bandName = $('#band-name');
    if ($bandName.length) {
        const $nameBlock = $bandName.closest('.name-block');
        const $submitBtn = $bandName.closest('form').find('button[type="submit"]');

        $bandName.on('change', function () {
            $nameBlock.find('.icon').hide();
            $nameBlock.find('.icon.wait').show();

            $bandName.removeClass('free used');
            $submitBtn.removeAttr('disabled');

            const bandName = $bandName.val();
            const url = `/api/RockBands/IsBandNameFree?name=${encodeURIComponent(bandName)}`;

            $.get(url)
                .done(function (answer) {
                    $nameBlock.find('.icon.wait').hide();
                    if (answer) {
                        $bandName.addClass('free');
                        $nameBlock.find('.icon.ok').show();
                    } else {
                        $bandName.addClass('used');
                        $nameBlock.find('.icon.deny').show();
                        $submitBtn.attr('disabled', 'disabled');
                    }
                });
        });
    }

    $(".band-list .band").click(function (e) {
        if ($(e.target).closest(".band-likes").length) {
            return;
        }

        const self = $(this);
        self.toggleClass("active");

        const atLeastOneSelected = $(".band-list .band.active").length > 0;
        $(".band-list .remove-band").prop("disabled", !atLeastOneSelected);
    });

    $(".band-list .remove-band").click(function () {
        $(".band-list .band.active").remove();
        $(this).prop("disabled", true);
    });

    $(".band-list").on("click", ".band-like-btn:not(:disabled)", function (e) {
        e.stopPropagation();
    
        const $btn = $(this);
        const bandId = $btn.data("band-id");
        const $count = $btn.siblings(".band-like-count");
    
        $.post(`/api/RockBands/AddLike?bandId=${bandId}`)
            .done(function (result) {
                $count.text(result.likeCount);
                if (result.liked) {
                    $btn.addClass("liked").prop("disabled", true);
                }
            })
            .fail(function () {
                alert("Не удалось поставить лайк");
            });
    });
});
