using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Dtos.DigitalItemDto;
using Service.Common;

namespace Service.Interfaces.DigitalItemsInterfaces
{
    public interface IDigitalItems
    {
        public Task<APIResponse<string>> CreateDigitalItem(CreateDigitalItemsDto digitalItem);
        public Task<APIResponse<List<DigitalItems>>> GetAllDigitalItems();
    }
}
