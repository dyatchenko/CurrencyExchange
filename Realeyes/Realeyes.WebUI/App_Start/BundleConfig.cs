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
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(
                new StyleBundle("~/Content/css").Include("~/Content/Rickshaw/rickshaw.min.css")
                    .Include("~/Content/*.css"));
            bundles.Add(new StyleBundle("~/Content/JQuery/css").Include("~/Content/JQuery/*.css"));
                    
            bundles.Add(
                new ScriptBundle("~/bundles/scripts").Include(
                    "~/Scripts/JQuery/jquery-1.11.1.min.js",
                    "~/Scripts/JQuery/jquery.ui.core.min.js",
                    "~/Scripts/JQuery/jquery.ui.widget.min.js",
                    "~/Scripts/JQuery/jquery.ui.datepicker.min.js",
                    "~/Scripts/JQuery/jquery.ui.button.min.js",
                    "~/Scripts/JQuery/jquery.ui.spinner.min.js",
                    "~/Scripts/Rickshaw/Vendor/d3.min.js",
                    "~/Scripts/Rickshaw/Vendor/d3.layout.min.js",
                    "~/Scripts/Rickshaw/rickshaw.js"));
            //BundleTable.EnableOptimizations = true;
        }
    }
}