using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using BeamgunApp.ViewModel;
using MessageBox = System.Windows.MessageBox;

namespace BeamgunApp
{
    public partial class MainWindow : Window
    {
        private Mutex _appMutex;
        public MainWindow()
        {
            CheckForDoubleRun();
            InitializeComponent();
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;
        }

        private void CheckForDoubleRun()
        {
            _appMutex  = new Mutex(true, "beamgun.lospi.net");
            if (!_appMutex.WaitOne(0))
            {
                MessageBox.Show(this, "Beamgun is already running.", "Beamgun", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                System.Windows.Application.Current.Shutdown();
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var viewModel = new BeamgunViewModel();
            viewModel.StealFocus += () => Dispatcher.Invoke(new MethodInvoker(delegate
            {
                Topmost = true;
                Activate();
            }));
            DataContext = viewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            _appMutex.ReleaseMutex();
        }
    }
}
