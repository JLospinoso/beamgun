using BeamgunApp.Commands;
using BeamgunApp.ViewModel;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace BeamgunTest.Commands
{
    [TestFixture]
    public class DeactivatedCommandTest
    {
        [Test]
        public void SetsInvisible()
        {
            var viewModel = Mock.Of<IViewModel>();
            var command = new DeactivatedCommand(viewModel);

            command.Execute(null);

            Mock.Get(viewModel).VerifySet(x => x.IsVisible = false);
        }

        [Test]
        public void IsExecutable()
        {
            var command = new DeactivatedCommand(Mock.Of<IViewModel>());

            command.CanExecute(null).ShouldBeTrue();
        }
    }
}
