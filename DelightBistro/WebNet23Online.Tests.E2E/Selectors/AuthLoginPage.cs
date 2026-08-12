using OpenQA.Selenium;

namespace DelightBistroMvc.Tests.E2E.Selectors
{
    public static class AuthLoginPage
    {
        public static By LoginInput = By.CssSelector("#Login");
        public static By PasswordInput = By.CssSelector("#Password");
        public static By SubmitButton = By.CssSelector("button[type=submit]");
    }
}
