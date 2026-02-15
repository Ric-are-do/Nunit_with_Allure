# Architecture Refactor Design

**Date:** 2026-02-15
**Status:** Approved

## Overview

Incremental refactor of the Allurre_with_Nunit Playwright test project to introduce proper folder structure, Page Object Model, configuration management, and CI/CD improvements.

## Target Folder Structure

```
Allurre_with_Nunit/
├── Config/
│   ├── appsettings.json          # Default config (base URL, headless, browser)
│   └── TestSettings.cs           # Strongly-typed config class
├── Pages/
│   ├── LoginPage.cs              # Login page locators + actions
│   └── InventoryPage.cs          # Inventory page locators + assertions
├── Infrastructure/
│   ├── TestBase.cs               # Browser-per-fixture, page-per-test lifecycle
│   └── GlobalSetup.cs            # Assembly-level auth state storage
├── Tests/
│   ├── LoginTests.cs             # Login test suite
│   └── StateStorageTests.cs      # Stored state verification
├── .github/workflows/test.yml    # Improved CI pipeline
├── dockerfile                    # Test runner container
├── CLAUDE.md                     # Local project context (gitignored)
└── Allurre_with_Nunit.csproj
```

## Configuration System

- `Config/appsettings.json` holds defaults: base URL, headless mode, browser type, credentials
- `Config/TestSettings.cs` is a strongly-typed POCO loaded via `Microsoft.Extensions.Configuration`
- Environment variables override config values for CI flexibility:
  - `TEST_BASE_URL` overrides `BaseUrl`
  - `TEST_HEADLESS` overrides `Headless`
  - `TEST_BROWSER` overrides `Browser`
- New NuGet packages: `Microsoft.Extensions.Configuration`, `Microsoft.Extensions.Configuration.Json`, `Microsoft.Extensions.Configuration.EnvironmentVariables`

## Page Object Model (Minimal)

**LoginPage** (`Pages/LoginPage.cs`):
- Constructor takes `IPage`
- Locators: `#user-name`, `#password`, `#login-button`, error message
- Methods: `NavigateAsync()`, `LoginAsync(username, password)`, `GetErrorMessageAsync()`

**InventoryPage** (`Pages/InventoryPage.cs`):
- Constructor takes `IPage`
- Locators: page title, product list
- Methods: `IsLoadedAsync()` (checks URL + title)

## Infrastructure

### Browser Lifecycle

| Scope | NUnit mechanism | What happens |
|---|---|---|
| Assembly | `[SetUpFixture]` GlobalSetup | Logs in once, saves auth state to `storedState.json` |
| Test class | `[OneTimeSetUp]` in TestBase | Creates a browser instance for the fixture |
| Test method | `[SetUp]` in TestBase | Creates a new BrowserContext + Page (new tab) |
| Test cleanup | `[TearDown]` in TestBase | Closes Page + Context |
| Class cleanup | `[OneTimeTearDown]` in TestBase | Closes Browser |

### Parallelism

- `[Parallelizable(ParallelScope.Self)]` on test fixtures: fixtures run in parallel
- Tests within a fixture run sequentially (no locking needed)
- Each fixture has an independent browser instance

### TestBase Pattern

```csharp
public class TestBase
{
    protected IBrowser Browser { get; private set; }
    protected IPage Page { get; private set; }
    private IBrowserContext _context;
    private IPlaywright _playwright;

    [OneTimeSetUp]
    public async Task ClassSetup()
    {
        var settings = TestSettings.Load();
        _playwright = await Playwright.CreateAsync();
        Browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = settings.Headless
        });
    }

    [SetUp]
    public async Task TestSetup()
    {
        _context = await Browser.NewContextAsync(new()
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

## CI/CD

### Dockerfile

Updated to be a proper test runner:
- Installs Playwright browsers during build
- `ENTRYPOINT` runs `dotnet test` with TRX logging

### GitHub Actions

- Add NuGet package caching via `actions/cache`
- Keep existing Allure report flow (already working)
- Existing workflow structure stays the same, just improved

## Test Scope

- Login page tests only (login success, login failure, title check)
- State storage verification test
- `UnitTest1.cs` removed (placeholder test)

## Decisions

- **Approach:** Incremental refactor (not rewrite)
- **POM granularity:** Minimal (LoginPage + InventoryPage only)
- **Config:** appsettings.json + env var overrides
- **Parallelism:** Fixtures parallel, tests within fixture sequential
- **CLAUDE.md:** Added to project root, gitignored for local context
