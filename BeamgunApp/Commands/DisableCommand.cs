using System;
using System.Windows.Input;
using BeamgunApp.ViewModel;

namespace BeamgunApp.Commands
{
    public class DisableCommand : ICommand
    {
        private readonly IViewModel _viewModel;

        public DisableCommand(IViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.Reset();
            _viewModel.DisableUntil(DateTime.Now.AddMinutes(30));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
