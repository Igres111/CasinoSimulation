using Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common.UserResponses
{
    public class InventoryItemsResponse : APIResponse<string>
    {
      public List<UserInventoryDto> InventoryItems { get; set; }
    }
}
