using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace E_santeFrontend.Helpers
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask<string> GetItemAsync(string key) => _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        public ValueTask SetItemAsync(string key, string value) => _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        public ValueTask RemoveItemAsync(string key) => _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}
