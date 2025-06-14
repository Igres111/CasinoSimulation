using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.UserDtos
{
    public class SellItemDto
    {
        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; } 
    }
}
