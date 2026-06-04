
using OpenQA.Selenium;

namespace WebNet23Online.Tests.E2E.Selectors.steam
{
    public class SteamIndexPage
    {
        public static By CatalogLink = By.CssSelector(".catalog-link a");
        public static By LogoutLink = By.CssSelector("a[href*='/Auth/Logout']");
    }
}
