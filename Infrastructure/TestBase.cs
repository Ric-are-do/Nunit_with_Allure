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
        var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "storedState.json");
        var contextOptions = new BrowserNewContextOptions();
        if (File.Exists(storagePath))
        {
            contextOptions.StorageStatePath = storagePath;
        }
        _context = await Browser.NewContextAsync(contextOptions);
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
