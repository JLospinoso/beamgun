using System;
using System.IO;
using System.Net;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace BeamgunApp.Models
{
    public class VersionChecker
    {
        public readonly Version LatestVersion;
        public readonly string DownloadUrl;
        private const string DefaultUrl = "https://s3.amazonaws.com/net.lospi.beamgun/version.json";
        private const string BeamgunBaseKey = "HKEY_CURRENT_USER\\SOFTWARE\\Beamgun";
        private const string VersionUrlSubkey = "VersionUrl";

        public VersionChecker()
        {
            JObject versionJson;
            var url = (string)Registry.GetValue(BeamgunBaseKey, VersionUrlSubkey, DefaultUrl);
            using (var client = new WebClient())
            {
                using (var data = client.OpenRead(url))
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
            LatestVersion = new Version(versionJson["latest_version"].ToString());
            DownloadUrl = versionJson["download_url"].ToString();
            Registry.SetValue(BeamgunBaseKey, VersionUrlSubkey, versionJson["update_version_url"].ToString());
        }
    }
}
