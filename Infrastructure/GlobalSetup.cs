using Microsoft.Playwright;
using Allurre_with_Nunit.Config;

namespace Allurre_with_Nunit;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        var settings = TestSettings.Load();

        var pw = await Playwright.CreateAsync();
        var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = settings.Headless
        });

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        // Login and save auth state
        await page.GotoAsync(settings.BaseUrl);
        await page.Locator("#user-name").FillAsync(settings.StandardUser);
        await page.Locator("#password").FillAsync(settings.StandardPassword);
        await page.Locator("#login-button").ClickAsync();
        await page.WaitForURLAsync("**/inventory.html");

        await context.StorageStateAsync(new()
        {
            Path = Path.Combine(Directory.GetCurrentDirectory(), "storedState.json")
        });

        await page.CloseAsync();
        await browser.CloseAsync();
        pw.Dispose();
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {
        // Allure results stay in bin/Debug for CI pipeline to pick up
    }
}
