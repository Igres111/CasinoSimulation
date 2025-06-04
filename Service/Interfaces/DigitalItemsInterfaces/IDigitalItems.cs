using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Dtos.DigitalItemDto;

namespace Service.Interfaces.DigitalItemsInterfaces
{
    public interface IDigitalItems
    {
        public Task CreateDigitalItem(CreateDigitalItemsDto digitalItem);
        public Task<List<DigitalItems>> GetAllDigitalItems();
    }
}
