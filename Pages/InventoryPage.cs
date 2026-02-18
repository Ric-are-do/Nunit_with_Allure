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
