using System;
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
            JObject versionJson;
            using (var client = new WebClient())
            {
                var url = settings.VersionUrl + "?id=" + settings.BeamgunId + "&ver=" + Assembly.GetExecutingAssembly().GetName().Version;
                var uri = Uri.EscapeUriString(url);
                using (var data = client.OpenRead(uri))
                {
                    if (data == null)
                    {
                        throw new Exception("Could not connect to update server.");
                    }
                    using (var reader = new StreamReader(data))
                    {
                        versionJson = JObject.Parse(reader.ReadToEnd());
                    }
                }
            }
            try
            {
                settings.LatestVersion = new Version(versionJson["latest_version"].ToString());
                settings.DownloadUrl = versionJson["download_url"].ToString();
                settings.VersionUrl = versionJson["update_version_url"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception("Could not parse update results from server.", e);
            }
        }
    }
}
