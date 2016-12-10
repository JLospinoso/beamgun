using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace BeamgunApp.Models
{
    public class VersionChecker
    {
        public void Update(IBeamgunSettings settings)
        {
            if (!settings.CheckForUpdates) return;
            using (var client = new WebClient())
            {
                var url = settings.VersionUrl + "?id=" + settings.BeamgunId + "&ver=" + Assembly.GetExecutingAssembly().GetName().Version;
                var requestTask = client.DownloadStringTaskAsync(url);
                requestTask.ContinueWith(x =>
                {
                    try
                    {
                        var versionJson = JObject.Parse(x.Result);
                        settings.LatestVersion = new Version(versionJson["latest_version"].ToString());
                        settings.DownloadUrl = versionJson["download_url"].ToString();
                        settings.VersionUrl = versionJson["update_version_url"].ToString();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"Unable to retrieve version information: {e.Message}");
                    }
                });
            }
        }
    }
}
