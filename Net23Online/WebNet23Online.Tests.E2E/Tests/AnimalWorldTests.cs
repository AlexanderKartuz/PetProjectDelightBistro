using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebNet23Online.Tests.E2E.Helper;
using WebNet23Online.Tests.E2E.Selectors;

namespace WebNet23Online.Tests.E2E.Tests
{
    public class AnimalWorldTests
    {
        private IWebDriver _driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _driver = new ChromeDriver();
        }

        [Test]
        public void BookZoo_Positive()
        {
            _driver.Logout();
            _driver.LoginAsAdmin();
            var reservation = TryToBook();
            Assert.That(reservation != null);
        }

        [Test]
        public void BookZoo_Negative()
        {
            _driver.Logout();
            _driver.LoginAsUser();
            var reservation = TryToBook();
            Assert.That(reservation == null);
        }

        private IWebElement TryToBook()
        {
            _driver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/AnimalWorld/Index");
            var zooBook = _driver.FindElement(AnimalWorldBookZoo.ZooBookLink);
            zooBook.Click();
            zooBook = _driver.FindElement(AnimalWorldBookZoo.ZooBookButton);
            zooBook.Click();
            return _driver.FindElements(AnimalWorldBookZoo.ZooSuccessfulBook).FirstOrDefault();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _driver.Quit();
        }
    }
}
