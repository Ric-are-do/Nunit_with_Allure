using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Allure.NUnit;

namespace Swaglabsstes;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[AllureNUnit]
public class ExampleTest : PageTest
{
    [Test]
    public async Task HasTitle()
    {
        await Page.GotoAsync("https://www.saucedemo.com/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Swag Labs"));
    }

    [Test]
    public async Task login()
    {
        await Page.GotoAsync("https://www.saucedemo.com/");

        // Click the get started link.
        await Page.Locator("#user-name").FillAsync("standard_user");
        await Page.Locator("#password").FillAsync("secret_sauce");
        await Page.Locator("#login-button").ClickAsync();

        // Expects page to have a heading with the name of Installation.
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    }
}