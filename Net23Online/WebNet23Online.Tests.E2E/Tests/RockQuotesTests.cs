using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebNet23Online.Tests.E2E.Selectors;

namespace WebNet23Online.Tests.E2E.Tests
{
    public class RockQuotesTests
    {
        private IWebDriver _webDriver;
        private WebDriverWait _waiter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _webDriver = new ChromeDriver();
            _waiter = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5));
        }

        [Test]
        public void CreateRockQuote_Positive()
        {
            string testAuthor = "TEST OZZY " + Guid.NewGuid().ToString().Substring(0, 5);
            string testQuote = "Rock and Roll never dies!";
            string testImgUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR45yqbIKWlWYnj0tVobN1LJHDrpfhi9fOPww&s";

            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/RockLegendsPortal/Quotes");

            var submitBtn = _webDriver.FindElement(RockQuotesPage.SubmitButton);
            _waiter.Until(d => submitBtn.Displayed);

            _webDriver.FindElement(RockQuotesPage.AuthorInput).SendKeys(testAuthor);
            _webDriver.FindElement(RockQuotesPage.UrlInput).SendKeys(testImgUrl);
            _webDriver.FindElement(RockQuotesPage.TextInput).SendKeys(testQuote);

            submitBtn.Click();


            _waiter.Until(d => d.FindElements(RockQuotesPage.QuoteCards).Any());

            var allCards = _webDriver.FindElements(RockQuotesPage.QuoteCards);
            var lastCard = allCards.Last();

            var actualAuthorName = lastCard.FindElement(RockQuotesPage.AuthorNameInCard).Text;

            Assert.That(actualAuthorName.ToUpper(), Is.EqualTo(testAuthor.ToUpper()));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }
    }
}
