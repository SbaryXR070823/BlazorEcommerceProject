using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PcPartsStore.Data;
using System.Threading;
using System.Threading.Tasks;

namespace PcPartsStore.Components.Account;

internal sealed class IdentityUserAccessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IdentityRedirectManager _redirectManager;
    private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public IdentityUserAccessor(
        IServiceScopeFactory scopeFactory,
        IdentityRedirectManager redirectManager)
    {
        _scopeFactory = scopeFactory;
        _redirectManager = redirectManager;
    }

    public async Task<Data.ApplicationUser> GetRequiredUserAsync(HttpContext context)
    {
        try
        {
            await _semaphore.WaitAsync();

            using var scope = _scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Data.ApplicationUser>>();

            var user = await userManager.GetUserAsync(context.User);
            if (user is null)
            {
                _redirectManager.RedirectToWithStatus("Account/InvalidUser",
                    $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.",
                    context);
            }
            return user;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}