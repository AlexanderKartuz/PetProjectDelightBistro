using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using WebNet23Online.Tests.E2E.Helper;
using WebNet23Online.Tests.E2E.Selectors;

namespace WebNet23Online.Tests.E2E.Tests
{
    public class DelightBistroCreateFoodTests
    {
        private IWebDriver _webDriver;
        private WebDriverWait _waiter;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _webDriver = new ChromeDriver();
            _waiter = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(6));
        }

        [TestCase("TestName1", "20", 0, "100")]
        public void CreateFoodItem_Positive(string foodName, string foodPrice, int ingredientIndex, string ingredientCount)
        {
            _webDriver.Logout();
            _webDriver.LoginAsAdmin();

            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/DelightBistro/Index");

            //var foodBuilderDataLink = _webDriver.FindElement(DelightBistroIndexPage.FoodBuilderDataLink);
            var foodBuilderDataLink = _waiter.Until(d => d.FindElement(DelightBistroIndexPage.FoodBuilderDataLink));
            foodBuilderDataLink.Click();

            _waiter.Until(d => d.FindElement(DelightBistroBuildFoodItemPage.FoodNameInput).Displayed);

            _webDriver.FindElement(DelightBistroBuildFoodItemPage.FoodNameInput)
                .SendKeys($"{foodName}");

            var foodItemPriceInput = _webDriver
                .FindElement(DelightBistroBuildFoodItemPage.FoodPriceInput);
            foodItemPriceInput.Click();
            foodItemPriceInput.SendKeys($"{foodPrice}");

            //var menuList = _webDriver.FindElement(DelightBistroBuildFoodItemPage.MenuListDropDown);
            _webDriver.FindElement(DelightBistroBuildFoodItemPage.IngredientCheckBox(ingredientIndex))
                .Click();
            _webDriver.FindElement(DelightBistroBuildFoodItemPage.IngredentQuantityInput(ingredientIndex))
                .SendKeys($"{ingredientCount}");

            _webDriver.FindElement(DelightBistroBuildFoodItemPage.FoodImgUrlInput)
                .SendKeys("1");

            var submitButton = _webDriver.FindElement(DelightBistroBuildFoodItemPage.SubmitButton);
            new Actions(_webDriver)
               .ScrollToElement(submitButton)
               .Perform();

            _waiter.Until(d => submitButton.Displayed);

            // Ждем обновления
            _waiter.Until(d => d.FindElement(DelightBistroBuildFoodItemPage.PreviewFoodName).Text == foodName);
            _waiter.Until(d => d.FindElement(DelightBistroBuildFoodItemPage.PreviewPrice).Text == foodPrice);

            var previewFoodName = _webDriver.FindElement(DelightBistroBuildFoodItemPage.PreviewFoodName).Text;
            Assert.That(previewFoodName == foodName);

            var previewPrice = _webDriver.FindElement(DelightBistroBuildFoodItemPage.PreviewPrice).Text;
            Assert.That(previewPrice == foodPrice);

            submitButton.Click();
        }


    }
}
