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
using vJoyInterfaceWrap;

namespace TurboXInput.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MappingDelay = 1;
        private CancellationTokenSource cts;

        private IXInputService xInputService;
        private vJoy vjoy;

        private bool owningVJoy = false;
        private uint owningXBoxId, owningVJoyId;

        private bool previousTurboPressed = false;
        private bool isTurboPressed = false;
        private DateTime nextTurboStateChange;

        public MainWindow()
        {
            InitializeComponent();

            this.xInputService = new XInputService();
            this.vjoy = new vJoy();
        }

        private void SetControllerList(ComboBox comboBox, IEnumerable<uint> ids, string prefix)
        {
            comboBox.Items.Clear();

            foreach (var id in ids)
            {
                comboBox.Items.Add(new GameControllerViewModel(
                    id,
                    prefix + id));
            }

            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void Refresh()
        {
            this.SetControllerList(this.cboXBoxController, this.xInputService.GetControllers(), "XBox Controller #");
            this.SetControllerList(this.cboVJoyController, this.vjoy.GetFreeIds(), "VJoy Controller #");
        }

        private async Task PeriodicCheck()
        {
            { // XBox
                GameControllerViewModel xboxController = null;
                await this.Dispatcher.InvokeAsync(() =>
                {
                    xboxController = this.cboXBoxController.SelectedItem as GameControllerViewModel;
                });

                if (xboxController != null)
                {
                    try
                    {
                        var state = this.xInputService.GetControllerState(xboxController.Id);
                        await this.ShowXBoxState(state, null);
                    }
                    catch (Exception ex)
                    {
                        await this.ShowXBoxState(null, ex);
                    }

                }
            }

            { // VJoy
                GameControllerViewModel vJoyController = null;
                await this.Dispatcher.InvokeAsync(() =>
                {
                    vJoyController = this.cboVJoyController.SelectedItem as GameControllerViewModel;
                });

                if (vJoyController != null)
                {
                    await this.ShowVJoyState(vJoyController.Id);
                }
            }
        }

        const XInputGamepadButton turboButtonXBox = XInputGamepadButton.Back;
        const int TurboDelay = 10;
        private void PeriodicMap()
        {
            if (!this.owningVJoy)
            {
                return;
            }

            var vJoyId = this.owningVJoyId;
            var xboxId = this.owningXBoxId;

            var xboxState = this.xInputService.GetControllerState(xboxId);
            var xboxGamepad = xboxState.GamePad;
            var buttons = xboxGamepad.wButtons;

            //this.vjoy.SetAxis(xboxGamepad.sThumbLX, vJoyId, HID_USAGES.HID_USAGE_X);
            //this.vjoy.SetAxis(xboxGamepad.sThumbLY, vJoyId, HID_USAGES.HID_USAGE_Y);
            //this.vjoy.SetAxis(xboxGamepad.sThumbRX, vJoyId, HID_USAGES.HID_USAGE_RX);
            //this.vjoy.SetAxis(xboxGamepad.sThumbRY, vJoyId, HID_USAGES.HID_USAGE_RY);

            //this.vjoy.SetAxis(xboxGamepad.bLeftTrigger, vJoyId, HID_USAGES.HID_USAGE_SL0);
            //this.vjoy.SetAxis(xboxGamepad.bRightTrigger, vJoyId, HID_USAGES.HID_USAGE_SL1);


            //foreach (var button in XInputInvoker.AllButtons)
            //{
            //    this.vjoy.SetBtn((button & buttons) > 0, vJoyId, (uint)button);
            //}

            //if ((buttons & XInputGamepadButton.Back) > 0)
            //{
            //    this.vjoy.SetBtn(true, vJoyId, (uint)XInputGamepadButton.A);
            //}
            //else
            //{
            //    this.vjoy.SetBtn(false, vJoyId, (uint)XInputGamepadButton.A);
            //}

            // Turbo button
            if ((turboButtonXBox & buttons) > 0)
            {
                if (this.previousTurboPressed)
                {
                    if (DateTime.Now > this.nextTurboStateChange)
                    {
                        this.isTurboPressed = !this.isTurboPressed;
                        this.nextTurboStateChange = this.nextTurboStateChange.AddMilliseconds(TurboDelay);
                    }
                }
                else
                {
                    this.previousTurboPressed = true;
                    this.isTurboPressed = true;
                    this.nextTurboStateChange = DateTime.Now.AddMilliseconds(TurboDelay);
                }

                foreach (var button in XInputInvoker.AllButtons)
                {
                    uint targetButton = 0;

                    switch (button)
                    {
                        case XInputGamepadButton.Start:
                            targetButton = 8;
                            break;
                        case XInputGamepadButton.LeftThumb:
                            targetButton = 9;
                            break;
                        case XInputGamepadButton.RightThumb:
                            targetButton = 10;
                            break;
                        case XInputGamepadButton.LeftShoulder:
                            targetButton = 5;
                            break;
                        case XInputGamepadButton.RightShoulder:
                            targetButton = 6;
                            break;
                        case XInputGamepadButton.Guide:
                            break;
                        case XInputGamepadButton.A:
                            targetButton = 1;
                            break;
                        case XInputGamepadButton.B:
                            targetButton = 2;
                            break;
                        case XInputGamepadButton.X:
                            targetButton = 3;
                            break;
                        case XInputGamepadButton.Y:
                            targetButton = 4;
                            break;
                        default:
                            break;
                    }


                    if ((button & buttons) > 0)
                    {
                        this.vjoy.SetBtn(this.isTurboPressed, vJoyId, targetButton);
                    }
                    else
                    {
                        this.vjoy.SetBtn(false, vJoyId, targetButton);
                    }
                }
            }
            else
            {
                this.vjoy.ResetButtons(vJoyId);
            }
        }

        private async Task ShowVJoyState(uint id)
        {
            var result = new StringBuilder();

            try
            {
                // Check status
                var status = this.vjoy.GetVJDStatus(id);
                result.AppendLine(status.ToString());
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Append(ex.ToString());
            }

            await this.Dispatcher.InvokeAsync(() =>
            {
                this.lblVJoyStatus.Text = result.ToString();
            });
        }

        private async Task ShowXBoxState(XInputState? state, Exception ex)
        {
            if (ex != null)
            {
                await this.Dispatcher.InvokeAsync(() =>
                {
                    this.lblXBoxStatus.Text = ex.ToString();
                });

                return;
            }

            var result = new StringBuilder();
            var gamepad = state.Value.GamePad;

            result.AppendLine($"Left Trigger : ({gamepad.bLeftTrigger})");
            result.AppendLine($"Right Trigger: ({gamepad.bRightTrigger})");

            result.AppendLine($"Left Thumb   : ({gamepad.sThumbLX} ; {gamepad.sThumbLY})");
            result.AppendLine($"Right Thumb  : ({gamepad.sThumbRX} ; {gamepad.sThumbRY})");

            var buttons = gamepad.wButtons.GetIncludedButtons();
            result.AppendLine($"Buttons      : {string.Join(", ", buttons)}");

            await this.Dispatcher.InvokeAsync(() =>
            {
                this.lblXBoxStatus.Text = result.ToString();
            });
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
                        await Task.Delay(MappingDelay);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }

                }
            });

            Task.Run(() =>
            {
                while (!this.cts.IsCancellationRequested)
                {
                    try
                    {
                        PeriodicMap();
                        Thread.Sleep(MappingDelay);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            });
        }

        private void OnMapButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var vJoyController = this.cboVJoyController.SelectedItem as GameControllerViewModel;
                if (vJoyController == null)
                {
                    return;
                }

                var xboxController = this.cboXBoxController.SelectedItem as GameControllerViewModel;
                if (xboxController == null)
                {
                    return;
                }

                if (this.owningVJoy)
                {
                    this.vjoy.RelinquishVJD(vJoyController.Id);
                    this.btnMap.Content = "Map";
                    this.btnRefresh.IsEnabled = true;
                    this.cboVJoyController.IsEnabled = true;
                }
                else
                {
                    this.btnRefresh.IsEnabled = false;
                    this.cboVJoyController.IsEnabled = false;
                    this.btnMap.Content = "Unmap";

                    this.owningXBoxId = xboxController.Id;
                    this.owningVJoyId = vJoyController.Id;

                    this.vjoy.AcquireVJD(vJoyController.Id);

                    this.owningVJoy = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.btnMap.Content = "Map";
                this.btnRefresh.IsEnabled = true;
                this.cboVJoyController.IsEnabled = true;
            }
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
