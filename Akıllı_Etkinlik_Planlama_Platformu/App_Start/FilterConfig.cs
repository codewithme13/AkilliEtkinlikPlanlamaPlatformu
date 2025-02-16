using System.Web.Mvc;

namespace Akıllı_Etkinlik_Planlama_Platformu
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
