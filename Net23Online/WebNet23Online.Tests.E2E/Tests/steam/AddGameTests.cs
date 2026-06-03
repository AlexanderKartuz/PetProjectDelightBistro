using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebNet23Online.Data;

using WebNet23Online.Tests.E2E.Helper;
using WebNet23Online.Tests.E2E.Selectors.steam;

namespace WebNet23Online.Tests.E2E.Tests.steam
{
    public class AddGameTests
    {
        private IWebDriver _webDriver;
        private WebDriverWait _waiter;
        private string _gameTitle = string.Empty;
        private string _gamePrice = "49.99";
        private string _gameDescription = "Test description";
        private const string NOT_VALID_IMAGE_URL = "https://google.com/some-page";
        private const string VALID_IMAGE_URL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1237950/header.jpg";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _webDriver = new ChromeDriver();
            _waiter = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(8));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _webDriver.Quit();
        }

        [TearDown]
        public void TearDown()
        {
            if (!string.IsNullOrEmpty(_gameTitle))
            {
                _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/Steam/Catalog");
                _waiter.Until(d => d.FindElements(SteamCatalogPage.GameCardTitles).Any());

                GoToLastPage();

                var gameCards = _webDriver.FindElements(By.CssSelector(".card"));
                IWebElement targetCard = null;

                foreach (var card in gameCards)
                {
                    var titleElement = card.FindElement(SteamCatalogPage.GameCardTitle);
                    if (titleElement.Text == _gameTitle)
                    {
                        targetCard = card;
                        break;
                    }
                }

                if (targetCard != null)
                {
                    targetCard.Click();

                    _waiter.Until(d =>
                    {
                        var el = d.FindElement(SteamCatalogPage.DeleteGameButton);
                        return el.Displayed && el.Enabled;
                    });
                    
                    var deleteButton = _webDriver.FindElement(SteamCatalogPage.DeleteGameButton);
                    deleteButton.Click();
                }
            }
        }

        [Test]
        public void AddGame_Positive()
        {
            _gameTitle = $"E2E Star Wars {Guid.NewGuid():N}";

            _webDriver.Logout();
            _webDriver.LoginAsAdmin();

            _waiter.Until(d => d.FindElements(By.CssSelector("a[href*='/Auth/Logout']")).Any());

            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/Steam/Index");

            _waiter.Until(d =>
            {
                var el = d.FindElement(SteamIndexPage.CatalogLink);
                return el.Displayed && el.Enabled;
            });

            _webDriver.FindElement(SteamIndexPage.CatalogLink).Click();

            _waiter.Until(d => d.Url.Contains("/Steam/Catalog", StringComparison.OrdinalIgnoreCase));

            _waiter.Until(d =>
            {
                var el = d.FindElement(SteamCatalogPage.AddGameButton);
                return el.Displayed && el.Enabled;
            });

            _webDriver.FindElement(SteamCatalogPage.AddGameButton).Click();

            _waiter.Until(d => d.Url.Contains("/Steam/AddGame", StringComparison.OrdinalIgnoreCase));

            _waiter.Until(d =>
            {
                var titleInput = d.FindElement(AddGamePage.TitleInput);
                return titleInput.Displayed && titleInput.Enabled;
            });

            _webDriver.FindElement(AddGamePage.TitleInput).SendKeys(_gameTitle);
            _webDriver.FindElement(AddGamePage.ImageUrlInput)
                .SendKeys(VALID_IMAGE_URL);
            _webDriver.FindElement(AddGamePage.DescriptionInput)
                .SendKeys(_gameDescription);

            var priceInput = _webDriver.FindElement(AddGamePage.PriceInput);
            priceInput.Clear();
            priceInput.SendKeys(_gamePrice);

            var genreSelect = new SelectElement(
                _webDriver.FindElement(AddGamePage.GameGenreSelect));

            genreSelect.SelectByText("Action");
            genreSelect.SelectByText("Indie");

            var publisherSelect = new SelectElement(
                _webDriver.FindElement(AddGamePage.PublisherSelect));

            if (publisherSelect.Options.Count > 1)
            {
                publisherSelect.SelectByIndex(1);
            }

            _waiter.Until(d =>
            {
                var el = d.FindElement(AddGamePage.SubmitButton);
                return el.Displayed && el.Enabled;
            });

            _webDriver.FindElement(AddGamePage.SubmitButton).Click();

            _waiter.Until(d => d.Url.Contains("/Steam/Catalog", StringComparison.OrdinalIgnoreCase));

            GoToLastPage();

            _waiter.Until(d =>
                d.FindElements(SteamCatalogPage.GameCardTitles)
                 .Any(e => e.Text == _gameTitle));

            var addedGameTitle = _webDriver
                .FindElements(SteamCatalogPage.GameCardTitles)
                .First(e => e.Text == _gameTitle)
                .Text;

            Assert.That(addedGameTitle, Is.EqualTo(_gameTitle));
        }

        [Test]
        public void AddGame_WithInvalidImageUrl_ShouldShowValidationMessage_GameNotAdded()
        {
            _gameTitle = "E2E Test";
            _webDriver.Logout();
            _webDriver.LoginAsAdmin();

            _waiter.Until(d => d.FindElements(By.CssSelector("a[href*='/Auth/Logout']")).Any());

            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/Steam/AddGame");

            _waiter.Until(d =>
            {
                var title = d.FindElement(AddGamePage.TitleInput);
                return title.Displayed && title.Enabled;
            });

            _webDriver.FindElement(AddGamePage.TitleInput)
                .SendKeys(_gameTitle);

            _webDriver.FindElement(AddGamePage.ImageUrlInput)
                .SendKeys(NOT_VALID_IMAGE_URL);

            _webDriver.FindElement(AddGamePage.DescriptionInput)
                .SendKeys(_gameDescription);

            var price = _webDriver.FindElement(AddGamePage.PriceInput);
            price.Clear();
            price.SendKeys(_gamePrice);

            var genreSelect = new SelectElement(_webDriver.FindElement(AddGamePage.GameGenreSelect));
            genreSelect.SelectByText("Action");

            var submitButton = _webDriver.FindElement(AddGamePage.SubmitButton);
            submitButton.Click();

            _waiter.Until(_ =>
            {
                try
                {
                    var el = _webDriver.FindElement(AddGamePage.ValidationMessageForImageUrl);
                    return el.Displayed && el.Text.Length > 0;
                }
                catch
                {
                    return false;
                }
            });

            //_waiter.Until(d =>
            //{
            //    var msg = d.FindElements(By.CssSelector("span[data-valmsg-for='ImageUrl']"));
            //    return msg.Any() && msg.First().Displayed;
            //});

            var validationMessage = _webDriver.FindElement(AddGamePage.ValidationMessageForImageUrl);
            Assert.That(validationMessage.Text, Is.Not.Empty);

            var options = new DbContextOptionsBuilder<WebContext>()
                  .UseSqlServer(GlobalConstants.DB_CONNECTION_STRING)
                  .Options;

            using (var context = new WebContext(options))
            {
                var gameInDb = context.Games.FirstOrDefault(g => g.Title == _gameTitle);
                Assert.That(gameInDb, Is.Null, $"Game '{_gameTitle}' should not exist in database");
            }

            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/Steam/Catalog");
            GoToLastPage();
            _waiter.Until(d => d.FindElements(SteamCatalogPage.GameCardTitles).Any());

            var exists = _webDriver
                .FindElements(SteamCatalogPage.GameCardTitles)
                .Any(e => e.Text == _gameTitle);

            Assert.That(exists, Is.False);
        }

        private void GoToLastPage()
        {
            if (!_webDriver.FindElements(SteamCatalogPage.CatalogPagination).Any())
                return;

            var pageLinks = _webDriver.FindElements(SteamCatalogPage.CatalogPageLinks)
                .Where(link => int.TryParse(link.Text.Trim(), out _))
                .ToList();

            if (pageLinks.Any())
            {
                var lastPageLink = pageLinks.Last();
                lastPageLink.Click();

                _waiter.Until(d => d.FindElements(SteamCatalogPage.GameCardTitles).Any());
            }
        }
    }
}
