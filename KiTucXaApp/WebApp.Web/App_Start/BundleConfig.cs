using System.Web.Optimization;

namespace WebApp.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // ***********************************
            // ************ Jquery public ***************
            // ***********************************
            bundles.Add(new ScriptBundle("~/scripts/jquery-public").Include(
                "~/Contents/scripts/jquery-1.10.2.js",
                "~/Contents/scripts/bootstrap.js",
                "~/Contents/scripts/owl.carousel.js",
                "~/Contents/scripts/swiper.js",
                "~/Contents/scripts/toastr.js",
                "~/Contents/scripts/template.js"
                ));


            // ***********************************
            // ************* Style public ***************
            // ***********************************

            bundles.Add(new StyleBundle("~/styles/css-public").Include(
                "~/Contents/styles/owl.carousel.css",
                "~/Contents/styles/swiper.css",
                "~/Contents/styles/toastr.css",
                "~/Contents/styles/template-desktop.css",
                "~/Contents/styles/template-mobile.css"
                ));

        }
    }
}
