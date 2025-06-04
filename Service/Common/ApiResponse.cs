using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common
{
    public class APIResponse<T>
    {
        public bool Success { get; set; }
        public string Error { get; set; } = string.Empty;
        public T Data { get; set; }
    }
}
