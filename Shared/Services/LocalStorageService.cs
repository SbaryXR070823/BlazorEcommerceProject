using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services;

public class LocalStorageService
{
    private readonly ILocalStorageService _localStorage;

    public LocalStorageService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SaveItemAsync<T>(string key, T value)
    {
        await _localStorage.SetItemAsync(key, value);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        return await _localStorage.GetItemAsync<T>(key);
    }

    public async Task RemoveItemAsync(string key)
    {
        await _localStorage.RemoveItemAsync(key);
    }

    public async Task<bool> ContainsKeyAsync(string key)
    {
        return await _localStorage.ContainKeyAsync(key);
    }

    public async Task ClearAllAsync()
    {
        await _localStorage.ClearAsync();
    }
}
