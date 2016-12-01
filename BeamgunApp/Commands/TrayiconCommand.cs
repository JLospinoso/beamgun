using System;
using System.Windows.Input;
using BeamgunApp.ViewModel;

namespace BeamgunApp.Commands
{
    public class TrayIconCommand : ICommand
    {
        private readonly IViewModel _viewModel;

        public TrayIconCommand(IViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.IsVisible = !_viewModel.IsVisible;
            if (_viewModel.IsVisible)
            {
                _viewModel.DoStealFocus();
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
