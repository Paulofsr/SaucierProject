using System.Web;
using System.Web.Optimization;

namespace SaucierWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.2.3.min.js",
                        "~/Scripts/jquery-ui-1.11.4.min.js",
                        //"~/Scripts/jquery.modal.min.js",
                        //"~/Scripts/highlight.pack.js",
                        //"~/Scripts/animatedModal.min.js",
                        "~/Scripts/jquery.bpopup.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      //"~/Scripts/respond.m*",
                      //"~/Scripts/animatedModal.min.js",
                      //"~/Scripts/plugins.js",
                      "~/Scripts/main.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.theme.css",
                      //"~/Content/jquery.modal.css",
                      "~/Content/site.css",
                      //"~/Content/github.css",
                      //"~/Content/animate.min.css",
                      "~/Content/normalize.min.css",
                      "~/Content/style.min.css"
                      ));
        }
    }
}
