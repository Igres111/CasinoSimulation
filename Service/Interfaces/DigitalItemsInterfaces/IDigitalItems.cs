﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Dtos.DigitalItemDto;
using Service.Common;
using Service.Common.DigitalItemsResponses;

namespace Service.Interfaces.DigitalItemsInterfaces
{
    public interface IDigitalItems
    {
        public Task<APIResponse<string>> CreateDigitalItem(CreateDigitalItemsDto digitalItem);
        public Task<DigitalItemsResponse> GetAllDigitalItems();
        public Task<APIResponse<string>> UpdateItem(UpdateItemDto itemInfo);
        public Task<APIResponse<string>> DeleteItem(Guid itemId);
    }
}
