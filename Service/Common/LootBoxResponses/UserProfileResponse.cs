using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos.UserDtos;

namespace Service.Common.LootBoxResponses
{
    public class UserProfileResponse : APIResponse<string>
    {
        public UserProfileDto UserProfile { get; set; }
    }
}
