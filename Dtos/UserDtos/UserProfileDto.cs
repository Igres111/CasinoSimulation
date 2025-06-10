using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.UserDtos
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PreferredLanguage { get; set; }
        public string? AvatarUrl { get; set; }
        public int TotalBoxesOpened { get; set; }
        public decimal Balance { get; set; }
        public int BonusPoints { get; set; }
    }
}
