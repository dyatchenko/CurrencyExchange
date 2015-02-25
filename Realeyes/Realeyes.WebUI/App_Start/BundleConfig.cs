namespace Realeyes.WebUI.App_Start
{
    using System;
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/*.css"));
            bundles.Add(
                new ScriptBundle("~/bundles/scripts").Include(
                    "~/Scripts/jquery-1.11.1.min.js",
                    "~/Scripts/jquery.ui.core.min.js",
                    "~/Scripts/jquery.ui.widget.min.js",
                    "~/Scripts/jquery.ui.datepicker.min.js",
                    "~/Scripts/jquery.ui.button.min.js",
                    "~/Scripts/jquery.ui.spinner.min.js"));
            //BundleTable.EnableOptimizations = true;
        }
    }
}