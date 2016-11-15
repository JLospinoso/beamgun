using System;
using System.Windows;
using System.Windows.Input;
using BeamgunApp.ViewModel;

namespace BeamgunApp.Commands
{
    public class DeactivatedCommand : ICommand
    {
        private readonly BeamgunViewModel _viewModel;

        public DeactivatedCommand(BeamgunViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.BeamgunState.MainWindowVisibility = Visibility.Hidden;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
