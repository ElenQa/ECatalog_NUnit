using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Collections.Generic;

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


            for (int i = 0; i < 200; i++)
            {
                try
                {
                    string part1 = $"/html/body/div[3]/div/ul/li[{i}]";
                    IWebElement category = driver.FindElement(By.XPath(part1 + "/a"));
                    currentCategoryName = category.Text;

                    for (int j = 1; j < 100; j++)
                    {
                        string final = $"{part1}/div/div/a[{j}]";
                        IWebElement sub_category = driver.FindElement(By.XPath(final));
                        string sub_cat = sub_category.GetAttribute("textContent");
                        
                        if (sub_cat.Contains(search1))
                        {
                            driver.Close();
                            return currentCategoryName;
                          
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

                if(subcategory == exp_subcategory)
                {
                    return exp_category;
                }
               

            }
            return null;
        }


        [Test]
        public void ActualCategoryEqualExpected()
        {
            List<string> search = File.ReadAllLines("C:/Users/OChernovolyk/source/repos/ECatalog_NUnit/ECatalog_NUnit/bin/Debug/Test.txt").ToList();

            for (int i = 0; i<search.Count; i++)
            {
                string actualName = ReadActualCategories(search[i]);

                string expName = ReadExpectedCategories(search[i]);


                Assert.AreEqual(expName, actualName);
            }
        }

        static void Main(string []args)
        {


        }
    }

}