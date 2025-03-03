using Shared.Dto;

namespace PcPartsStore.Client.Services
{
    public interface IUserApiService
    {
        Task<List<string>> GetUserRoleAsync();
        Task<UserInfoDto> GetUserInfoAsync();
    }
}
