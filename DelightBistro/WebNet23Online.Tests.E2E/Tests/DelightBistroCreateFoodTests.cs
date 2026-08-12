using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using DelightBistroMvc.Tests.E2E.Helper;
using DelightBistroMvc.Tests.E2E.Path;
using DelightBistroMvc.Tests.E2E.Selectors;

namespace DelightBistroMvc.Tests.E2E.Tests
{
    public class DelightBistroCreateFoodTests
    {
        private IWebDriver _webDriver;
        private WebDriverWait _waiter;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _webDriver = new ChromeDriver();
            _waiter = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(4));
        }

        [TestCase("TestName1", "20", 0, "100")]
        public void CreateFoodItem_Positive(string foodName, string foodPrice, int ingredientIndex, string ingredientCount)
        {
            _webDriver.Logout();
            Thread.Sleep(100);
            _webDriver.LoginAsAdmin();
            Thread.Sleep(100);
            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}{DelightBistroPaths.Index}");

            var foodBuilderDataLink = _waiter.Until(d => d.FindElement(DelightBistroIndexPage.FoodBuilderDataLink));
            new Actions(_webDriver)
               .ScrollToElement(foodBuilderDataLink)
               .Perform();
            _waiter.Until(d => foodBuilderDataLink.Displayed);
            foodBuilderDataLink.Click();

            _waiter.Until(d => d.FindElement(DelightBistroBuildFoodItemPage.FoodNameInput).Displayed);

            _webDriver.FindElement(DelightBistroBuildFoodItemPage.FoodNameInput)
                .SendKeys($"{foodName}");

            var foodItemPriceInput = _webDriver
                .FindElement(DelightBistroBuildFoodItemPage.FoodPriceInput);
            foodItemPriceInput.Clear();
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

            var allFoodItemsLink = _waiter.Until(d => d.FindElement(DelightBistroIndexPage.AllFoodItemsLink));
            new Actions(_webDriver)
               .ScrollToElement(allFoodItemsLink)
               .Perform();
            _waiter.Until(d => allFoodItemsLink.Displayed);
            allFoodItemsLink.Click();

            var foodItems = _webDriver.FindElements(DelightBistroAllFoodItemsPage.FoodItem);

            _waiter.Until(d => foodItems.Any());

            var lastFoodItem = foodItems.Last();
            var lastFoodName = lastFoodItem.FindElement(DelightBistroAllFoodItemsPage.FoodName).Text;
            var lastFoodPrice = lastFoodItem.FindElement(DelightBistroAllFoodItemsPage.FoodPrice).Text;

            Assert.That(lastFoodName == foodName);
            Assert.That(lastFoodPrice, Does.Contain($"{foodPrice}."));

            var deleteLink = lastFoodItem.FindElement(DelightBistroAllFoodItemsPage.DeleteFoodLinkInto);
            new Actions(_webDriver)
               .ScrollToElement(deleteLink)
               .Perform();
            _waiter.Until(d => deleteLink.Displayed);
            Thread.Sleep(100);
            deleteLink.Click();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _webDriver.Quit();
        }

    }
}
