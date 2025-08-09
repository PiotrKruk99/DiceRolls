using Microsoft.JSInterop;

namespace FullBlazorVersion.Services.SessionStorage;

public class SessionStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public SessionStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetItem(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorageInterop.setItem", key, value);
    }

    public async Task<string> GetItem(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("sessionStorageInterop.getItem", key);
    }

    public async Task RemoveItem(string key)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorageInterop.removeItem", key);
    }
}
