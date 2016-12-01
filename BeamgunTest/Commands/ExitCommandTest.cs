using BeamgunApp.Commands;
using BeamgunApp.ViewModel;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace BeamgunTest.Commands
{
    [TestFixture]
    public class ExitCommandTest
    {
        [Test]
        public void CallsReset()
        {
            var viewModel = Mock.Of<IViewModel>();
            var command = new ExitCommand(viewModel) { Exiter = Mock.Of<IExiter>() };

            command.Execute(null);

            Mock.Get(viewModel).Verify(x => x.Reset());
        }

        [Test]
        public void CallsExit()
        {
            var viewModel = Mock.Of<IViewModel>();
            var command = new ExitCommand(viewModel) { Exiter = Mock.Of<IExiter>() };

            command.Execute(null);

            Mock.Get(command.Exiter).Verify(x => x.Exit());
        }

        [Test]
        public void IsExecutable()
        {
            var command = new DeactivatedCommand(Mock.Of<IViewModel>());

            command.CanExecute(null).ShouldBeTrue();
        }
    }
}
