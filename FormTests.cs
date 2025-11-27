using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutomationTests
{
    public class FormTests
    {
        private IWebDriver _driver;
        private const string Url = "https://app.cloudqa.io/home/AutomationPracticeForm";

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            // options.AddArgument("--headless"); // Uncomment to run headless
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void Test_FillFormFields_Robustly()
        {
            _driver.Navigate().GoToUrl(Url);

            IWebElement firstNameInput = GetInputByLabel("First Name");
            IWebElement lastNameInput = GetInputByLabel("Last Name");
            IWebElement emailInput = GetInputByLabel("Email");

            FillField(firstNameInput, "John");
            FillField(lastNameInput, "Doe");
            FillField(emailInput, "john.doe@example.com");

            Assert.That(firstNameInput.GetAttribute("value"), Is.EqualTo("John"), "First Name should be 'John'");
            Assert.That(lastNameInput.GetAttribute("value"), Is.EqualTo("Doe"), "Last Name should be 'Doe'");
            Assert.That(emailInput.GetAttribute("value"), Is.EqualTo("john.doe@example.com"), "Email should be 'john.doe@example.com'");
        }

        private void FillField(IWebElement element, string value)
        {
            element.Clear();
            element.SendKeys(value);
        }

        private IWebElement GetInputByLabel(string labelText)
        {
            var labels = _driver.FindElements(By.XPath($"//label[normalize-space(text())='{labelText}']"));

            if (labels.Count == 0)
            {
                throw new NoSuchElementException($"No label found with text '{labelText}'");
            }

            foreach (var label in labels)
            {
                string forAttribute = label.GetAttribute("for");
                if (!string.IsNullOrEmpty(forAttribute))
                {
                    try
                    {
                        return _driver.FindElement(By.Id(forAttribute));
                    }
                    catch (NoSuchElementException)
                    {
                    }
                }

                try
                {
                    var inputDescendant = label.FindElements(By.TagName("input")).FirstOrDefault();
                    if (inputDescendant != null) return inputDescendant;
                }
                catch { }

                try
                {
                    var followingInput = label.FindElement(By.XPath("./following-sibling::input"));
                    if (followingInput != null) return followingInput;
                }
                catch { }

                try
                {
                    var parent = label.FindElement(By.XPath(".."));
                    var inputsInParent = parent.FindElements(By.TagName("input"));
                    if (inputsInParent.Count == 1) return inputsInParent[0];
                }
                catch { }
            }

            throw new NoSuchElementException($"Could not find an input associated with label '{labelText}' using any strategy.");
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}
