$(document).ready(function () {
  $("div.food-item").click(function () {
    const self = $(this);

    self.toggleClass("active");

    //const atLeastOneItemForRemove = $("div.food-item.active").length > 0;
  });
});
