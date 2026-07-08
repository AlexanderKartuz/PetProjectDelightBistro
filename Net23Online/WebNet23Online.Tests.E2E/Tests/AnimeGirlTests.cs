using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using WebNet23Online.Tests.E2E.Helper;
using WebNet23Online.Tests.E2E.Selectors;

namespace WebNet23Online.Tests.E2E.Tests
{
    public class AnimeGirlTests
    {
        private IWebDriver _webDriver;
        private WebDriverWait _waiter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Open chrome
            _webDriver = new ChromeDriver();
            _waiter = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(4));
        }

        [TestCase("Test Olga")]
        [TestCase("Test Nina")]
        [TestCase("Test Zina")]
        public void CreateAnimeGirl_Positive(string newGirlName)
        {
            _webDriver.Logout();
            _webDriver.LoginAsAdmin();

            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/AnimeGirl/Index");

            var link = _webDriver.FindElement(AnimeGirlIndexPage.CreateGilrLink);
            _waiter.Until(d => link.Displayed);
            link.Click();

            _webDriver.FindElement(AnimeGirlCreateGirlPage.NameInput)
                .SendKeys(newGirlName);
            _webDriver.FindElement(AnimeGirlCreateGirlPage.DoubleCheckNameInput)
                .SendKeys(newGirlName);
            _webDriver.FindElement(AnimeGirlCreateGirlPage.DescriptionInput)
                .SendKeys("Smile");
            _webDriver.FindElement(AnimeGirlCreateGirlPage.UrlInput)
                .SendKeys("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR45yqbIKWlWYnj0tVobN1LJHDrpfhi9fOPww&s");
            var sizeInput = _webDriver.FindElement(AnimeGirlCreateGirlPage.SizeInput);
            sizeInput.Clear();
            sizeInput.SendKeys("4");
            _webDriver.FindElement(AnimeGirlCreateGirlPage.SubmitButton)
                .Click();

            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/AnimeGirl/Index?pageSize=0");

            var allGirlBlocks = _webDriver.FindElements(AnimeGirlIndexPage.GirlBlocks);
            
            _waiter.Until(d => allGirlBlocks.Any());

            var firstGirlBlock = allGirlBlocks.First();
            var firstGirlName = firstGirlBlock.FindElement(AnimeGirlIndexPage.NameIntoGirlBlocks).Text;
            Assert.That(newGirlName == firstGirlName);

            var deleteLink = firstGirlBlock.FindElement(AnimeGirlIndexPage.DeleteGilrLinkIntoGirlBlocks);
            new Actions(_webDriver)
               .ScrollToElement(deleteLink)
               .Perform();
            _waiter.Until(d => deleteLink.Displayed);
            deleteLink.Click();
        }

        [Test]
        public void CreateAnimeGirl_GuestForbiden()
        {
            _webDriver.Logout();

            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/AnimeGirl/Index");

            _webDriver.FindElement(AnimeGirlIndexPage.CreateGilrLink).Click();
        }
    }
}
