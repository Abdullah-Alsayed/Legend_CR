using DicomApp.DAL.DB;
using Microsoft.Extensions.Localization;

namespace DicomApp.CommonDefinitions.Requests
{
    public class BaseRequest
    {
        public ShippingDBContext context;
        public IStringLocalizer Localizer { get; set; }

        public const int DefaultPageSize = 20;
        public bool IsDesc { get; set; } = true;
        public string OrderByColumn { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }

        public int ID { get; set; }

        public int LanguageId { get; set; }

        public string BaseUrl { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public bool applyFilter { get; set; }
    }
}
