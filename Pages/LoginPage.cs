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
