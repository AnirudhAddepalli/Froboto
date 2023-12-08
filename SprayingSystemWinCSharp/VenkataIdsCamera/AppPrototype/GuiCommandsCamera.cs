using System;
using System.Windows;
using SprayingSystem.CameraModule;

namespace SprayingSystem
{
    public partial class MainWindow 
    {
        #region Fields

        private ICameraController _camera;
        private bool _saveNextImageRequested;
        private bool _takePictureRequested;

        private bool _liveMode;
        private bool LiveMode
        {
            get { return _liveMode;}
            set
            {
                _liveMode = value;
                DisplayStatus.Dispatcher.BeginInvoke(new Action<MainWindow>
                        (delegate
                        {
                            DisplayStatus.Text = _liveMode ? "Live" : "Not Live";
                        }), new object[] { this });
            }
        }

        #endregion

        /// <summary>
        /// Connect Command.
        /// If not connected to camera, it will connect.
        /// Then, the camera wills tart displaying Live images.
        /// </summary>
        private void CameraConnectCommand_OnClick(object sender, RoutedEventArgs e)
        {
            if (_camera == null)
                InitializeCameraController();
        }

        /// <summary>
        /// Live Command
        /// We start displaying live camera.
        /// The live mode stops when the user takes a picture.
        /// </summary>
        private void CameraLiveCommand_OnClick(object sender, RoutedEventArgs e)
        {
            LiveMode = true;
        }

        /// <summary>
        /// Take a picture and display it.
        /// If the user takes a picture then the picture is displayed.
        /// We stop displaying live images.
        /// </summary>
        private void CameraTakePictureCommand_OnClick(object sender, RoutedEventArgs e)
        {
            StartLiveMode();
            _takePictureRequested = true;
        }

        /// <summary>
        /// Save Image Command
        /// Saves a frame of a live image to temporary file.
        /// The camera needs to be in display live mode.
        /// We should be able to save an image being displayed too.
        /// </summary>
        private void CameraSaveImageCommand_OnClick(object sender, RoutedEventArgs e)
        {
            if (LiveMode)
                _saveNextImageRequested = true;
            else
                SaveImageBeingDisplayed();
        }

        private void StartLiveMode()
        {
            if (!LiveMode)
                LiveMode = true;
        }
    }
}
