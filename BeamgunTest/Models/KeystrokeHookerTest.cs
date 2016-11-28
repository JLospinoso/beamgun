using System;
using System.Threading;
using BeamgunApp.Models;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace BeamgunTest.Models
{
    [TestFixture]
    public class KeystrokeHookerTest
    {
        readonly Mutex _runMutex = new Mutex();

        [Test]
        public void ThrowsExceptionWhenInitializedTwice()
        {
            _runMutex.WaitOne();
            using (var hooker = new KeystrokeHooker())
            {
                Should.Throw<Exception>(() => new KeystrokeHooker());
            }
            _runMutex.ReleaseMutex();
        }

        [Test]
        public void CanRegisterCallback()
        {
            _runMutex.WaitOne();
            using (var hooker = new KeystrokeHooker())
            {
                hooker.Callback += key => { };
            }
            _runMutex.ReleaseMutex();
        }

        [Test]
        public void CanCreateTwoInSerial()
        {
            _runMutex.WaitOne();
            using (var hooker = new KeystrokeHooker())
            {
                hooker.Callback += key => { };
            }
            using (var hooker = new KeystrokeHooker())
            {
                hooker.Callback += key => { };
            }
            _runMutex.ReleaseMutex();
        }
    }
}
