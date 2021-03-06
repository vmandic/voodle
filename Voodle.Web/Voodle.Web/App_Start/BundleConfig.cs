﻿using System.Web;
using System.Web.Optimization;

namespace Voodle.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //////////////////////////
            // SCRIPTS - JAVASCRIPT //
            //////////////////////////
            #region Scripts
            bundles.Add(new ScriptBundle("~/js/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/js/app").Include
            (
                //GENERIC AND API SCRIPTS
                       "~/Scripts/jquery-{version}.js",
                       "~/Scripts/bootstrap.js",
                       "~/Scripts/respond.js",
                       "~/Scripts/toastr.js",

                //SCRIPTS APP
                       "~/ScriptsApp/base.js"
           ));

            #endregion

            //////////////////////////
            // STYLES - CSS //////////
            //////////////////////////

            #region Styles
            bundles.Add(new StyleBundle("~/css/app").Include
            (
                //GENERIC AND API STYLES
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/toastr.css",

                //STYLES APP
                      "~/StylesApp/base.css"
            ));
            #endregion

            // enable optimizations, false is when debug, true for deployment
            BundleTable.EnableOptimizations = false;
        }
    }
}
