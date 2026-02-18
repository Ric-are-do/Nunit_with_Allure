using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using Allurre_with_Nunit.Infrastructure;
using Allurre_with_Nunit.Pages;

namespace Allurre_with_Nunit.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[AllureNUnit]
[AllureSuite("Login Test Suite")]
[AllureFeature("Login")]
public class LoginTests : TestBase
{
    [Test(Description = "Verify the Swag Labs page title")]
    [AllureStory("Navigate to Swag Labs and verify title")]
    [AllureTag("regression")]
    public async Task HasTitle()
    {
        var loginPage = new LoginPage(Page, Settings);
        await loginPage.NavigateAsync();

        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Swag Labs"));
    }

    [Test(Description = "Login with valid credentials succeeds")]
    [AllureStory("Log into Swag Labs with valid credentials")]
    [AllureTag("regression")]
    public async Task LoginSuccess()
    {
        var loginPage = new LoginPage(Page, Settings);
        await loginPage.NavigateAsync();
        await loginPage.LoginAsync(Settings.StandardUser, Settings.StandardPassword);

        var inventoryPage = new InventoryPage(Page);
        Assert.That(await inventoryPage.IsLoadedAsync(), Is.True);
    }

    [Test(Description = "Login with invalid credentials fails")]
    [AllureStory("Log into Swag Labs with invalid credentials and verify error")]
    [AllureTag("regression")]
    public async Task LoginFailure()
    {
        var loginPage = new LoginPage(Page, Settings);
        await loginPage.NavigateAsync();
        await loginPage.LoginAsync("potato", "potato");

        var errorMessage = await loginPage.GetErrorMessageAsync();
        Assert.That(errorMessage, Does.Contain("Username and password do not match"));
    }
}
