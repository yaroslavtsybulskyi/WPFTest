using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace EndToEndTestSample
{
    public class CreateVehicleTest
    {

        [Test]
        public void CreateNewVehicle()
        {
            AppiumOptions options = new AppiumOptions();

            options.AddAdditionalCapability("app", "C:\\Users\\UNICORN\\source\\repos\\CSProCourse\\src\\Logistic.Desktop\\Logistic.Desktop\\bin\\Debug\\net7.0-windows\\Logistic.Desktop.exe");

            WindowsDriver<WindowsElement> driver = new WindowsDriver<WindowsElement>(
                new Uri("http://127.0.0.1:4723/"), options);

            Thread.Sleep(3000);

            try
            {
                Thread.Sleep(5000);
                driver.FindElementByAccessibilityId("EnterNumber").SendKeys("1");

                driver.FindElementByAccessibilityId("EnterMaxWeight").SendKeys("50");

                driver.FindElementByAccessibilityId("EnterMaxVolume").SendKeys("100");

                WindowsElement comboBox = driver.FindElementByAccessibilityId("SelectVehicleType");
                comboBox.Click();

                WindowsElement desiredOption = driver.FindElementByXPath("//ListItem[@Name='Ship']");
                desiredOption.Click();

                driver.FindElementByAccessibilityId("CreateButton").Click();

                WindowsElement newVehicle = driver.FindElementByAccessibilityId("1");

                Assert.That(newVehicle, Is.Not.Null, "Vehicle was not created");

            }
            finally
            {
                driver.Close();
            }

        }
    }
}