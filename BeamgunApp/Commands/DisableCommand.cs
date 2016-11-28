using System;
using System.Windows.Input;
using BeamgunApp.Models;
using BeamgunApp.ViewModel;

namespace BeamgunApp.Commands
{
    public class DisableCommand : ICommand
    {
        public DisableCommand(IViewModel viewModel, IBeamgunSettings beamgunSettings)
        {
            _viewModel = viewModel;
            _beamgunSettings = beamgunSettings;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.Reset();
            _viewModel.DisableUntil(DateTime.Now.AddMinutes(_beamgunSettings.DisableTime));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        
        private readonly IViewModel _viewModel;
        private readonly IBeamgunSettings _beamgunSettings;
    }
}
