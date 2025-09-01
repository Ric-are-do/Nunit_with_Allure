using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using System.ComponentModel;


namespace Swaglabsstes;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[AllureNUnit]
[AllureSuite("Swag Labse Test Suite")]
[AllureFeature("Testing Allure features")]
public class ExampleTest : PageTest
{
    [Test(Description = "This test logs in and checks the url ")]
    [AllureStory("This story test -  Navigate to Swag labs")]
    [AllureStep("This story test -  Navigate to Swag labs - allure step")]
    [AllureTag("regression")]
    public async Task HasTitle()
    {
        await Page.GotoAsync("https://www.saucedemo.com/");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Swag Labs"));
    }

    [Test(Description = "This test Logs into swag labs  ")]
    [AllureStory("Log into swag labs and confirm that we are logged in successfully ")]
    [AllureStep("log into swag labs")]
    [AllureTag("regression")]
    public async Task login()
    {
        await Page.GotoAsync("https://www.saucedemo.com/");

        // Click the get started link.
        await Page.Locator("#user-name").FillAsync("standard_user");
        await Page.Locator("#password").FillAsync("secret_sauce");
        await Page.Locator("#login-button").ClickAsync();
        Console.WriteLine("Failing test on purpose next ");
        Assert.Fail();
        Console.WriteLine("Forced assert fail");

        // Expects page to have a heading with the name of Installation.
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    }
}