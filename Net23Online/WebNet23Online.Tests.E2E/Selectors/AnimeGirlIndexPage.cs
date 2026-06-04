using OpenQA.Selenium;

namespace WebNet23Online.Tests.E2E.Selectors
{
    public class AnimeGirlIndexPage
    {
        public static By CreateGilrLink = By.CssSelector(".create-gilr-link");

        public static By GirlBlocks = By.CssSelector(".section-heroes .media-card");
        public static By NameIntoGirlBlocks = By.CssSelector(".media-card__name");
        public static By DeleteGilrLinkIntoGirlBlocks = By.CssSelector(".delete-girl-link");
    }
}
