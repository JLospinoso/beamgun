using System;
using BeamgunApp.Commands;
using BeamgunApp.ViewModel;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace BeamgunTest.Commands
{
    [TestFixture]
    public class ResetCommandTest
    {
        [Test]
        public void CallsReset()
        {
            var viewModel = Mock.Of<IViewModel>();
            var command = new ResetCommand(viewModel);

            command.Execute(null);

            Mock.Get(viewModel).Verify(x => x.Reset());
        }
        
        [Test]
        public void IsExecutable()
        {
            var command = new ResetCommand(Mock.Of<IViewModel>());

            command.CanExecute(null).ShouldBeTrue();
        }
    }
}
