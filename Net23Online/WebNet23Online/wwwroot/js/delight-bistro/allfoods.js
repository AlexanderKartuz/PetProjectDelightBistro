$(document).ready(function () {
  $("div.food-item").click(function () {
    const self = $(this);
    self.toggleClass("active");

    const atLeastOneItemIsSelected = $("div.food-item.active").length > 0;

    if (atLeastOneItemIsSelected) {
      $(".remove-food-item").removeAttr("disabled");
    } else {
      $(".remove-food-item").attr("disabled");
    }
  });
  $(".remove-food-item").click(function () {
    $("div.food-item.active").remove();
  });
});
