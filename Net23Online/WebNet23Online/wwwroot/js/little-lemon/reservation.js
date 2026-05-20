$(document).ready(function () {
    const imagePreview = $(".image-preview_image");
    const imagePreviewWrapper = $(".image-preview_image__wrapper");
    const imagePreviewBtn = $(".image-preview_btn");
    const imagePreviewContainer = $(".image-preview_container");
    $("#DessertReferencePhoto").on("change", function () {
        const file = this.files[0];
        if (file) {
            imagePreviewContainer.removeClass("hide");
            const reader = new FileReader();
            reader.onload = function () {
                imagePreview.attr("src",reader.result);
            };
            reader.readAsDataURL(file);
        }
    });
    imagePreviewBtn.on("click", function () {
        imagePreviewContainer.addClass("hide");
    });
});
