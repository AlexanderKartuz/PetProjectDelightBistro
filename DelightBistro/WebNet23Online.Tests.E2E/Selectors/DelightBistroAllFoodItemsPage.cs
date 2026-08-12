using OpenQA.Selenium;

namespace DelightBistroMvc.Tests.E2E.Selectors
{
    public class DelightBistroAllFoodItemsPage
    {
        public static By FoodItem = By.CssSelector(".menu .food-item");
        public static By FoodName = By.CssSelector(".food-name");
        public static By FoodPrice = By.CssSelector(".price");
        public static By DeleteFoodLinkInto = By.CssSelector(".delete-food-item");
    }
}
