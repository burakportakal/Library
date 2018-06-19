using System.Web;
using System.Web.Optimization;

namespace Library.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~///Scripts/jquery-3.3.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/index").Include(
                "~///Scripts/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/date").Include(
                "~///Scripts/date.format.js"));

            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                "~///Scripts/layout.js"));

            bundles.Add(new ScriptBundle("~/bundles/profile").Include(
                "~///Scripts/profile.js"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~///Scripts/login-register.js"));

            bundles.Add(new ScriptBundle("~/bundles/storage").Include(
                "~///Scripts/storage.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~///Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~///Scripts/bootstrap.min.js",
                      "~///Scripts/respond.js", "~///Scripts/meterialize.min.js", "~///Scripts/popper.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~///Content/bootstrap.min.css",
                      "~///Content/site.css"));
        }
    }
}
