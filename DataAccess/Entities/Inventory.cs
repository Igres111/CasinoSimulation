using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
        public class Inventory:BaseEntity
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public User User { get; set; }
            public Guid DigitalItemId { get; set; }
            public DigitalItems DigitalItem { get; set; }
            public int Quantity { get; set; }
        }

}
