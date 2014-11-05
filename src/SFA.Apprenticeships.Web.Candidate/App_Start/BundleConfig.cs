using System.Web.Optimization;

namespace SFA.Apprenticeships.Web.Candidate
{
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles. For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery", "//cdnjs.cloudflare.com/ajax/libs/jquery/1.11.1/jquery.min.js").Include(
                "~/Content/_assets/js/vendor/jquery-1.11.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/fastclick", "//cdnjs.cloudflare.com/ajax/libs/fastclick/1.0.3/fastclick.min.js").Include(
                "~/Content/_assets/js/vendor/fastclick-1.0.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/underscore", "//cdnjs.cloudflare.com/ajax/libs/underscore.js/1.7.0/underscore-min.js").Include(
                "~/Content/_assets/js/vendor/underscore-1.7.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/nascript").Include(
                  "~/Content/_assets/js/scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(              
                "~/Content/_assets/js/vendor/jquery.validate.js",
                "~/Content/_assets/js/vendor/jquery.validate.unobtrusive.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Content/_assets/js/vendor/knockout-3.1.0.js",
                "~/Content/_assets/js/vendor/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas").Include(
                "~/Content/_assets/js/nas/lookupService.js",
                "~/Content/_assets/js/nas/validationscripts.js",
                "~/Content/_assets/js/nas/webTrendsInlineTrack.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/passwordstrength").Include(
                "~/Content/_assets/js/vendor/zxcvbn-async.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/applicationform").Include(
                "~/Content/_assets/js/nas/applicationform.js",
                "~/Content/_assets/js/nas/dirtyFormDialog.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/locationsearch").Include(
                "~/Content/_assets/js/vendor/jquery-ui-1.10.4.custom.min.js",
                "~/Content/_assets/js/nas/locationAutocomplete.js"));
        }
    }
}
