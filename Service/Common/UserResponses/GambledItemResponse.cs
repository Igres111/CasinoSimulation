using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Dtos.UserDtos;

namespace Service.Common.UserResponses
{
    public class GambledItemResponse:APIResponse<string>
    {
        public UserGambledItemDto DigitalItem { get; set; }
    }
}
