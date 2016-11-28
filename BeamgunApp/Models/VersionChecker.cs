using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace BeamgunApp.Models
{
    public class VersionChecker
    {
        public void Update(IBeamgunSettings settings)
        {
            JObject versionJson;
            using (var client = new WebClient())
            {
                using (var data = client.OpenRead(settings.VersionUrl))
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
