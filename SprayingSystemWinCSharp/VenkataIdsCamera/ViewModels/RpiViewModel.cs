using System;
using System.Diagnostics;
using System.IO;
using System.IO.Enumeration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SprayingSystem.RpiModule;
using SprayingSystem.Utility;
using SprayingSystem.RpiDriver;
using SprayingSystem.Models;

namespace SprayingSystem.ViewModels
{
    public class RpiViewModel: ViewModelBase
    {
        #region Fields

        private IRpiController _rpiController;
        private bool _isConnected;
        private ICommand _connectCmd;
        private ICommand _sprayCmd;
        private ICommand _powerDownCmd;
        private ICommand _cleanSprayerCmd;

        private ICommand _blotSolenoidCmd;
        private ICommand _blotSolenoidForwardCmd;
        private ICommand _blotSolenoidReverseCmd;

        private ICommand _startVideoRecordingSprayCmd;
        private ICommand _getVideoFileCmd;
        private ICommand _playVideoCmd;

        private UserOptions _userOptions;
        private RobotVariablesModel _varsModel;
        private Action<string> Logger;
        private MediaElement _rpiVideoPlayer;

        private string _videoFileName;
        private bool _videoPlaying;

        #endregion

        public RpiViewModel(UserOptions userOptions, RobotVariablesModel varsModel,
            Action<string> logger)
        {
            _userOptions = userOptions;
            _varsModel = varsModel;

            Logger = logger;

            RpiEndPoint.IpAndPort =
                $"http://{_userOptions.Options.rpi.ip_address}:{_userOptions.Options.rpi.port}/";

            RpiEndPoint.MessageTimeoutSecs = _userOptions.Options.rpi.message_timeout_sec;


            _connectCmd = new RelayCommand(Connect, CanConnect);
            _sprayCmd = new RelayCommand(Spray, CanSpray);
            _powerDownCmd = new RelayCommand(PowerDown, CanPowerDown);
            _cleanSprayerCmd = new RelayCommand(CleanSprayer, CanSendCommand);

            _blotSolenoidCmd = new RelayCommand(BlotSolenoid, CanSendCommand);
            _blotSolenoidForwardCmd = new RelayCommand(BlotSolenoidForward, CanSendCommand);
            _blotSolenoidReverseCmd = new RelayCommand(BlotSolenoidReverse, CanSendCommand);

            _startVideoRecordingSprayCmd = new RelayCommand(StartRecordingSpray, CanStartRecordingSpray);
            _getVideoFileCmd = new RelayCommand(GetVideoFile, CanGetVideoFile);
            _playVideoCmd = new RelayCommand(PlayVideo, CanPlayVideo);
        }

        public MediaElement RpiVideoPlayer
        {
            set { _rpiVideoPlayer = value; }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConnectCmd
        {
            get { return _connectCmd; }
            set
            {
                _connectCmd = value;
                OnPropertyChanged();
            }
        }

        public ICommand SprayCmd
        {
            get { return _sprayCmd; }
            set
            {
                _sprayCmd = value;
                OnPropertyChanged();
            }
        }

        public ICommand PowerDownCmd
        {
            get { return _powerDownCmd; }
            set
            {
                _powerDownCmd = value;
                OnPropertyChanged();
            }
        }

        public ICommand CleanSprayerCmd
        {
            get { return _cleanSprayerCmd; }
            set
            {
                _cleanSprayerCmd = value;
                OnPropertyChanged();
            }
        }

        public ICommand BlotSolenoidCmd
        {
            get { return _blotSolenoidCmd; }
            set
            {
                _blotSolenoidCmd = value;
                OnPropertyChanged();
            }
        }

        public ICommand BlotSolenoidForwardCmd
        {
            get { return _blotSolenoidForwardCmd; }
            set
            {
                _blotSolenoidForwardCmd = value;
                OnPropertyChanged();
            }
        }

        public ICommand BlotSolenoidReverseCmd
        {
            get { return _blotSolenoidReverseCmd; }
            set
            {
                _blotSolenoidReverseCmd = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The SprayPreparationDelay is a time delay that is inherent in the RaspBerry Pi
        /// and the Python code which is the result of getting the spraying controller
        /// ready to fire the spray-output.
        /// </summary>
        public int SprayPreparationDelay
        {
            get { return _userOptions.Options.biojet_process.prep_delay; }
        }

        public string VideoFolder
        {
            get { return _userOptions.Options.rpi.video_folder; }
        }

        public int VideoStartUpDelayMillisec
        {
            get { return _userOptions.Options.rpi.video_startup_delay_msec; }
        }

        public int VideoSavingDelayMillisec
        {
            get { return _userOptions.Options.rpi.video_saving_delay_msec; }
        }

        public string VideoFileName
        {
            get { return _videoFileName; }
            set
            {
                _videoFileName = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartVideoRecordingSprayCmd
        {
            get { return _startVideoRecordingSprayCmd; }
            set
            {
                _startVideoRecordingSprayCmd = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetVideoFileCmd
        {
            get { return _getVideoFileCmd; }
            set
            {
                _getVideoFileCmd = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlayVideoCmd
        {
            get { return _playVideoCmd; }
            set
            {
                _playVideoCmd = value;
                OnPropertyChanged();
            }
        }

        #region Private

        private bool CanConnect(object obj)
        {
            return !IsConnected;
        }

        private async void Connect(object sender)
        {
            Logger("RPI status: connecting");

            _rpiController = new RpiController();
            _rpiController.Connect();
            try
            {
                var cmd = new DateTimeCommand();
                await _rpiController.SendMessage(cmd);
                IsConnected = true;
                Logger("RPI status: connected");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                _rpiController = null;
            }
        }

        private bool CanSpray(object obj)
        {
            return IsConnected;
        }

        private async void Spray(object sender)
        {
            if (!IsConnected)
                return;

            Logger("RPI Spraying");

            try
            {
                // TODO: Why I ddi not use this? Because this is POST?
                var cmd = new SprayCommand();

                var sprayTime = int.Parse(_varsModel.SprayTime);
                await _rpiController.SendSetSpray(sprayTime);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private bool CanSendCommand(object obj)
        {
            return IsConnected;
        }

        private async void CleanSprayer(object sender)
        {
            if (!IsConnected)
                return;

            Logger("RPI Clean Process");
            Logger($"CleanTime(msec)={_varsModel.CleanTime},CleanCycles={_varsModel.CleanCycles}");

            try
            {
                var cleantime = int.Parse(_varsModel.CleanTime);
                var cleancycles = int.Parse(_varsModel.CleanCycles);
                await _rpiController.SendSetCleanSpray(cleantime, cleancycles);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool CanPowerDown(object obj)
        {
            return true;
        }

        private void PowerDown(object obj)
        {

        }

        private async void BlotSolenoid(object obj)
        {
            if (!IsConnected)
                return;

            Logger("RPI Blot Solenoid");

            try
            {
                var rdelay = int.Parse(_varsModel.BlotTime);
                await _rpiController.SendBlotSolenoid("legacy",rdelay);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void BlotSolenoidForward(object obj)
        {
            if (!IsConnected)
                return;

            Logger("RPI Blot Solenoid Forward");

            try
            {
                //var rdelay = int.Parse(_varsModel.BlotTime);
                await _rpiController.SendBlotSolenoid("forward", 0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void BlotSolenoidReverse(object obj)
        {
            if (!IsConnected)
                return;

            Logger("RPI Blot Solenoid Reverse");

            try
            {
                //var rdelay = int.Parse(_varsModel.BlotTime);
                await _rpiController.SendBlotSolenoid("reverse", 0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool CanStartRecordingSpray(object obj)
        {
            return IsConnected;
        }

        private async void StartRecordingSpray(object obj)
        {
            if (!IsConnected)
                return;

            Logger("RPI Start Recording");

            try
            {
                // TODO: Get the parameter from the view-model.
                //var rdelay = int.Parse(_varsModel.BlotTime);
                var durationSecs = 0.0;

                await _rpiController.SendStartRecording(durationSecs);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool CanGetVideoFile(object obj)
        {
            return IsConnected;
        }

        private async void GetVideoFile(object obj)
        {
            if (!IsConnected)
                return;

            Logger("RPI Get Video File");

            StopVideoPlayer();
            try
            {
                ResetVideoPlayer();

                Directory.CreateDirectory(VideoFolder);
                var fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".mp4";
                VideoFileName = Path.Combine(VideoFolder, fileName);

                await _rpiController.SendGetVideoFile(VideoFileName);

                Logger( "Video File Saved as: " + VideoFileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool CanPlayVideo(object obj)
        {
            return (_rpiVideoPlayer != null);
        }

        private async void PlayVideo(object obj)
        {
            if (VideoFileName == null)
            {
                Logger("Play RPI Video: Nothing to play");
                return;
            }

            Logger("Play RPI Video: " + VideoFileName);

            try
            {
                StartVideoPlayer();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void RpiLedOnCommand_OnClick(object sender)
        {
            if (!IsConnected)
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

        private async void RpiLedOffCommand_OnClick(object sender)
        {
            if (!IsConnected)
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

        private void StopVideoPlayer()
        {
            if(_videoPlaying)
                _rpiVideoPlayer.Stop();
            _videoPlaying = false;
        }

        private void StartVideoPlayer()
        {
            StopVideoPlayer();

            _rpiVideoPlayer.Source = new Uri(VideoFileName);
            _rpiVideoPlayer.Play();
            _videoPlaying = true;
        }

        private void ResetVideoPlayer()
        {
            if (VideoFileName != null)
                _rpiVideoPlayer.Clock = null;
        }
        #endregion
    }
}
