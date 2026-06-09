using OpenQA.Selenium;

namespace WebNet23Online.Tests.E2E.Selectors
{
    public class AnimeGirlCreateGirlPage
    {
        public static By NameInput = By.CssSelector("#Name");
        public static By DoubleCheckNameInput = By.CssSelector("#DoubleCheckName");
        public static By DescriptionInput = By.CssSelector("#Description");
        public static By UrlInput = By.CssSelector("#Url");
        public static By SizeInput = By.CssSelector("#Size");
        public static By SubmitButton = By.CssSelector("button[type=submit]");
    }
}
