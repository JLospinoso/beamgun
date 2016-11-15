using System;
using System.Windows;
using System.Windows.Input;
using BeamgunApp.ViewModel;

namespace BeamgunApp.Commands
{
    public class TrayIconCommand : ICommand
    {
        private readonly BeamgunViewModel _viewModel;

        public TrayIconCommand(BeamgunViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var isVisible = _viewModel.BeamgunState.MainWindowVisibility == Visibility.Visible;
            _viewModel.BeamgunState.MainWindowVisibility = isVisible ? Visibility.Hidden : Visibility.Visible;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
