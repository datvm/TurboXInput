using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TurboXInput.App.Models.ViewModels;
using TurboXInput.Core;

namespace TurboXInput.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MillisecondsDelay = 100;
        private CancellationTokenSource cts;

        private IXInputService xInputService;

        public MainWindow()
        {
            InitializeComponent();

            this.xInputService = new XInputService();
        }

        public void Refresh()
        {
            var xboxControllers = this.xInputService.GetControllers();

            this.cboXBoxController.Items.Clear();

            foreach (var xboxController in xboxControllers)
            {
                this.cboXBoxController.Items.Add(new XBoxControllerViewModel(
                    xboxController,
                    "XBox Controller #" + xboxController));
            }

            if (cboXBoxController.Items.Count > 0)
            {
                cboXBoxController.SelectedIndex = 0;
            }
        }

        private async Task PeriodicCheck()
        {
            { // XBox
                XBoxControllerViewModel xboxController = null;
                await this.Dispatcher.InvokeAsync(() =>
                {
                    xboxController = this.cboXBoxController.SelectedItem as XBoxControllerViewModel;
                });

                if (xboxController != null)
                {
                    var state = this.xInputService.GetControllerState(xboxController.Id);
                    Debug.WriteLine(state);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Refresh();

            this.cts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (!this.cts.IsCancellationRequested)
                {
                    try
                    {
                        await PeriodicCheck();
                        await Task.Delay(MillisecondsDelay);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    
                }
            });
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            this.Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.cts.Cancel();
        }
    }
}
