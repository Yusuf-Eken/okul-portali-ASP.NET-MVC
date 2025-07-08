using System.Web.Mvc;

namespace YusufEken.Web.Areas.Yonetim
{
    public class YonetimAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Yonetim";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Yonetim_Root",
            //    "Yonetim",
            //    new { controller = "Dashboard", action = "Index" }
            //);

            context.MapRoute(
                "Yonetim_default",
                "Yonetim/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}