using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace E_Catalog
{


    [TestFixture]
    public class Program
    {
        public string ReadActualCategories(string subcategory)
        {
            IWebDriver driver;
            driver = new ChromeDriver();

            driver.Url = "https://ek.ua/";

            string search1 = subcategory;


            string currentCategoryName;


            for (int i = 1; i < 100; i++)
            {
                try
                {
                    //string part1 = $"/html/body/div[3]/div/ul/li[{i}]";
                    //IWebElement category = driver.FindElement(By.XPath(part1 + "/a"));

                    //currentCategoryName = categories.Text;

                    //for (int j = 1; j < 100; j++)
                    //{
                    //    string final = $"{part1}/div/div/a[{j}]";
                    //    IWebElement sub_category = driver.FindElement(By.XPath(final));
                    //    string sub_cat = sub_category.GetAttribute("textContent");

                    //    if (sub_cat.Contains(search1))

                    IWebElement categories_list = driver.FindElement(By.ClassName("mainmenu-list"));
                    IReadOnlyCollection<IWebElement> categories = categories_list.FindElements(By.TagName("li"));

                    foreach (IWebElement category in categories)
                    {
                        currentCategoryName = category.Text;
                        //TestContext.WriteLine($"Processing categoty {currentCategoryName}");

                        IWebElement items_list = category.FindElement(By.ClassName("mainmenu-subwrap"));
                        IReadOnlyCollection<IWebElement> items = items_list.FindElements(By.TagName("a"));
                        foreach (IWebElement item in items)
                        {
                            string actualItem = item.GetAttribute("textContent");
                            //TestContext.WriteLine($"Processing item {actualItem}");
                            if (actualItem.Contains(search1))

                            {
                                driver.Close();
                                return currentCategoryName;

                            }
                        }
                    }
                }


                catch (NoSuchElementException)
                {
                    continue;
                }
                catch (WebDriverException)
                {

                    break;
                }
                                
            }
            driver.Close();
            return null;
        }
        public string ReadExpectedCategories(string subcategory)
        {
            List<string> lines = File.ReadAllLines("C:/Users/OChernovolyk/source/repos/ECatalog_NUnit/ECatalog_NUnit/bin/Debug/MustBe.txt").ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                string exp_category;
                string exp_subcategory;

                int delim_pos = lines[i].IndexOf('-');
                exp_category = lines[i].Substring(0, delim_pos);
                exp_subcategory = (lines[i].Substring(delim_pos + 1));

                if (subcategory == exp_subcategory)
                {
                    return exp_category;
                }


            }
            return null;
        }


        
        public void ActualCategoryEqualExpected()
        {
            string temp;
            List<string> search = File.ReadAllLines("C:/Users/OChernovolyk/source/repos/ECatalog_NUnit/ECatalog_NUnit/bin/Debug/Test.txt").ToList();
            try {

                for (int i = 0; i < search.Count; i++)
                {
                    temp = search[i];
                    string actualName = ReadActualCategories(search[i]);

                    string expName = ReadExpectedCategories(search[i]);


                    //Assert.AreEqual(expName, actualName);
                    if (expName == actualName)
                    {
                        TestContext.WriteLine($"Категория {search[i]} находится в {actualName}");
                    }
                    else
                    {
                        TestContext.WriteLine($"Категория {search[i]} была в {expName} , а сейчас в {actualName}");
                        continue;
                    }

                }
                
                }
            catch (Exception e)
            {
                TestContext.WriteLine(e.Message);
            }
            


        }

        
    }

   
}
