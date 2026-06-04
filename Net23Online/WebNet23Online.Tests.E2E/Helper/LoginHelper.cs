using OpenQA.Selenium;
using WebNet23Online.Tests.E2E.Selectors;

namespace WebNet23Online.Tests.E2E.Helper
{
    public static class LoginHelper
    {
        public static void Login(this IWebDriver _webDriver, string login, string password)
        {
            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/Auth/Login");

            var loginInput = _webDriver.FindElement(AuthLoginPage.LoginInput);
            loginInput.SendKeys(login);

            var passwordInput = _webDriver.FindElement(AuthLoginPage.PasswordInput);
            passwordInput.SendKeys(password);

            _webDriver
                .FindElement(AuthLoginPage.SubmitButton)
                .Click();
        }

        public static void LoginAsAdmin(this IWebDriver _webDriver)
        {
            Login(_webDriver, "admin", "admin");
        }

        public static void Logout(this IWebDriver _webDriver)
        {
            _webDriver.Navigate().GoToUrl($"{GlobalConstants.BASE_URL}/Auth/Logout");
        }

        public static void LoginAsUser(this IWebDriver _webDriver)
        {
            Login(_webDriver, "user", "user");
        }
    }
}
