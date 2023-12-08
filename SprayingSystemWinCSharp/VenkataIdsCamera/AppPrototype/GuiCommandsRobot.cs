using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SprayingSystem.RobotDriver;

namespace SprayingSystem
{
    public partial class MainWindow
    {
        // Status Ribbon:
        // RobotStatus, RobotLastCmd

        private IRobotDriver _robotDriver;
        private EpsonRc7DriverConfig _robotConfig;

        private void InitializeLogArea()
        {
            richTextBoxLog.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

        private void RobotConnectCommand_OnClick(object sender, RoutedEventArgs e)
        {
            richTextBoxLog.AppendText("Connecting...\n");
            richTextBoxLog.ScrollToEnd();

            Task.Factory.StartNew(() =>
            {
                RobotConnect();
            });
        }

        private void RobotGoHomeCommand_OnClick(object sender, RoutedEventArgs e)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver == null)
                    return;

                if (_robotDriver.GoHome())
                    RobotLastCmd.Text = "Go Home";
                else
                    RobotLastCmd.Text = "Failed";
            }
            catch (Exception exception)
            {
                richTextBoxLog.AppendText($"Command filed: {exception}\n");
            }
            finally
            {
                stopWatch.Stop();
                richTextBoxLog.AppendText($"Time to Go Home (msec): {stopWatch.ElapsedMilliseconds}\n");
                richTextBoxLog.ScrollToEnd();
            }
        }

        private void RobotGoStandbyCommand_OnClick(object sender, RoutedEventArgs e)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver == null)
                    return;

                if (_robotDriver.GoStandby())
                    RobotLastCmd.Text = "Go Standby";
                else
                    RobotLastCmd.Text = "Failed";
            }
            catch (Exception exception)
            {
                richTextBoxLog.AppendText($"Command filed: {exception}\n");
            }
            finally
            {
                stopWatch.Stop();
                richTextBoxLog.AppendText($"Time to Go Standby (msec): {stopWatch.ElapsedMilliseconds}\n");
                richTextBoxLog.ScrollToEnd();
            }
        }

        private void RobotGoBeforeSprayCommand_OnClick(object sender, RoutedEventArgs e)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver == null)
                    return;

                if (_robotDriver.GoBeforeSpray())
                    RobotLastCmd.Text = "Go BeforeSpray";
                else
                    RobotLastCmd.Text = "Failed";
            }
            catch (Exception exception)
            {
                richTextBoxLog.AppendText($"Command filed: {exception}\n");
            }
            finally
            {
                stopWatch.Stop();
                richTextBoxLog.AppendText($"Time to Go Before Spray (msec): {stopWatch.ElapsedMilliseconds}\n");
                richTextBoxLog.ScrollToEnd();
            }
        }

        private void RobotGoAfterSprayCommand_OnClick(object sender, RoutedEventArgs e)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver == null)
                    return;

                if (_robotDriver.GoAfterSpray())
                    RobotLastCmd.Text = "Go AfterSpray";
                else
                    RobotLastCmd.Text = "Failed";
            }
            catch (Exception exception)
            {
                richTextBoxLog.AppendText($"Command filed: {exception}\n");
            }
            finally
            {
                stopWatch.Stop();
                richTextBoxLog.AppendText($"Time to Go After Spray (msec): {stopWatch.ElapsedMilliseconds}\n");
                richTextBoxLog.ScrollToEnd();
            }
        }

        private void RobotMotorsOnOffHandler(object sender, RoutedEventArgs e)
        {
            if (_robotDriver == null)
                return;

            var checkBox = sender as CheckBox;

            if ((bool)checkBox.IsChecked)
                _robotDriver.TurnMotorsOn();
            else
                _robotDriver.TurnMotorsOff();
        }

        private void MotionTypeCheckedHandler(object sender, RoutedEventArgs e)
        {
            if (_robotDriver == null)
                return;

            if ((bool)JumpMotionButton.IsChecked)
            {
                _robotDriver.SetMotionType(MotionType.JUMP);
            }
            else if ((bool)MoveTypeButton.IsChecked)
            {
                _robotDriver.SetMotionType(MotionType.MOVE);
            }
            //else if ((bool)GotTypeButton.IsChecked)
            //{
            //    _robotDriver.SetMotionType(MotionType.Go);
            //}
        }

        private void SpeedChangedHandler(object sender, TextChangedEventArgs args)
        {
            if (_robotDriver == null)
                return;

            var textBox = sender as TextBox;
            var speed = int.Parse(textBox.Text);

            if (speed < 0 || speed > 100)
                return;

            _robotDriver.SetDefaultSpeed(speed);
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // https://dotnetpattern.com/wpf-dispatcher
        // I want this method to run in a non-UI thread.
        // I want the UI to be set free to update the screen.
        private void RobotConnect()
        {
            Thread.Sleep(10);
            Dispatcher.BeginInvoke((Action)(() =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                try
                {
                    if (_robotDriver == null)
                        _robotDriver = new EpsonRc7Driver(_robotConfig);

                    if (_robotDriver.Start())
                    {
                        // TODO: Don't turn motors on just yet.
                        // _robotDriver.TurnMotorsOn();

                        RobotStatus.Text = "Connected";

                        if (_userOption.robot.simulation)
                        {
                            EpsonRc7Driver epson = _robotDriver as EpsonRc7Driver;
                            epson.OpenSimulatorWindow();
                        }
                    }
                    else
                    {
                        RobotStatus.Text = "Disconnected";
                    }
                }
                catch (Exception exception)
                {
                    richTextBoxLog.AppendText($"Command filed: {exception}\n");
                }
                finally
                {
                    stopWatch.Stop();
                    richTextBoxLog.AppendText($"Time to Connect (msec): {stopWatch.ElapsedMilliseconds}\n");
                    richTextBoxLog.ScrollToEnd();
                }
            }));
        }
    }
}
