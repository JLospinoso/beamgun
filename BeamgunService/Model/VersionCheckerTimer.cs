using System;
using System.Reflection;
using System.Threading;

namespace BeamgunService.Model
{
    public class VersionCheckerTimer : IDisposable
    {
        private readonly Timer _timer;

        public VersionCheckerTimer(IBeamgunSettings beamgunSettings, VersionChecker checker, Action<string> report)
        {
            _timer = new Timer(state =>
            {
                try
                {
                    checker.Update(beamgunSettings);
                    var availableVersion = beamgunSettings.LatestVersion;
                    var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    report(availableVersion > currentVersion
                            ? $"Version {availableVersion} is available at {beamgunSettings.DownloadUrl}"
                            : "Beamgun is up to date.");
                }
                catch (Exception e)
                {
                    report($"Unable to connect to update server. {e.Message}");
                }
            }, null, 1000, beamgunSettings.UpdatePollInterval);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
