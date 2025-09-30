using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allurre_with_Nunit.SwagLabs
{
    public class Testbase
    {
        public IBrowser Browser { get; set; }
        public IPage Page { get; set; }

        // Create a hook that  will create a browser for me 
        //So whenver I want to use that authenticated state , I can inherit from this test base class

        [SetUp]
        public async Task Setup()
        {
            var playwright =   await Playwright.CreateAsync();
            Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                // temp setting headless to true here 
                Headless = true
            });

            var context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                // here we are using the stored state from the global setup 
                // So when the browser context runs it uses the state from here 
                StorageStatePath = Path.Combine(Directory.GetCurrentDirectory(), "storedState.json")
            });

            Page = await context.NewPageAsync(); // this will create a page thats already logged in since its using the storage state

        }

        [TearDown]
        public async Task Teardown()
        {
            await Page.CloseAsync();
            await Browser.CloseAsync();
        }
    }
}
