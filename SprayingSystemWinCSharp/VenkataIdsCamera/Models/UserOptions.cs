using SprayingSystem.RpiDriver;
using System.Windows;

namespace SprayingSystem.Models
{
    public class UserOptions
    {
        public SprayingSystemConfig Options;

        public void LoadSprayingSystemConfiguration()
        {
            Options = SprayingSystemConfig.LoadSettings();
            if (Options == null)
            {
                MessageBox.Show("SprayingSystemConfig.json file not found. It is required for user options.");
                Application.Current.Shutdown();
            }

            SetUserOptionsCamera();
            SetUserOptionsRpi();
            SetUserOptionsRobot();
        }

        public void ReloadSprayingSystemConfiguration()
        {
            var tempOptions = SprayingSystemConfig.LoadSettings();
            if (tempOptions == null)
            {
                MessageBox.Show("SprayingSystemConfig.json file not found. It is required for user options.");
                Application.Current.Shutdown();
            }

            tempOptions.CopyTo(Options);
        }

        private void SetUserOptionsCamera()
        {
            // TODO: User Options - save / load from file? _tempImageFolder = Options.camera.temp_folder;
        }

        private void SetUserOptionsRpi()
        {
            RpiEndPoint.IpAndPort =
                $"http://{Options.rpi.ip_address}:{Options.rpi.port}/";

            RpiEndPoint.MessageTimeoutSecs = Options.rpi.message_timeout_sec;
        }

        private void SetUserOptionsRobot()
        {
            // TODO: user options - _robotConfig.RobotConfigurationNumber = Options.robot.id;
            // TODO: user options _robotConfig.RobotProject = Options.robot.project;

            // TODO: user options - What do we do with robot point number definitions? Fixed for now.
        }
    }
}
