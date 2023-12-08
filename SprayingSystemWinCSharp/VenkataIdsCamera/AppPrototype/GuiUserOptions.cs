using System.Windows;
using SprayingSystem.Models;
using SprayingSystem.RobotDriver;
using SprayingSystem.RpiDriver;

namespace SprayingSystem
{
    public partial class MainWindow
    {
        private SprayingSystemConfig _userOption;

        private void LoadSprayingSystemConfiguration()
        {
            _userOption = SprayingSystemConfig.LoadSettings();
            if (_userOption == null)
            {
                MessageBox.Show("SprayingSystemConfig.json file not found. It is required for user options.");
                Application.Current.Shutdown();
            }

            SetUserOptionsCamera();
            SetUserOptionsRpi();
            SetUserOptionsRobot();
        }

        private void SetUserOptionsCamera()
        {
            _tempImageFolder = _userOption.camera.temp_folder;
        }

        private void SetUserOptionsRpi()
        {
            RpiEndPoint.IpAndPort = 
                $"http://{_userOption.rpi.ip_address}:{_userOption.rpi.port}/";

            RpiEndPoint.MessageTimeoutSecs = _userOption.rpi.message_timeout_sec;
        }

        private void SetUserOptionsRobot()
        {
            var realUserOptions = new UserOptions();
            realUserOptions.LoadSprayingSystemConfiguration();

            _robotConfig = new EpsonRc7DriverConfig(realUserOptions);
            _robotConfig.RobotConfigurationNumber = _userOption.robot.id;
            _robotConfig.RobotProject = _userOption.robot.project;
        }
    }
}
