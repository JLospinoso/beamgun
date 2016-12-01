using BeamgunApp.Commands;
using BeamgunApp.ViewModel;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace BeamgunTest.Commands
{
    [TestFixture]
    public class TrayIconCommandTest
    {
        [Test]
        public void VisibleViewModelSetsInvisible()
        {
            var viewModel = Mock.Of<IViewModel>(x => x.IsVisible == false);
            var command = new TrayIconCommand(viewModel);

            command.Execute(null);

            Mock.Get(viewModel).VerifySet(x => x.IsVisible = true);
        }

        [Test]
        public void InvisibleViewModelSetsVisible()
        {
            var viewModel = Mock.Of<IViewModel>(x => x.IsVisible == false);
            var command = new TrayIconCommand(viewModel);

            command.Execute(null);

            Mock.Get(viewModel).VerifySet(x => x.IsVisible = true);
        }

        [Test]
        public void InvisibleViewModelStealsFocus()
        {
            var viewModel = Mock.Of<IViewModel>(x => x.IsVisible == false);
            var command = new TrayIconCommand(viewModel);

            command.Execute(null);

            Mock.Get(viewModel).Verify(x => x.DoStealFocus());
        }

        [Test]
        public void IsExecutable()
        {
            var command = new TrayIconCommand(Mock.Of<IViewModel>());

            command.CanExecute(null).ShouldBeTrue();
        }
    }
}
