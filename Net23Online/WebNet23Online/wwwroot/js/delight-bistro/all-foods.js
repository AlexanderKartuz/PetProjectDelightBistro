$(document).ready(function () {
    $("div.food-item").click(function () {
        const self = $(this);
        self.toggleClass("active");

        const atLeastOneItemIsSelected = $("div.food-item.active").length > 0;

        if (atLeastOneItemIsSelected) {
            $(".remove-food-item").removeAttr("disabled");
        } else {
            $(".remove-food-item").attr("disabled", "disabled");
        }
    });

    $(".remove-food-item").click(function () {
        const ids = [];

        $("div.food-item.active").each((x, item) => {
            const id = $(item).attr("item-id");
            ids.push(id);
        });

        const idsStr = ids.join("&ids=");

        $("div.food-item.active").remove();

        const url = `/api/DelightBistro/delete?ids=${idsStr}`;
        $.get(url);
    });

});
