using SupplyChain.Common.Models;
using System.Threading.Tasks;

namespace SupplyChain.Common.Services
{
    public interface IUserProfileService
    {
        Task<string> CreateAndUploadUserProfile(string name, string email);
        Task DeleteUserProfile(string id);
        Task<UserProfile> GetUserProfile(string id);
        Task<bool> UpdateUserProfile(string id, UserProfile userProfile, string newName, string newEmail);
    }
}
