
using OpenQA.Selenium;

namespace WebNet23Online.Tests.E2E.Selectors
{
    public class DelightBistroIndexPage
    {
        public static By CreateMenuLink = By.CssSelector("[data-test='create-menu']");
        public static By CreateIngredientLink = By.CssSelector("[data-test='create-ingredient]'");
        public static By FoodBuilderDataLink = By.CssSelector("[data-test='food-builder']");
        public static By AllFoodItemsLink = By.CssSelector("[data-test='all-foods']");
    }
}
