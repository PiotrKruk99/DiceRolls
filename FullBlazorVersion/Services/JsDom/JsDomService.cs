using Microsoft.JSInterop;

namespace DiceRolls.Services.JsDom;

public class JsDomService
{
    private readonly IJSRuntime _jsRuntime;
    
    public JsDomService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    public async Task CopyAppUrl()
    {
        await _jsRuntime.InvokeVoidAsync("JSDOMInterop.copyAppUrl");
    }
}