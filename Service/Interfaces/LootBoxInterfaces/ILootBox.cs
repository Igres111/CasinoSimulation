using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos.LootBoxDto;
using Service.Common;
using Service.Common.LootBoxResponses;

namespace Service.Interfaces.LootBoxInterfaces
{
    public interface ILootBox
    {
        public Task<APIResponse<string>> CreateLootBox(CreateLootBoxDto lootInfo);
        public Task<AllLootBoxResponse> GetAllLootBox();
        public Task<LootBoxItemsResponse> GetLootBoxItems(Guid lootBoxId);
    }
}
