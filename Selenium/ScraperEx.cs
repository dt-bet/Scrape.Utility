using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Scrape.Utility.Selenium
{

    public static class WebDriverEx
    {
        public static bool IsDriverAttached(this IWebDriver driver)
        {

            return driver != null;
        }

        public static bool IsBrowserAttached(this IWebDriver driver)
        {
            try
            {
                var x = driver.PageSource;
            }
            catch
            {
                //Console.WriteLine("Browser closed");
                return false;
            }

            return true;
        }


        //public static List<string[]> ParseTable(string stable)
        //{

        //    // this is to test dcsoup for parsing speed based on recommendation made here:
        //    //https://stackoverflow.com/questions/21243523/using-seleniumdriver-to-extract-all-rows-and-columns-given-a-table-element
        //    Document doc = Dcsoup.Parse(stable, "UTF-8");
        //    List<string[]> tbl = new List<string[]>();

        //    foreach (Element rowElmt in doc.GetElementsByTag("tr"))
        //    {
        //        Elements cols = rowElmt.GetElementsByTag("th");
        //        if (cols.Count() == 0)
        //            cols = rowElmt.GetElementsByTag("td");

        //        string[] rowTxt = new string[cols.Count()];
        //        for (int j = 0; j < rowTxt.Length; j++)
        //        {
        //            rowTxt[j] = cols[j].Text;
        //        }
        //        tbl.Add(rowTxt);
        //    }

        //    return tbl;

        //}






        public static bool Login(this IWebDriver driver, string username, string password, string usernameId, string passwordId, string submitcss, string urllogin, string urlcomplete, bool loginByJavaScript = false, string submitstring = null)
        {



            driver.Navigate().GoToUrl(urllogin);


            Delay();
            IWebElement usernameElem = driver.FindElement(By.Id(usernameId));
            usernameElem.SendKeys(username);
            Delay();
            IWebElement passwordElem = driver.FindElement(By.Id(passwordId));
            passwordElem.SendKeys(password);
            Delay();
            IWebElement loginBtn = null;

            IJavaScriptExecutor js = null;

            if (submitstring != null)
            {
                js = (IJavaScriptExecutor)driver;
                loginBtn = (IWebElement)js.ExecuteScript("return " + submitstring);
            }
            else
            {
                loginBtn = driver.FindElement(By.CssSelector(submitcss));
            }

            if (loginByJavaScript)
            {
                if (submitstring != null)
                {
                    string jstr = String.Format("function triggerMouseEvent(eventType) " +
                        " {{" +
                        "  var node= {0}; " +
                        "  var clickEvent = document.createEvent('MouseEvents');" +
                        "  clickEvent.initEvent(eventType, true, true);" +
                        "  node.dispatchEvent(clickEvent);" +
                         "}}" +
                        "triggerMouseEvent('mouseover');" +
                        "triggerMouseEvent('mousedown');" +
                        "triggerMouseEvent('mouseup');" +
                        "triggerMouseEvent('click');", submitstring);

                    //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript(jstr);
                }
                else
                {
                    string jstr = System.IO.File.ReadAllText(@"Login.js");
                    js.ExecuteScript(jstr, loginBtn);
                }
            }
            else
            {
                //loginBtn.Submit();
                loginBtn.Click();
            }

            //var cookies = driver.Manage().Cookies.AllCookies;
            //pickle.dump(driver.get_cookies(), open("QuoraCookies.pkl", "wb"))

            Delay();


            return driver.Url != urllogin;


            //var cookies2 = driver.Manage().Cookies.AllCookies;
            //driver.Navigate().GoToUrl("http://www.oddsportal.com/login/");
            //IWebElement loginElem =driver.FindElement(By.Id("login-username1"));
            //IWebElement passwordElem = driver.FindElement(By.Id("login-password1"));

            //loginElem.SendKeys(username);
            //Delay();
            //passwordElem.SendKeys(password);
            //Delay();
            //IWebElement loginBtn = driver.FindElement(By.Name("login-submit"));
            //TennisOddsScraper.   WebScrape.AbstractScraper.ScraperHelpers.Delay();

            //loginBtn.Submit();
            ////loginBtn.Click();
            //Delay();

            //return driver.Url == "http://www.oddsportal.com/";


        }


        ////public static bool IsStale(this IWebDriver webDriver, IWebElement webElement)
        ////{
        ////    return ExpectedConditions.StalenessOf(webElement)(webDriver);
        ////}





        public static System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> WaitForElements(this IWebDriver driver, string css, int time = 120)
        {
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(time));

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> pagelinks = null;
            try
            {
                pagelinks = wait.Until((d) =>
                {
                    return d.FindElements(By.CssSelector(css));
                });
            }
            catch
            {
                int cnt = 0;
                while (pagelinks == null & cnt < 10)
                {
                    pagelinks = driver.FindElements(By.CssSelector(css));
                    Thread.Sleep(1000);
                    cnt++;
                }
            }

            return pagelinks;
        }

        public static IWebElement WaitForElement(this IWebDriver driver, string css, int time = 120)
        {
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(time));

            var pagelinks = wait.Until((d) =>
            {
                return d.FindElement(By.CssSelector(css));
            });


            return pagelinks;
        }

        public static Random _random = new Random();

        public static void Delay()
        {
            Thread.Sleep(_random.Next(1500, 1500));
        }

        public static bool TryClick(this OpenQA.Selenium.IWebElement webElement, OpenQA.Selenium.IWebDriver webDriver, string url)
        {
            webElement.Click();
            System.Threading.Thread.Sleep(1000);
            if (webDriver.Url == url)
            {
                throw new Exception("Url is not " + url);
            }
            return true;
        }


        public static bool TryNavigate(this OpenQA.Selenium.IWebDriver webDriver, string destinationUrl)
        {
            
            webDriver.Navigate().GoToUrl(destinationUrl);
            System.Threading.Thread.Sleep(1000);
            if (webDriver.Url != destinationUrl)
            {
                throw new Exception("Url is not " + destinationUrl);
            }
            return true;
        }

    }
}

