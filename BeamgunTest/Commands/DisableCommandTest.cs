using System;
using BeamgunApp.Commands;
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
            var command = new DisableCommand(viewModel);

            command.Execute(null);

            Mock.Get(viewModel).Verify(x => x.Reset());
        }

        [Test]
        public void CallsDisable()
        {
            var viewModel = Mock.Of<IViewModel>();
            var command = new DisableCommand(viewModel);

            command.Execute(null);

            var lower = DateTime.Now.AddMinutes(29);
            var upper = DateTime.Now.AddMinutes(31);
            Mock.Get(viewModel).Verify(x => x.DisableUntil(It.IsInRange(lower, upper, Range.Exclusive)));
        }

        [Test]
        public void IsExecutable()
        {
            var command = new DisableCommand(Mock.Of<IViewModel>());

            command.CanExecute(null).ShouldBeTrue();
        }
    }
}
