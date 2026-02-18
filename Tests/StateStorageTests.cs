using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using Allurre_with_Nunit.Infrastructure;

namespace Allurre_with_Nunit.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
[AllureNUnit]
[AllureSuite("State Storage Tests")]
[AllureFeature("Authentication State")]
public class StateStorageTests : TestBase
{
    [Test(Description = "Verify stored auth state allows direct access to inventory")]
    [AllureStory("Access inventory page using stored authentication state")]
    [AllureTag("regression")]
    public async Task StoredStateAllowsDirectAccess()
    {
        await Page.GotoAsync($"{Settings.BaseUrl}/inventory.html");

        await Assertions.Expect(Page).ToHaveTitleAsync(new Regex("Swag Labs"));
        await Assertions.Expect(Page).ToHaveURLAsync($"{Settings.BaseUrl}/inventory.html");
    }
}
