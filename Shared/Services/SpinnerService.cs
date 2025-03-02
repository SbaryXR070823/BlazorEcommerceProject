using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    using Microsoft.JSInterop;

    public class SpinnerService
    {
        private readonly IJSRuntime _jsRuntime;

        public SpinnerService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ShowSpinnerAsync()
        {
            await _jsRuntime.InvokeVoidAsync("loadingSpinner.show");
        }

        public async Task HideSpinnerAsync()
        {
            await _jsRuntime.InvokeVoidAsync("loadingSpinner.hide");
        }
    }

}
