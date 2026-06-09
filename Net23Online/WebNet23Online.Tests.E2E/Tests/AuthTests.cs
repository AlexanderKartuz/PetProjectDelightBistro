using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebNet23Online.Tests.E2E.Helper;

namespace WebNet23Online.Tests.E2E.Tests
{
    public class AuthTests
    {
        private IWebDriver _webDriver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Open chrome
            _webDriver = new ChromeDriver();
        }

        [Test]
        public void Login()
        {
            _webDriver.LoginAsAdmin();

            var userNameSpan = _webDriver.FindElement(By.CssSelector(".user-name"));
            var userName = userNameSpan.Text;

            Assert.That(userName == "admin");
        }

        [OneTimeTearDown]
        public void Tear()
        {
            _webDriver.Quit();
        }
    }
}
