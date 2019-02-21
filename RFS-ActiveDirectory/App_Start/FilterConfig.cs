using System.Web;
using System.Web.Mvc;

namespace RFS_ActiveDirectory
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
