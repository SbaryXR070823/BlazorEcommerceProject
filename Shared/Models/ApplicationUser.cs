using Microsoft.AspNetCore.Identity;

namespace PcPartsStore;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
}

