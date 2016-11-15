using System;
using System.Threading;
using BeamgunApp.Models;

namespace BeamgunApp
{
    class Alarm
    {
        private readonly BeamgunState _beamgunState;
        private readonly int _repeatInterval;
        public Action AlarmCallback;
        private bool _triggered;

        public Alarm(uint repeatInterval, BeamgunState beamgunState)
        {
            _beamgunState = beamgunState;
            _repeatInterval = (int) repeatInterval;
            _triggered = false;
        }

        public bool Triggered => _triggered;

        public void Trigger(string message)
        {
            if (_triggered) return;
            _beamgunState.AppendToAlert(message);
            _triggered = true;
            var triggerThread = new Thread(() =>
            {
                while (_triggered)
                {
                    AlarmCallback?.Invoke();
                    Thread.Sleep(_repeatInterval);
                }
            });
            triggerThread.Start();
        }

        public void Reset()
        {
            _triggered = false;
        }
    }
}
