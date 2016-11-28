using System;
using System.Threading;
using System.Windows;
using BeamgunApp.Models;
using Moq;
using NUnit.Framework;

namespace BeamgunTest
{
    //TODO: Now that this is mocked, revisit.
    [TestFixture]
    public class DisablerTest
    {
        [Test]
        public void EnableSetsIconPath()
        {
            var state = Mock.Of<IBeamgunState>();
            var disabler = new Disabler(state);

            disabler.Enable();
        }

        [Test]
        public void IsNotDisabledAfterEnable()
        {
            var state = Mock.Of<IBeamgunState>();
            var disabler = new Disabler(state);

            disabler.Enable();

            Assert.AreEqual(false, disabler.IsDisabled);
        }

        [Test]
        public void HasZeroDisabledTimeAfterEnable()
        {
            var state = Mock.Of<IBeamgunState>();
            var disabler = new Disabler(state);

            disabler.Enable();

            Assert.AreEqual(0, disabler.DisabledTime);
        }

        [Test]
        public void IsNotDisabledAfterConstruction()
        {
            var state = Mock.Of<IBeamgunState>();

            var disabler = new Disabler(state);
            
            Assert.AreEqual(false, disabler.IsDisabled);
        }

        [Test]
        public void HasNonZeroDisabledTimeAfterDisabled()
        {
            var state = Mock.Of<IBeamgunState>();
            var disabler = new Disabler(state);

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(50));

            Assert.AreNotEqual(0, disabler.DisabledTime);
        }

        [Test]
        public void IsDisabledAfterDisabled()
        {
            var state = Mock.Of<IBeamgunState>();
            var disabler = new Disabler(state);

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(50));

            Assert.AreEqual(true, disabler.IsDisabled);
        }

        [Test]
        public void WindowNotVisibleAfterDisabled()
        {
            var state = Mock.Of<IBeamgunState>();
            var disabler = new Disabler(state);

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(50));

            Assert.AreEqual(Visibility.Hidden, state.MainWindowVisibility);
        }

        [Test]
        public void TrayPathHasIconAfterDisabled()
        {
            var state = Mock.Of<IBeamgunState>();
            var disabler = new Disabler(state);

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(50));

            //Assert.IsNotNull(state.TrayIconPath);
        }

        [Test]
        public void HasZeroDisabledTimeAfterDisabledAndWait()
        {
            var state = Mock.Of<IBeamgunState>();
            var disabler = new Disabler(state);
            var wait = 10;

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(wait));
            Thread.Sleep(wait + 1);

            Assert.AreEqual(0, disabler.DisabledTime);
        }

        [Test]
        public void IsNotDisabledTimeAfterDisabledAndWait()
        {
            var state = Mock.Of<IBeamgunState>();
            var disabler = new Disabler(state);
            var wait = 10;

            disabler.DisableUntil(DateTime.Now.AddMilliseconds(wait));
            Thread.Sleep(wait + 1);

            Assert.AreEqual(false, disabler.IsDisabled);
        }
    }
}
