# NUnit + Allure + Playwright Test Framework

Automated UI testing framework using **NUnit 3**, **Playwright for .NET**, and **Allure reporting**, targeting [SauceDemo](https://www.saucedemo.com). Runs in CI via GitHub Actions with Docker, and publishes test reports automatically.

## Tech Stack

- .NET 8 / NUnit 3
- Playwright for .NET (browser automation)
- Allure NUnit (test reporting)
- GitHub Actions (CI)
- Docker (`dotnet/sdk:8.0`)

## Project Structure

```
Allurre_with_Nunit/
├── Config/
│   ├── appsettings.json        # Test configuration defaults
│   └── TestSettings.cs         # Settings loader with env var overrides
├── Pages/
│   ├── LoginPage.cs            # Login page object
│   └── InventoryPage.cs        # Inventory page object
├── Infrastructure/
│   ├── GlobalSetup.cs          # One-time auth state setup
│   └── TestBase.cs             # Browser/page lifecycle per fixture
├── Tests/
│   ├── LoginTests.cs           # Login feature tests
│   ├── StateStorageTests.cs    # Auth state reuse tests
│   └── QuickTests.cs           # Unit tests for report validation
├── dockerfile                  # Docker test runner
└── .github/workflows/test.yml  # CI pipeline
```

## Configuration

Default settings are in `Config/appsettings.json`:

```json
{
  "BaseUrl": "https://www.saucedemo.com",
  "Headless": true,
  "Browser": "chromium",
  "StandardUser": "standard_user",
  "StandardPassword": "secret_sauce"
}
```

Override any setting with environment variables for CI or local use:

| Setting | Environment Variable |
|---------|---------------------|
| BaseUrl | `TEST_BASE_URL` |
| Headless | `TEST_HEADLESS` |
| Browser | `TEST_BROWSER` |

## Running Tests Locally

```powershell
# Build the project
dotnet build

# Install Playwright browsers
pwsh bin/Debug/net8.0/playwright.ps1 install

# Run all tests
dotnet test
```

To run in headed mode (visible browser):

```powershell
$env:TEST_HEADLESS="false"
dotnet test
```

To generate an Allure report locally (requires Java and Allure CLI):

```powershell
allure serve bin/Debug/net8.0/allure-results
```

Or generate a single-file HTML report:

```powershell
allure generate --single-file bin/Debug/net8.0/allure-results --clean -o allure-report
```

## Running with Docker

```powershell
docker build -t allure-tests .
docker run allure-tests
```

## GitHub Pages — Live Test Report

The latest Allure test report is always available at a **fixed URL**:

**https://ric-are-do.github.io/Nunit_with_Allure/**

This link never changes — bookmark it or share it with your team. It automatically redirects to the most recent test run.

**How it works:** Each pipeline run creates a numbered report (e.g. `/1/`, `/2/`, `/3/`). A root `index.html` always redirects to the latest. Previous runs remain accessible at their numbered URLs if needed.

The report includes:
- Test results with pass/fail status for every test
- Run history and trend charts across previous runs
- Test suites, features, and stories organized by Allure attributes
- Detailed test steps and timing information

The report updates automatically on every push to `main` or `dev`. No manual steps needed — just push code and check the site.

## CI Pipeline

The GitHub Actions workflow (`.github/workflows/test.yml`) triggers on pushes to `main` and `dev` branches, or manually via workflow dispatch.

**Pipeline steps:**
1. Build project and install Playwright browsers
2. Run all tests
3. Generate and publish reports

**On every run (pass or fail), three outputs are produced:**

| Output | Description | Where to find it |
|--------|-------------|-----------------|
| GitHub Pages report | Live Allure report with run history | Repository's GitHub Pages site |
| Single-file HTML report | Portable `index.html` to share with anyone | Actions > run summary > `allure-report` artifact |
| TRX test results | Import into Visual Studio for analysis | Actions > run summary > `trx-test-results` artifact |

### Using the artifacts

**Single-file Allure report:** Download the `allure-report` artifact zip from the Actions run, extract it, and open `index.html` in any browser. No server or tools needed.

**TRX results:** Download the `trx-test-results` artifact and open the `.trx` file in Visual Studio (Test > Windows > Test Explorer, or double-click the file).

## Architecture

### Page Object Model

Page objects (`Pages/`) encapsulate page interactions. Each takes an `IPage` in its constructor and exposes async methods:

```csharp
var loginPage = new LoginPage(Page, Settings);
await loginPage.NavigateAsync();
await loginPage.LoginAsync("standard_user", "secret_sauce");
```

### Test Lifecycle

- **GlobalSetup** runs once before all tests — logs in and saves auth state to `storedState.json`
- **TestBase** manages browser and page lifecycle:
  - `[OneTimeSetUp]` — creates a browser per test fixture
  - `[SetUp]` — creates a fresh page per test, preloaded with stored auth state
  - `[TearDown]` — closes the page
  - `[OneTimeTearDown]` — closes the browser
- Test fixtures run in parallel; tests within a fixture run sequentially

### Allure Reporting

All test classes use Allure attributes for structured reporting:

```csharp
[AllureNUnit]
[AllureSuite("Login Test Suite")]
[AllureFeature("Login")]
public class LoginTests : TestBase
{
    [Test]
    [AllureStory("Valid Login")]
    [AllureTag("regression")]
    public async Task LoginSuccess() { ... }
}
```

## Reference

- [NUnit Documentation](https://docs.nunit.org/articles/nunit/intro.html)
- [Playwright .NET Documentation](https://playwright.dev/dotnet/docs/intro)
- [Allure NUnit Documentation](https://allurereport.org/docs/nunit/)
- [SauceDemo Test Site](https://www.saucedemo.com)
