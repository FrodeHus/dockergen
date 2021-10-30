using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DockerGen.Infrastructure
{
    public class ClipboardService
    {
        private readonly IJSRuntime _runtime;

        public ClipboardService(IJSRuntime jsRuntime)
        {
            _runtime = jsRuntime;
        }

        public ValueTask WriteTextAsync(string text)
        {
            return _runtime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }

    }
}
