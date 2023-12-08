using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using SprayingSystem.RpiModule;

namespace SprayingSystem
{
    public partial class MainWindow
    {
        #region Fields

        private IRpiController _rpiController;

        #endregion

        private bool _rpiConnected;
        public bool RpiConnected
        {
            get { return _rpiConnected;}
            set
            {
                _rpiConnected = value;
                RpiStatus.Text = "Connected";
            }
        }

        private async void RpiConnectCommand_OnClick(object sender, RoutedEventArgs e)
        {
            _rpiController = new RpiController();
            _rpiController.Connect();
            try
            {
                var cmd = new DateTimeCommand();
                await _rpiController.SendMessage(cmd);
                RpiConnected = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                _rpiController = null;
            }
        }

        private async void RpiSprayCommand_OnClick(object sender, RoutedEventArgs e)
        {
            if (!RpiConnected)
                return;

            try
            {
                var cmd = new SprayCommand();
                await _rpiController.SendMessage(cmd);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private async void RpiLedOnCommand_OnClick(object sender, RoutedEventArgs e)
        {
            if (!RpiConnected)
                return;

            try
            {
                var cmd = new LedCommand() { On = true };
                await _rpiController.SendMessage(cmd);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private async void RpiLedOffCommand_OnClick(object sender, RoutedEventArgs e)
        {
            if (!RpiConnected)
                return;

            try
            {
                var cmd = new LedCommand() { On = false };
                await _rpiController.SendMessage(cmd);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}
