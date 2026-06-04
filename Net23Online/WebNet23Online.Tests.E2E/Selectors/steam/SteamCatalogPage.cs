using OpenQA.Selenium;

namespace WebNet23Online.Tests.E2E.Selectors.steam
{
    public class SteamCatalogPage
    {
        public static By AddGameButton = By.CssSelector(".add-game-button");
        public static By GameCardTitles = By.CssSelector(".cards .card-title");
        public static By CatalogPagination = By.CssSelector(".catalog-pagination");
        public static By CatalogActivePage = By.CssSelector(".catalog-pagination .btn.active");
        public static By CatalogPageLinks = By.CssSelector(".catalog-pagination a.btn");
        public static By GameCardTitle = By.CssSelector(".card-title");
        public static By GameCard = By.CssSelector(".card");
        public static By DeleteGameButton = By.CssSelector(".remove-game-card");
    }
}