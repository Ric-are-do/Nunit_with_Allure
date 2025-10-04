using Allure.NUnit.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;
using Allure.NUnit;

namespace Allurre_with_Nunit.SwagLabs
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("State Storage Tests")]
    [AllureFeature("Testing Allure features")]
    //test coment 

    public class StateStorageTests:Testbase
    {
        [Test(Description = "This test Logs into swag labs  ")]
        [AllureStory("Log into swag labs and confirm that we are logged in successfully ")]
        [AllureStep("log into swag labs")]
        [AllureTag("regression")]
        public async Task login()
        {
            await Page.GotoAsync("https://www.saucedemo.com/inventory.html\"");

            // assert we are already logged in 
            await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Swag Labs"));
            await Assertions.Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
        }
    }
}
