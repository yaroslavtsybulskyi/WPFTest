using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace EndToEndTestSample
{
    public class DeleteVehicleTest
    {

        [Test]
        public void CreateAndDeleteNewVehicle()
        {
            AppiumOptions options = new AppiumOptions();

            options.AddAdditionalCapability("app", "C:\\Users\\UNICORN\\source\\repos\\CSProCourse\\src\\Logistic.Desktop\\Logistic.Desktop\\bin\\Debug\\net7.0-windows\\Logistic.Desktop.exe");

            WindowsDriver<WindowsElement> driver = new WindowsDriver<WindowsElement>(
                new Uri("http://127.0.0.1:4723/"), options);

            Thread.Sleep(3000);

            try
            {
                Thread.Sleep(5000);
                driver.FindElementByAccessibilityId("EnterNumber").SendKeys("435");

                driver.FindElementByAccessibilityId("EnterMaxWeight").SendKeys("120");

                driver.FindElementByAccessibilityId("EnterMaxVolume").SendKeys("200");

                WindowsElement comboBox = driver.FindElementByAccessibilityId("SelectVehicleType");
                comboBox.Click();

                WindowsElement desiredOption = driver.FindElementByXPath("//ListItem[@Name='Plane']");
                desiredOption.Click();

                driver.FindElementByAccessibilityId("CreateButton").Click();

                WindowsElement newVehicle = driver.FindElementByAccessibilityId("1");

                Assert.That(newVehicle, Is.Not.Null, "Vehicle was not created");
                
                newVehicle.Click();

                driver.FindElementByAccessibilityId("DeleteButton").Click();

                bool isElementPresent = true;
                try
                {
                    WindowsElement deletedVehicle = driver.FindElementByAccessibilityId("1");
                }
                catch (WebDriverException)
                {
                    isElementPresent = false;
                }

                Assert.False(isElementPresent, "Element is still present in the data grid after deletion");


            }
            finally
            {
                driver.Close();
            }

        }
    }
}