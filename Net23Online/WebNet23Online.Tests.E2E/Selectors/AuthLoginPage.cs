using OpenQA.Selenium;

namespace WebNet23Online.Tests.E2E.Selectors
{
    public static class AuthLoginPage
    {
        public static By LoginInput = By.CssSelector("#Login");
        public static By PasswordInput = By.CssSelector("#Password");
        public static By SubmitButton = By.CssSelector("button[type=submit]");
    }
}
