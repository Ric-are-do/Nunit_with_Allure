using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[SetUpFixture]
public class GlobalSetup
{
    public IPlaywright Pw { get;set; }
    public IBrowser Browser { get;set; }

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        // Code that runs once before any tests in the assembly
        Console.WriteLine("Global setup before any tests run.");

        // Adding some playwright setup here 
        Pw = await Playwright.CreateAsync();
        Browser = await Pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        var context = await Browser.NewContextAsync();
        var page = await context.NewPageAsync();


        // setup the login for swag labs so we can store the state
        await page.GotoAsync("https://www.saucedemo.com/");
        await page.Locator("#user-name").FillAsync("standard_user");
        await page.Locator("#password").FillAsync("secret_sauce");
        await page.Locator("#login-button").ClickAsync();

        // this is how I will store the state nb this will be stored Path = /playwright/.auth/state.json"
        // But im going to just keep teh stored state test inside the debug folder 
        // stored Allurre_with_Nunit\bin\Debug\net8.0
        // read more : https://playwright.dev/dotnet/docs/auth#signing-in-before-each-test
        await context.StorageStateAsync(new()
        {
            Path = Path.Combine(Directory.GetCurrentDirectory(), "storedState.json")
        });

        await page.CloseAsync();
        await Browser.CloseAsync();
        
        /*
         * now when we run our tests there will be 2 sessions 
         * 1 that runs the onetime login and stores the state
         * 2 will be the actual test
         */

        // Next we need to use this saved state json file ,
        // now lets use it 




    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {


        var timestamps = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var targetDir = Path.Combine($"Run_{timestamps}");
        var baseTestDirectory = "../../../TestReports";

        Directory.CreateDirectory("../../../TestReports"); 

        if (Directory.Exists("allure-results"))
        {
            Directory.Move("allure-results", $"{baseTestDirectory}/{targetDir}");
        }
        else
        {
            Console.WriteLine("Directory 'allure-results' not found.");
        }
    }
}

