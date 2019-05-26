using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace KalamazoDefteri.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/projStyles")
                .Include("~/Content/css/bootstrap-grid.css")
                .Include("~/Content/css/bootstrap-grid.min.css")
                .Include("~/Content/css/bootstrap-reboot.css")
                .Include("~/Content/css/bootstrap.css")
                .Include("~/Content/css/bootstrap.min.css")
                .Include("~/Content/css/default-css.css")
                .Include("~/Content/css/font-awesome.min.css")
                .Include("~/Content/css/metisMenu.css")
                .Include("~/Content/css/owl.carousel.min.css")
                .Include("~/Content/css/responsive.css")
                .Include("~/Content/css/slicknav.min.css")
                .Include("~/Content/css/styles.css")
                .Include("~/Content/css/themify-icons.css")
                .Include("~/Content/css/typography.css")
                .Include("~/Content/css/export.css")
                .Include("~/Content/css/dataTables.bootstrap4.min.css")
                .Include("~/Content/css/jquery.dataTables.css")
                .Include("~/Content/css/responsive.bootstrap.min.css")
                .Include("~/Content/css/responsive.jqueryui.min.css")
                );

            //bundles.Add(new ScriptBundle("~/projScripts").IncludeDirectory("~/Content/js", "*.js", true));
            bundles.Add(new ScriptBundle("~/projScripts")
                .Include("~/Content/js/vendor/jquery-2.2.4.min.js")
                .Include("~/Content/js/vendor/modernizr-2.8.3.min.js")
                .Include("~/Content/js/amcharts.js")
                .Include("~/Content/js/ammap.js")
                .Include("~/Content/js/bar-chart.js")
                .Include("~/Content/js/bootstrap.min.js")
                .Include("~/Content/js/Chart.min.js")
                .Include("~/Content/js/export-data.js")
                .Include("~/Content/js/export.min.js")
                .Include("~/Content/js/exporting.js")
                .Include("~/Content/js/jquery.slicknav.min.js")
                .Include("~/Content/js/jquery.slimscroll.min.js")
                .Include("~/Content/js/light.js")
                .Include("~/Content/js/line-chart.js")
                .Include("~/Content/js/maps.js")
                .Include("~/Content/js/metisMenu.min.js")
                .Include("~/Content/js/owl.carousel.min.js")
                .Include("~/Content/js/pie-chart.js")
                .Include("~/Content/js/plugins.js")
                .Include("~/Content/js/popper.min.js")
                .Include("~/Content/js/scripts.js")
                .Include("~/Content/js/serial.js")
                .Include("~/Content/js/worldLow.js")
                .Include("~/Content/js/datatable.js")
                .Include("~/Content/js/dataTables.min.js")
                .Include("~/Content/js/dataTables-responsive.min.js")
                .Include("~/Content/js/dataTables.bootstrap4.min.js")
                .Include("~/Content/js/responsive.bootstrap.min.js")
                .Include("~/Areas/Admin/Controllers/Scripts/Form.js")
                .Include("~/Content/js/inputmask/inputmask.js",
                "~/Content/js/inputmask/jquery.inputmask.js",
                "~/Content/js/inputmask/inputmask.extensions.js",
                "~/Content/js/inputmask/inputmask.date.extensions.js",
                "~/Content/js/inputmask/inputmask.numeric.extensions.js"));

        }
    }
}