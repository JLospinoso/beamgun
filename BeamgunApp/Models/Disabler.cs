using System;
using System.Threading;
using System.Windows;

namespace BeamgunApp.Models
{
    public class Disabler
    {
        public Disabler(IBeamgunState beamgunState)
        {
            _beamgunState = beamgunState;
        }

        public int DisabledTime
        {
            get
            {
                if (_disableUntil.HasValue)
                {
                    var difference = Math.Round(_disableUntil.Value.Subtract(DateTime.Now).TotalMilliseconds);
                    if (difference > 0) return (int) difference;
                }
                _disableUntil = null;
                return 0;
            }
        }

        public bool IsDisabled => DisabledTime > 0;

        public void DisableUntil(DateTime dateTime)
        {
            _disableUntil = dateTime;
            _beamgunState.MainWindowVisibility = Visibility.Hidden;
            _beamgunState.SetGraphicsDisabled();
            var monitor = new Thread(() =>
            {
                _beamgunState.AppendToAlert($"Beamgun is disabled until {dateTime}");
                Thread.Sleep(dateTime - DateTime.Now);
                Enable();
                _beamgunState.AppendToAlert("Beamgun is enabled.");
            });
            monitor.Start();
        }

        public void Enable()
        {
            _disableUntil = null;
            _beamgunState.SetGraphicsArmed();
        }

        private DateTime? _disableUntil;
        private readonly IBeamgunState _beamgunState;
    }
}
