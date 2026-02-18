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
