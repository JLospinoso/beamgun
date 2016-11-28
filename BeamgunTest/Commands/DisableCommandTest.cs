using System;
using BeamgunApp.Commands;
using BeamgunApp.Models;
using BeamgunApp.ViewModel;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace BeamgunTest.Commands
{
    [TestFixture]
    public class DisableCommandTest
    {

        [Test]
        public void CallsReset()
        {
            var viewModel = Mock.Of<IViewModel>();
            var settings = Mock.Of<IBeamgunSettings>();
            var command = new DisableCommand(viewModel, settings);

            command.Execute(null);

            Mock.Get(viewModel).Verify(x => x.Reset());
        }

        [Test]
        public void CallsDisable()
        {
            const double waitTime = 100;
            var viewModel = Mock.Of<IViewModel>();
            var settings = Mock.Of<IBeamgunSettings>(x => x.DisableTime == waitTime);
            var command = new DisableCommand(viewModel, settings);

            command.Execute(null);

            var lower = DateTime.Now.AddMinutes(waitTime-1);
            var upper = DateTime.Now.AddMinutes(waitTime+1);
            Mock.Get(viewModel).Verify(x => x.DisableUntil(It.IsInRange(lower, upper, Range.Exclusive)));
        }

        [Test]
        public void IsExecutable()
        {
            var command = new DisableCommand(Mock.Of<IViewModel>(), Mock.Of<IBeamgunSettings>());

            command.CanExecute(null).ShouldBeTrue();
        }
    }
}
