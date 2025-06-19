using DataAccess.Entities;

namespace Service.Common.DigitalItemsResponses
{
    public class DigitalItemsResponse : APIResponse<string>
    {
        public List<DigitalItems> DigitalItems { get; set; }
    }
}
