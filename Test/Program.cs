using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        [Test]

        public void CheckItemCategory( )
        {
            string categoryName = TestContext.Parameters["categoryName"];
            string ItemName = TestContext.Parameters["ItemName"];
            TestContext.WriteLine(categoryName);
            TestContext.WriteLine(ItemName);

            IWebDriver driver;

            driver = new ChromeDriver();
            driver.Url = "https://ek.ua/";

            IWebElement categories_list = driver.FindElement(By.ClassName("mainmenu-list"));
            IReadOnlyCollection<IWebElement> categories = categories_list.FindElements(By.TagName("li"));

            foreach (IWebElement category in categories)
            {
                string actualCategory = category.Text;
                TestContext.WriteLine($"Processing categoty {actualCategory}");

                IWebElement items_list = category.FindElement(By.ClassName("mainmenu-subwrap"));
                IReadOnlyCollection<IWebElement> items = items_list.FindElements(By.TagName("a"));
                foreach (IWebElement item in items)
                {
                    string actualItem = item.GetAttribute("textContent"); 
                    TestContext.WriteLine($"Processing item {actualItem}");
                    if (actualItem.Contains(ItemName))
                    {
                        Assert.IsTrue(actualCategory == categoryName);
                    }
                }

            }

            driver.Close();
        }
    }
}
