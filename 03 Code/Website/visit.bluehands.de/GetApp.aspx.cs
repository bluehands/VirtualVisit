using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationInsights;

namespace visit.bluehands.de
{
    public partial class GetApp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var telemetry = new TelemetryClient();
            telemetry.TrackPageView("GetApp.aspx");
            var platform = GetPlatform();
            telemetry.TrackEvent(string.Format("Redirect_{0}", platform));
            string campaign = string.Empty;
            var values = Request.Params.GetValues("id");
            if (values != null)
            {
                var value = values.FirstOrDefault();
                if (!string.IsNullOrEmpty(value))
                {
                    campaign = value;
                }
            }
            telemetry.TrackEvent(string.Format("campaign_{0}", campaign));
            switch (platform)
            {
                case Platform.Unknown:
                    Response.Redirect("./index.html", true);
                    break;
                case Platform.iOs:
                    Response.Redirect("https://itunes.apple.com/de/app/stadtgeist-karlsruhe/id923381058", true);
                    break;
                case Platform.Android:
                    Response.Redirect("http://play.google.com/store/apps/details?id=de.bluehands.cityscope.karlsruhe", true);
                    break;
                case Platform.WP:
                    Response.Redirect("http://www.windowsphone.com/s?appid=c1744685-2f5b-495e-bd4e-0f82c0b33aa9", true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Platform GetPlatform()
        {
            var ua = Request.UserAgent;
            if (string.IsNullOrEmpty(ua))
            {
                return Platform.Unknown;
            }
            if (ua.IndexOf("iPhone", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                return Platform.iOs;
            }
            else if (ua.IndexOf("Android", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                return Platform.Android;
            }
            else if (ua.IndexOf("Windows Phone", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                return Platform.WP;
            }
            return Platform.Unknown;
        }

        private enum Platform
        {
            Unknown,
            iOs,
            Android,
            WP
        };
    }
}