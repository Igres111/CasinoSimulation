using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Dtos.UserDtos;
using Service.Common;
using Service.Common.LootBoxResponses;
using Service.Common.UserResponses;

namespace Service.Interfaces.UserInterfaces
{
    public interface IUser
    {
        public Task<APIResponse<string>> CreateUser(CreateUserDto userInfo);
        public Task<APIResponse<string>> LogInUser(LogInUserDto userInfo);
        public Task<GambledItemResponse> UserGamble(UserGambleDto betInfo);
        public Task<UserProfileResponse> UserProfile(Guid UserId);
        public Task<InventoryItemsResponse> UserInventory(Guid UserId);
        public Task<APIResponse<string>> SellItem(SellItemDto itemInfo);
        public Task<APIResponse<string>> UpdateProfile(UpdateProfileDto userInfo);
        public Task<APIResponse<string>> DeleteProfile(Guid UserId);
    }
}
