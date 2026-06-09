using OpenQA.Selenium;

namespace WebNet23Online.Tests.E2E.Selectors
{
    public class AnimalWorldBookZoo
    {
        public static By ZooBookLink = By.CssSelector(".zoo-book");
        public static By ZooBookButton = By.CssSelector(".zoo-book-button");
        public static By ZooSuccessfulBook = By.CssSelector(".book-successful");
    }
}
