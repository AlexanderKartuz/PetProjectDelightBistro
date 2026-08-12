using OpenQA.Selenium;

namespace DelightBistroMvc.Tests.E2E.Selectors
{
    public class DelightBistroBuildFoodItemPage
    {
        public static By FoodNameInput = By.CssSelector("[data-test='food-name']");
        public static By FoodPriceInput = By.CssSelector("[data-test='food-price']");
        public static By MenuListDropDown = By.CssSelector("[data-test='menu-list']");
        public static By IngredientCheckBox(int index) => By.CssSelector($"[data-test='ingredient-checked-box-{index}']");
        public static By IngredentQuantityInput(int index) => By.CssSelector($"[data-test='ingredient-quantity-{index}']");
        public static By FoodImgUrlInput = By.CssSelector("[data-test='imgUrl']");
        public static By SubmitButton = By.CssSelector("[data-test='submit-button']");

        public static By PreviewImage = By.CssSelector(".preview-image-container");
        public static By PreviewFoodName = By.CssSelector(".preview-food-name");
        public static By PreviewMenuName = By.CssSelector(".preview-menu-name");
        public static By PreviewPrice = By.CssSelector(".preview-price");
    }
}
