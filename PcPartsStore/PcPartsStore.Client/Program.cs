using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http.Features;
using PcPartsStore.Client.Services;
using Shared.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddTransient<IProductApiService, ProductApiService>();
builder.Services.AddTransient<ICategoryApiService, CategoryApiService>();
builder.Services.AddTransient<IUserApiService, UserApiService>();
builder.Services.AddTransient<ISpecificationApiService, SpecificationApiService>();
builder.Services.AddTransient<ICartItemApiService, CartItemApiService>();
builder.Services.AddTransient<IOrderApiService, OrderApiService>();
builder.Services.AddTransient<IOrderItemApiService, OrderItemApiService>();
builder.Services.AddScoped<SpinnerService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 20 * 1024 * 1024 * 100;
});

var app = builder.Build();
await app.RunAsync();