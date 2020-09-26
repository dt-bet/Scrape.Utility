using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Opera;

namespace Scrape.Utility.Selenium
{
    public static class DriverFactory
    {


        public static IWebDriver BuildChrome(bool showScraper = true)
        {

            ChromeDriverService chromeDriverService;
            try
            {
                chromeDriverService = ChromeDriverService.CreateDefaultService();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                chromeDriverService = ChromeDriverService.CreateDefaultService(System.IO.Directory.GetCurrentDirectory());
            }
            chromeDriverService.HideCommandPromptWindow = !showScraper;

            //taken from https://github.com/seleniumhq/selenium/issues/6234
            ChromeOptions options = new ChromeOptions();
            if (showScraper == false)
            {
                options.AddArgument("headless");
            }
            options.AddArgument("no-sandbox");
            options.AddArgument("proxy-server='direct://'");
            options.AddArgument("proxy-bypass-list=*");

            var _driver = new ChromeDriver(chromeDriverService,options);
            try { _driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(200); }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _driver;
        }


        public static IWebDriver BuildEdge(bool showScraper = true)
        {
            EdgeDriverService driverService;
            try
            {
                driverService = EdgeDriverService.CreateDefaultService(System.Reflection.Assembly.GetExecutingAssembly().Location, "MicrosoftWebDriver.exe");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                driverService = EdgeDriverService.CreateDefaultService(System.IO.Directory.GetCurrentDirectory());
            }
            driverService.HideCommandPromptWindow = !showScraper;


            EdgeOptions options = new EdgeOptions();
            options.BrowserVersion = "";
            //if (showScraper == false)
            //{
            //    options.AddArgument("headless");
            //}
            //options.AddArgument("no-sandbox");
            //options.AddArgument("proxy-server='direct://'");
            //options.AddArgument("proxy-bypass-list=*");

            var _driver = new EdgeDriver(driverService, options);
            try { _driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(200); }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _driver;
        }


//        testSettings.BrowserName = "Edge " + browserVersion;
//             var driverService = EdgeDriverService.CreateDefaultService(AssemblyDirectory,
//                 "MicrosoftWebDriver.exe");
//        var options = new EdgeOptions
//        {
//            PageLoadStrategy = EdgePageLoadStrategy.Default
//        };
//        var driver = new EdgeDriver(driverService, options, testSettings.TimeoutTimeSpan);
//             if (testSettings.DeleteAllCookies)
//             {
//                 driver.Manage().Cookies.DeleteAllCookies();
//             }
//    driver.Manage().Timeouts().ImplicitlyWait(testSettings.TimeoutTimeSpan);
//             if (testSettings.MaximiseBrowser)
//             {
//                 driver.Manage().Window.Maximize();
//             }
//var extendedWebDriver = new TestWebDriver(driver, testSettings, TestOutputHelper);
//TestWebDriver = extendedWebDriver;
//             return extendedWebDriver;


        public static IWebDriver BuildOpera(bool showScraper = true)
        {
            OperaDriverService service = OperaDriverService.CreateDefaultService(System.IO.Directory.GetCurrentDirectory());
            service.HideCommandPromptWindow = true;
            OperaOptions options = new OperaOptions() { };
            options.BinaryLocation = @"C:\Users\rytal\AppData\Local\Programs\Opera\launcher.exe";
            if (showScraper == false)
            {
                options.AddArgument("headless");
            }
            options.AddArguments("--no-sandbox");
            options.AddArguments("--disable-dev-shm-usage");
            TimeSpan time = TimeSpan.FromSeconds(10);
            var driver = new OperaDriver(service, options, time);
            return driver;

        }



        public static IWebDriver BuildFirefox(bool showScraper = true, int timeOut = 10)
        {
            // I'm assuming your geckodriver.exe is located there:
            // @"C:\MyGeckoDriverExePath\geckodriver.exe"
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(System.IO.Directory.GetCurrentDirectory());
            service.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe"; // May not be necessary
            service.HideCommandPromptWindow = true;
            FirefoxOptions options = new FirefoxOptions() { };
            if (showScraper == false)
            {
                options.AddArgument("--headless");
            }
            var driver = new FirefoxDriver(service, options, TimeSpan.FromSeconds(timeOut));
            return driver;
        }



    }
}
