using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;

namespace EndToEndTestSample
{
    public class CreateVehicleAndAddCargoTest
    {

        [Test]
        public void CreateNewVehicleAndAddCargo()
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

                driver.FindElementByAccessibilityId("EnterMaxWeight").SendKeys("500");

                driver.FindElementByAccessibilityId("EnterMaxVolume").SendKeys("1000");

                WindowsElement comboBox = driver.FindElementByAccessibilityId("SelectVehicleType");
                comboBox.Click();

                WindowsElement desiredOption = driver.FindElementByXPath("//ListItem[@Name='Train']");
                desiredOption.Click();

                driver.FindElementByAccessibilityId("CreateButton").Click();

                WindowsElement newVehicle = driver.FindElementByAccessibilityId("1");

                newVehicle.Click();

                string mainWindowHandle = driver.CurrentWindowHandle;

                driver.FindElementByAccessibilityId("LoadCargoButton").Click();

                Thread.Sleep(2000);
                ReadOnlyCollection<string> windowHandles = driver.WindowHandles;
                foreach (string handle in windowHandles)
                {
                    if (handle != mainWindowHandle)
                    {
                        driver.SwitchTo().Window(handle);
                        break; 
                    }
                }

                driver.FindElementByAccessibilityId("TextVolume").SendKeys("100");
                driver.FindElementByAccessibilityId("TextWeight").SendKeys("400");
                driver.FindElementByAccessibilityId("TextCode").SendKeys("123");
                driver.FindElementByAccessibilityId("TextRecipientAddress").SendKeys("742 Evergreen Terrace");
                driver.FindElementByAccessibilityId("TextSenderAddress").SendKeys("Moe's Tavern");
                driver.FindElementByAccessibilityId("TextRecipientPhoneNumber").SendKeys("123456789");
                driver.FindElementByAccessibilityId("TextSenderPhoneNumber").SendKeys("987654321");

                Thread.Sleep(2000);

                driver.FindElementByAccessibilityId("SaveChangesButton").Click();

                driver.SwitchTo().Window(mainWindowHandle);

                driver.FindElementByAccessibilityId("1").Click();

                driver.FindElementByAccessibilityId("LoadCargoButton").Click();

                windowHandles = driver.WindowHandles;
                foreach (string handle in windowHandles)
                {
                    if (handle != mainWindowHandle)
                    {
                        driver.SwitchTo().Window(handle);
                        break;
                    }
                }

                Thread.Sleep(1000);
                WindowsElement listBox = driver.FindElementByAccessibilityId("ListBoxExistingCargo");

                var listItem = listBox.FindElementByClassName("ListBoxItem");

                Assert.That(listItem, Is.Not.Null, "The cargo code does not coincide");

                driver.Close();

                driver.SwitchTo().Window(mainWindowHandle);

            }
            finally
            {
                driver.Close();
            }

        }
    }
}