using System.Web.Optimization;

namespace SFA.Apprenticeships.Web.Employer
{
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles. For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Content/_assets/js/vendor/jquery-1.11.0.min.js",
                "~/Content/_assets/js/scripts.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Content/_assets/js/vendor/modernizr-custom.js"));

            //bundles.Add(new Bundle("~/content/css/bundle").Include(
            //    "~/Content/_assets/css/main.css"));

            //bundles.Add(new Bundle("~/content/css/fonts").Include(
            //    "~/Content/_assets/css/fonts.css"));

            //bundles.Add(new Bundle("~/content/css/bundle-ie8").Include(
            //    "~/Content/_assets/css/main-ie8.css"));

            //bundles.Add(new Bundle("~/content/css/fonts-ie8").Include(
            //    "~/Content/_assets/css/fonts-ie8.css"));
        }
    }
}