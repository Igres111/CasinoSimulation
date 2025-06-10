using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.UserDtos
{
    public class UserGambleDto
    {
        public Guid UserId { get; set; }
        public Guid LootBoxId { get; set; }
        public int Quantity { get; set; }
    }
}
