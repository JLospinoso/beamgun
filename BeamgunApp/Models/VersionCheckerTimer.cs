using System;
using System.Reflection;
using System.Threading;

namespace BeamgunApp.Models
{
    public class VersionCheckerTimer : IDisposable
    {
        private readonly Timer _timer;

        public VersionCheckerTimer(IBeamgunSettings beamgunSettings, VersionChecker checker, Action<string> report)
        {
            var autoEvent = new AutoResetEvent(false);
            _timer = new Timer(state =>
            {
                lock (_timer)
                {
                    try
                    {
                        checker.Update(beamgunSettings);
                        var availableVersion = beamgunSettings.LatestVersion;
                        var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                        report(availableVersion > currentVersion
                                ? $"Version {availableVersion} is available at {beamgunSettings.DownloadUrl}"
                                : $"Beamgun is up to date.");
                    }
                    catch (Exception e)
                    {
                        report($"Unable to connect to update server. {e.Message}");
                    }
                }
            }, autoEvent, 0, beamgunSettings.UpdatePollInterval);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
