using OpenQA.Selenium;

namespace WebNet23Online.Tests.E2E.Selectors.steam
{
    public class AddGamePage
    {
        public static By TitleInput = By.CssSelector("#Title");
        public static By ImageUrlInput = By.CssSelector("#ImageUrl");
        public static By DescriptionInput = By.CssSelector("#Description");
        public static By PriceInput = By.CssSelector("#Price");
        public static By GameGenreSelect = By.CssSelector("#SelectedGenreIds");
        public static By PublisherSelect = By.CssSelector("#PublisherId");
        public static By SubmitButton = By.CssSelector("form[method='post'] button[type=submit]");

        public static By ValidationMessageForImageUrl = By.CssSelector("span[data-valmsg-for='ImageUrl']");
    }
}
