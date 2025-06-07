using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dtos.LootBoxDto;

namespace Service.Common.LootBoxResponses
{
    public class AllLootBoxResponse : APIResponse<string>
    {
        public List<GetAllLootBoxDto> LootBoxes { get; set; } 
    }
}
