# Architecture Refactor Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Refactor the Allurre_with_Nunit project to use proper folder structure, configuration management, Page Object Model, and improved CI/CD.

**Architecture:** Incremental refactor — each task builds on the previous one. Config system first, then page objects, then infrastructure, then migrate tests, then clean up old files, then CI/CD improvements.

**Tech Stack:** .NET 8, NUnit 3, Playwright for .NET, Allure.NUnit, Microsoft.Extensions.Configuration

---

### Task 1: Add Configuration NuGet Packages

**Files:**
- Modify: `Allurre_with_Nunit.csproj`

**Step 1: Add NuGet packages**

Run:
```bash
cd /c/Users/ricar/source/repos/Allurre_with_Nunit
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
```

**Step 2: Verify restore succeeds**

Run: `dotnet restore`
Expected: Restore successful, no errors

**Step 3: Commit**

```bash
git add Allurre_with_Nunit.csproj
git commit -m "chore: add Microsoft.Extensions.Configuration packages"
```

---

### Task 2: Create Configuration System

**Files:**
- Create: `Config/appsettings.json`
- Create: `Config/TestSettings.cs`
- Modify: `Allurre_with_Nunit.csproj` (to copy appsettings.json to output)

**Step 1: Create `Config/appsettings.json`**

```json
{
  "TestSettings": {
    "BaseUrl": "https://www.saucedemo.com",
    "Headless": true,
    "Browser": "chromium",
    "StandardUser": "standard_user",
    "StandardPassword": "secret_sauce"
  }
}
```

**Step 2: Create `Config/TestSettings.cs`**

```csharp
using Microsoft.Extensions.Configuration;

namespace Allurre_with_Nunit.Config;

public class TestSettings
{
    public string BaseUrl { get; set; } = "https://www.saucedemo.com";
    public bool Headless { get; set; } = true;
    public string Browser { get; set; } = "chromium";
    public string StandardUser { get; set; } = "standard_user";
    public string StandardPassword { get; set; } = "secret_sauce";

    public static TestSettings Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables(prefix: "TEST_")
            .Build();

        var settings = new TestSettings();
        config.GetSection("TestSettings").Bind(settings);

        // Environment variable overrides
        var baseUrl = Environment.GetEnvironmentVariable("TEST_BASE_URL");
        if (!string.IsNullOrEmpty(baseUrl)) settings.BaseUrl = baseUrl;

        var headless = Environment.GetEnvironmentVariable("TEST_HEADLESS");
        if (!string.IsNullOrEmpty(headless)) settings.Headless = bool.Parse(headless);

        var browser = Environment.GetEnvironmentVariable("TEST_BROWSER");
        if (!string.IsNullOrEmpty(browser)) settings.Browser = browser;

        return settings;
    }
}
```

**Step 3: Add appsettings.json copy to output in `.csproj`**

Add this `<ItemGroup>` to `Allurre_with_Nunit.csproj`:

```xml
<ItemGroup>
  <None Update="Config\appsettings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

**Step 4: Verify build succeeds**

Run: `dotnet build`
Expected: Build succeeded, no errors

**Step 5: Commit**

```bash
git add Config/appsettings.json Config/TestSettings.cs Allurre_with_Nunit.csproj
git commit -m "feat: add configuration system with appsettings.json and env var overrides"
```

---

### Task 3: Create LoginPage Page Object

**Files:**
- Create: `Pages/LoginPage.cs`

**Step 1: Create `Pages/LoginPage.cs`**

```csharp
using Microsoft.Playwright;
using Allurre_with_Nunit.Config;

namespace Allurre_with_Nunit.Pages;

public class LoginPage
{
    private readonly IPage _page;
    private readonly TestSettings _settings;

    public LoginPage(IPage page, TestSettings settings)
    {
        _page = page;
        _settings = settings;
    }

    // Locators
    private ILocator UsernameInput => _page.Locator("#user-name");
    private ILocator PasswordInput => _page.Locator("#password");
    private ILocator LoginButton => _page.Locator("#login-button");
    private ILocator ErrorMessage => _page.Locator("[data-test='error']");

    public async Task NavigateAsync()
    {
        await _page.GotoAsync(_settings.BaseUrl);
    }

    public async Task LoginAsync(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }

    public async Task<string> GetErrorMessageAsync()
    {
        return await ErrorMessage.TextContentAsync() ?? string.Empty;
    }
}
```

**Step 2: Verify build succeeds**

Run: `dotnet build`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add Pages/LoginPage.cs
git commit -m "feat: add LoginPage page object"
```

---

### Task 4: Create InventoryPage Page Object

**Files:**
- Create: `Pages/InventoryPage.cs`

**Step 1: Create `Pages/InventoryPage.cs`**

```csharp
using Microsoft.Playwright;

namespace Allurre_with_Nunit.Pages;

public class InventoryPage
{
    private readonly IPage _page;

    public InventoryPage(IPage page)
    {
        _page = page;
    }

    // Locators
    private ILocator Title => _page.Locator(".title");
    private ILocator ProductList => _page.Locator(".inventory_list");

    public async Task<bool> IsLoadedAsync()
    {
        var url = _page.Url;
        var titleVisible = await Title.IsVisibleAsync();
        return url.Contains("/inventory.html") && titleVisible;
    }
}
```

**Step 2: Verify build succeeds**

Run: `dotnet build`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add Pages/InventoryPage.cs
git commit -m "feat: add InventoryPage page object"
```

---

### Task 5: Create Infrastructure/TestBase.cs

**Files:**
- Create: `Infrastructure/TestBase.cs`

**Step 1: Create `Infrastructure/TestBase.cs`**

```csharp
using Microsoft.Playwright;
using Allurre_with_Nunit.Config;

namespace Allurre_with_Nunit.Infrastructure;

public class TestBase
{
    protected IBrowser Browser { get; private set; } = null!;
    protected IPage Page { get; private set; } = null!;
    protected TestSettings Settings { get; private set; } = null!;
    private IBrowserContext _context = null!;
    private IPlaywright _playwright = null!;

    [OneTimeSetUp]
    public async Task ClassSetup()
    {
        Settings = TestSettings.Load();
        _playwright = await Playwright.CreateAsync();

        var browserType = Settings.Browser.ToLower() switch
        {
            "firefox" => _playwright.Firefox,
            "webkit" => _playwright.Webkit,
            _ => _playwright.Chromium
        };

        Browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = Settings.Headless
        });
    }

    [SetUp]
    public async Task TestSetup()
    {
        _context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            StorageStatePath = Path.Combine(Directory.GetCurrentDirectory(), "storedState.json")
        });
        Page = await _context.NewPageAsync();
    }

    [TearDown]
    public async Task TestTeardown()
    {
        await Page.CloseAsync();
        await _context.CloseAsync();
    }

    [OneTimeTearDown]
    public async Task ClassTeardown()
    {
        await Browser.CloseAsync();
        _playwright.Dispose();
    }
}
```

**Step 2: Verify build succeeds**

Run: `dotnet build`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add Infrastructure/TestBase.cs
git commit -m "feat: add TestBase with browser-per-fixture, page-per-test lifecycle"
```

---

### Task 6: Move and Refactor GlobalSetup

**Files:**
- Create: `Infrastructure/GlobalSetup.cs`
- Delete: `GlobalSetup.cs` (root)

**Step 1: Create `Infrastructure/GlobalSetup.cs`**

```csharp
using Microsoft.Playwright;
using Allurre_with_Nunit.Config;

namespace Allurre_with_Nunit.Infrastructure;

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
```

**Step 2: Delete old `GlobalSetup.cs` from root**

Delete: `GlobalSetup.cs` (the file at project root)

**Step 3: Verify build succeeds**

Run: `dotnet build`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add Infrastructure/GlobalSetup.cs
git rm GlobalSetup.cs
git commit -m "refactor: move GlobalSetup to Infrastructure/, make config-driven"
```

---

### Task 7: Create New Login Tests

**Files:**
- Create: `Tests/LoginTests.cs`

**Step 1: Create `Tests/LoginTests.cs`**

```csharp
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
```

**Step 2: Verify build succeeds**

Run: `dotnet build`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add Tests/LoginTests.cs
git commit -m "feat: add LoginTests using POM and TestBase"
```

---

### Task 8: Create New StateStorageTests

**Files:**
- Create: `Tests/StateStorageTests.cs`

**Step 1: Create `Tests/StateStorageTests.cs`**

```csharp
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
```

**Step 2: Verify build succeeds**

Run: `dotnet build`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add Tests/StateStorageTests.cs
git commit -m "feat: add StateStorageTests using TestBase"
```

---

### Task 9: Run All Tests and Verify

**Step 1: Run all tests**

Run: `dotnet test --filter "Namespace~Allurre_with_Nunit.Tests" --logger "console;verbosity=detailed"`
Expected: All tests pass (LoginTests: 3 pass, StateStorageTests: 1 pass)

**Step 2: If any test fails, fix before proceeding**

Check output carefully. Common issues:
- `storedState.json` not found: GlobalSetup may not have run — check namespace/assembly
- Locator errors: verify selectors match current SauceDemo site
- Config not loading: verify `appsettings.json` copies to output directory

---

### Task 10: Remove Old Test Files

**Files:**
- Delete: `UnitTest1.cs`
- Delete: `SwagLabs/SwagLabsTest.cs`
- Delete: `SwagLabs/StateStorageTests.cs`
- Delete: `SwagLabs/Testbase.cs`
- Delete: `SwagLabs/` directory (should be empty after above)

**Step 1: Delete old files**

```bash
git rm UnitTest1.cs
git rm SwagLabs/SwagLabsTest.cs
git rm SwagLabs/StateStorageTests.cs
git rm SwagLabs/Testbase.cs
```

**Step 2: Verify build succeeds**

Run: `dotnet build`
Expected: Build succeeded, no references to deleted files

**Step 3: Run tests again to confirm nothing broke**

Run: `dotnet test --logger "console;verbosity=detailed"`
Expected: All tests pass

**Step 4: Commit**

```bash
git add -A
git commit -m "chore: remove old test files and SwagLabs directory"
```

---

### Task 11: Update Dockerfile

**Files:**
- Modify: `dockerfile`

**Step 1: Replace dockerfile contents**

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet build
RUN pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps

ENTRYPOINT ["dotnet", "test", "--logger", "trx;LogFileName=test_results.trx", "--results-directory", "TestReports"]
```

**Step 2: Verify Docker build (if Docker available)**

Run: `docker build -t allure-tests .`
Expected: Build succeeds

**Step 3: Commit**

```bash
git add dockerfile
git commit -m "feat: update Dockerfile to be a proper test runner"
```

---

### Task 12: Improve GitHub Actions Workflow

**Files:**
- Modify: `.github/workflows/test.yml`

**Step 1: Update workflow with caching**

```yaml
name: playwright-tests

on:
  push:
    branches:
      - main
      - dev
  workflow_dispatch:

jobs:
  run-tests:
    permissions:
      contents: write
    runs-on: ubuntu-latest
    container:
      image: mcr.microsoft.com/dotnet/sdk:8.0
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET environment
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 8.0.x

      - name: Cache NuGet packages
        uses: actions/cache@v4
        with:
          path: ~/.nuget/packages
          key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
          restore-keys: |
            ${{ runner.os }}-nuget-

      - name: Install dependencies and build
        run: dotnet build

      - name: Install Playwright browsers
        run: pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps

      - name: Run tests
        id: run-tests
        run: dotnet test --logger "trx;LogFileName=test_results.trx" --results-directory TestReports

      # Allure history setup
      - name: Load test report history
        uses: actions/checkout@v4
        if: always()
        continue-on-error: true
        with:
          ref: gh-pages
          path: gh-pages

      - name: Build test report
        uses: simple-elf/allure-report-action@v1.12
        if: always()
        with:
          gh_pages: gh-pages
          allure_history: allure-history
          allure_results: bin/Debug/net8.0/allure-results

      - name: Publish test report
        uses: peaceiris/actions-gh-pages@v4.0.0
        if: always()
        with:
          github_token: ${{ secrets.GITHUB_TOKEN }}
          publish_branch: gh-pages
          publish_dir: allure-history

      # Upload artifacts
      - name: Upload TRX logger file
        if: always()
        uses: actions/upload-artifact@v4
        with:
          name: test-results
          path: '**/test_results.trx'

      - name: Upload test report artifact if tests fail
        if: ${{ failure() && steps.run-tests.outcome == 'failure' }}
        uses: actions/upload-artifact@v4
        with:
          name: Test-reports-folder
          path: TestReports
```

Changes from current workflow:
- Added NuGet package caching step
- Removed the debug `ls -R TestReports` step
- Renamed "Check that browsers have been installed" to clearer "Install Playwright browsers"

**Step 2: Commit**

```bash
git add .github/workflows/test.yml
git commit -m "feat: add NuGet caching and clean up GitHub Actions workflow"
```

---

### Task 13: Update CLAUDE.md with Final State

**Files:**
- Modify: `CLAUDE.md`

**Step 1: Update CLAUDE.md TODOs to mark completed items**

Mark all completed phases and update any conventions that changed during implementation.

**Step 2: No commit needed (gitignored)**

---

### Summary of All Commits

1. `chore: add Microsoft.Extensions.Configuration packages`
2. `feat: add configuration system with appsettings.json and env var overrides`
3. `feat: add LoginPage page object`
4. `feat: add InventoryPage page object`
5. `feat: add TestBase with browser-per-fixture, page-per-test lifecycle`
6. `refactor: move GlobalSetup to Infrastructure/, make config-driven`
7. `feat: add LoginTests using POM and TestBase`
8. `feat: add StateStorageTests using TestBase`
9. *(test verification, no commit)*
10. `chore: remove old test files and SwagLabs directory`
11. `feat: update Dockerfile to be a proper test runner`
12. `feat: add NuGet caching and clean up GitHub Actions workflow`
