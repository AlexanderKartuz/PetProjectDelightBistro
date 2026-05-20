$(document).ready(function () {

    $(".band-list .band").click(function () {
        const self = $(this);
        self.toggleClass("active");

        const atLeastOneSelected = $(".band-list .band.active").length > 0;
        $(".band-list .remove-band").prop("disabled", !atLeastOneSelected);
    });

    $(".band-list .remove-band").click(function () {
        $(".band-list .band.active").remove();
        $(this).prop("disabled", true);
    });
});
