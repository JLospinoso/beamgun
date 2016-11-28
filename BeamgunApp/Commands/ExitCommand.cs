using System;
using System.Windows;
using System.Windows.Input;
using BeamgunApp.ViewModel;

namespace BeamgunApp.Commands
{
    public interface IExiter
    {
        void Exit();
    }

    public class DefaultExiter : IExiter
    {
        public void Exit()
        {
            Application.Current.Shutdown();
        }
    }

    public class ExitCommand : ICommand
    {
        public IExiter Exiter { get; set; }

        private readonly IViewModel _viewModel;

        public ExitCommand(IViewModel viewModel)
        {
            _viewModel = viewModel;
            Exiter = new DefaultExiter();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.Reset();
            Exiter.Exit();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
