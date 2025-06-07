using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common.LootBoxResponses
{
    public class LootBoxItemsResponse:APIResponse<string>
    {
        public List<LootBoxDigitalItem> LootBoxItems { get; set; }
    }
}
