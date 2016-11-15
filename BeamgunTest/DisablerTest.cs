using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using BeamgunApp.Models;
using NUnit.Framework;

namespace BeamgunTest
{
    [TestFixture]
    public class DisablerTest
    {
        [Test]
        public void EnableSetsIconPath()
        {
            var state = new BeamgunState();
            var disabler = new Disabler(state);

            disabler.Enable();

            Assert.NotNull(state.TrayIconPath);
        }

        [Test]
        public void IsNotDisabledAfterEnable()
        {
            var state = new BeamgunState();
            var disabler = new Disabler(state);

            disabler.Enable();

            Assert.AreEqual(false, disabler.IsDisabled);
        }

        [Test]
        public void HasZeroDisabledTimeAfterEnable()
        {
            var state = new BeamgunState();
            var disabler = new Disabler(state);

            disabler.Enable();

            Assert.AreEqual(0, disabler.DisabledTime);
        }

        [Test]
        public void IsNotDisabledAfterConstruction()
        {
            var state = new BeamgunState();

            var disabler = new Disabler(state);
            
            Assert.AreEqual(false, disabler.IsDisabled);
        }

        [Test]
        public void HasNonZeroDisabledTimeAfterDisabled()
        {
            var state = new BeamgunState();
            var disabler = new Disabler(state);

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(50));

            Assert.AreNotEqual(0, disabler.DisabledTime);
        }

        [Test]
        public void IsDisabledAfterDisabled()
        {
            var state = new BeamgunState();
            var disabler = new Disabler(state);

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(50));

            Assert.AreEqual(true, disabler.IsDisabled);
        }

        [Test]
        public void WindowNotVisibleAfterDisabled()
        {
            var state = new BeamgunState();
            var disabler = new Disabler(state);

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(50));

            Assert.AreEqual(Visibility.Hidden, state.MainWindowVisibility);
        }

        [Test]
        public void TrayPathHasIconAfterDisabled()
        {
            var state = new BeamgunState();
            var disabler = new Disabler(state);

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(50));

            Assert.IsNotNull(state.TrayIconPath);
        }

        [Test]
        public void HasZeroDisabledTimeAfterDisabledAndWait()
        {
            var state = new BeamgunState();
            var disabler = new Disabler(state);
            var wait = 10;

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(wait));
            Thread.Sleep(wait + 1);

            Assert.AreEqual(0, disabler.DisabledTime);
        }

        [Test]
        public void IsNotDisabledTimeAfterDisabledAndWait()
        {
            var state = new BeamgunState();
            var disabler = new Disabler(state);
            var wait = 10;

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(wait));
            Thread.Sleep(wait + 1);

            Assert.AreEqual(false, disabler.IsDisabled);
        }
    }
}
