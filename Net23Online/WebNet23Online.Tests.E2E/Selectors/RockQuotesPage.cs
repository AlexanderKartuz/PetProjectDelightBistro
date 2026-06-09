using OpenQA.Selenium;

namespace WebNet23Online.Tests.E2E.Selectors
{
    public static class RockQuotesPage
    {
        public static By AuthorInput = By.CssSelector(".quote-author-input");
        public static By UrlInput = By.CssSelector(".quote-url-input");
        public static By TextInput = By.CssSelector(".quote-text-input");
        public static By SubmitButton = By.CssSelector(".create-quote-button");
        public static By QuoteCards = By.CssSelector(".quotes-catalog-grid .rockStar");
        public static By AuthorNameInCard = By.CssSelector(".quote-author");
        public static By TextInCard = By.CssSelector(".quote-text-content");
    }
}