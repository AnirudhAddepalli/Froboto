using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using SprayingSystem.CameraModule;
using SprayingSystem.ImageProcess;
using SprayingSystem.Utility;

namespace SprayingSystem
{
    public partial class MainWindow : Window
    {
        #region Fields

        private String projectName = "NIEHS Spraying System by piTree Software";
        private String version = "v0.1.0";

        private string _tempImageFolder = @"c:\temp\Venkata_IDS\";

        #endregion

        public MainWindow()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CameraStatus.Text = "Disconnected";
            DisplayStatus.Text = "None";
            RpiStatus.Text = "Disconnected";
            RobotStatus.Text = "Disconnected";
            RobotLastCmd.Text = "None";

            LoadSprayingSystemConfiguration();
            DisplaySplashLikeImage();
            InitializeLogArea();
        }

        #region Camera event handlers

        private void ImageReceivedHandler(object sender, Bitmap bitmap)
        {
            try
            {
                var bitmapImage = FormatConverter.ToBitmapImage(bitmap);

                // It is important to display the image first.
                // Otherwise we may not be able to TakePicture correctly.
                DisplayImage(bitmapImage);

                SaveImageIfRequested(bitmapImage);
            }
            catch (Exception e)
            {
                Debug.WriteLine("--- [MainWindow] Exception: " + e.Message);
                MessageBoxHandler(this, "Exception", e.Message);
            }
        }

        private void CountersUpdatedHandler(object sender, uint frameCounter, uint errorCounter)
        {
            try
            {
                image.Dispatcher.BeginInvoke(new Action<MainWindow>
                    (delegate { counterTextBlock.Text = "Camera Frame Errors: " + errorCounter; }), new object[] { this });
            }
            catch (Exception e)
            {
                Debug.WriteLine("--- [MainWindow] Exception: " + e.Message);
                MessageBoxHandler(this, "Exception", e.Message);
            }
        }

        private void MessageBoxHandler(object sender, String messageTitle, String messageText)
        {
            MessageBox.Show(messageText, messageTitle);
        }

        #endregion

        #region GUI for camera controller

        private void InitializeCameraController()
        {
            Debug.WriteLine("--- [MainWindow] InitializeCameraController");
            try
            {
                _camera = new CameraController();
                _camera.Open();
                _camera.HookHandlers(ImageReceivedHandler, MessageBoxHandler, CountersUpdatedHandler);

                Closing += MainWindow_Closing;

                LiveMode = true;
                _saveNextImageRequested = false;

                CameraStatus.Text = "Connected";

                if (!_camera.Start())
                {
                    _camera = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("--- [MainWindow] Exception: " + e.Message);
                MessageBoxHandler(this, "Exception", e.Message);
            }
        }

        private void DisplayImage(BitmapImage bitmapImage)
        {
            if (!LiveMode)
                return;

            image.Dispatcher.BeginInvoke(
                new Action<MainWindow>(delegate { image.Source = bitmapImage; }),
                new object[] { this });
        }

        private void SaveImageBeingDisplayed()
        {
            var frame = (BitmapImage)(image.Source);
            if (frame != null)
                SaveImageWithUniqueName(_tempImageFolder, frame);
        }

        private void SaveImageIfRequested(BitmapImage bitmapImage)
        {
            if (_saveNextImageRequested)
            {
                SaveImageWithUniqueName(_tempImageFolder, bitmapImage);

                _saveNextImageRequested = false;
                LiveMode = false;
            }

            if (_takePictureRequested)
            {
                _takePictureRequested = false;
                LiveMode = false;
            }
        }

        private void SaveImageWithUniqueName(string folder, BitmapImage bitmapImage)
        {
            var fullname = FileUtil.CreateFilenameWithDateTime(
                folder, "Img_", "jpg");

            FileUtil.CreateFolderIfNotExist(fullname);

            _camera.SavePicture(fullname, bitmapImage);
        }

        #endregion

        private void DisplaySplashLikeImage()
        {
            try
            {
                var uri = new Uri("pack://application:,,,/splash.jpg");
                var bitmap = new BitmapImage(uri);
                image.Source = bitmap;
            }
            catch
            {
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Debug.WriteLine("--- [MainWindow] Closing");

            // If the camera was open / connected, then ensure that it is closed.
            // I don't think there is any issue not closing the camera on exit.
            _camera?.Close();

            _robotDriver.Stop();
        }

    }
}
