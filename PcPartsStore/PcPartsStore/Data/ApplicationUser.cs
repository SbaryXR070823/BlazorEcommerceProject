using Microsoft.AspNetCore.Identity;

namespace PcPartsStore.Data;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
}

