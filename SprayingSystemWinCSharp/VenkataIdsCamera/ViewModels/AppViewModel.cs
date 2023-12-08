using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SprayingSystem.Models;
using SprayingSystem.SprayingProcess;
using SprayingSystem.SprayingSystemConfigViewer;
using SprayingSystem.Utility;

namespace SprayingSystem.ViewModels
{
    public class AppViewModel : ViewModelBase
    {
        #region Fields

        private UserOptions _userOptions;
        private RobotVariablesModel _varsModel;
        private GridInfo _gridInfo;

        private RobotViewModel _robotViewModel;
        private CameraViewModel _cameraViewModel;
        private RpiViewModel _rpiViewModel;
        private ProcessOptionsViewModel _processOptionsViewModel;
        private GridViewModel _gridViewModel;

        private ICommand _powerDownCmd;
        private ICommand _sprayAndPlungeCmd;
        private ICommand _editConfigSettingsCmd;

        private MediaElement _rpiVideoPlayer;

        #endregion

        public AppViewModel(Action<string> logger, MainWindow2 wpfWindow)
        {
            if (logger == null)
            {
                MessageBox.Show("Invalid system configuration. Missing logger.");
                Application.Current.Shutdown();
            }
            Logger = logger;

            _varsModel = new RobotVariablesModel();
            _userOptions = new UserOptions();
            _gridInfo = new GridInfo(){Position="1"};

            _userOptions.LoadSprayingSystemConfiguration();

            RobotViewModel = new RobotViewModel(_userOptions, _varsModel, logger, wpfWindow);
            CameraViewModel = new CameraViewModel(_userOptions, logger);
            RpiViewModel = new RpiViewModel(_userOptions, _varsModel, logger);
            ProcessOptionsViewModel = new ProcessOptionsViewModel(_userOptions, logger);
            GridViewModel = new GridViewModel(_userOptions, _gridInfo, logger, this);

            _powerDownCmd = new RelayCommand(PowerDown, CanPowerDown);
            _sprayAndPlungeCmd = new RelayCommand(SprayAndPlunge, RobotViewModel.CanMove);
            _editConfigSettingsCmd = new RelayCommand(EditConfigSettings, obj => true);

            RpiViewModel.RpiVideoPlayer = wpfWindow.RpiVideoPlayer;
        }

        public Action<string> Logger;

        public RobotViewModel RobotViewModel
        {
            get { return _robotViewModel; }
            set
            {
                _robotViewModel = value;
                OnPropertyChanged();
            }
        }

        public RobotVariablesViewModel RobotVariablesViewModel
        {
            get { return _robotViewModel.RobotVariablesViewModel; }
        }

        public CameraViewModel CameraViewModel
        {
            get { return _cameraViewModel; }
            set
            {
                _cameraViewModel = value;
                OnPropertyChanged();
            }
        }

        public RpiViewModel RpiViewModel
        {
            get { return _rpiViewModel; }
            set
            {
                _rpiViewModel = value;
                OnPropertyChanged();
            }
        }

        public ProcessOptionsViewModel ProcessOptionsViewModel
        {
            get { return _processOptionsViewModel; }
            set
            {
                _processOptionsViewModel = value;
                OnPropertyChanged();
            }
        }

        public GridViewModel GridViewModel
        {
            get { return _gridViewModel; }
            set
            {
                _gridViewModel = value;
                OnPropertyChanged();
            }
        }

        public GridInfo GridModel
        {
            get { return _gridInfo; }
        }

        public ICommand PowerDownCmd => _powerDownCmd;

        public ICommand SprayAndPlungeCmd => _sprayAndPlungeCmd;

        public ICommand EditConfigSettingsCmd => _editConfigSettingsCmd;

        #region Private

        private bool CanPowerDown(object obj)
        {
            return true;
        }

        private void PowerDown(object obj)
        {
            try
            {
                RobotViewModel.PowerDownCmd.Execute(obj);
                CameraViewModel.PowerDownCmd.Execute(obj);
                RpiViewModel.PowerDownCmd.Execute(obj);
            }
            catch
            {
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }

        private void SprayAndPlunge(object obj)
        {
            SprayAndPlungeProcess.Process(
                ProcessOptionsViewModel, 
                RobotVariablesViewModel,
                _varsModel,
                RobotViewModel, 
                CameraViewModel, 
                RpiViewModel,
                Logger);
        }

        private void EditConfigSettings(object obj)
        {
            var _window = new JsonTreeView(SprayingSystemConfig.FileName);
            _window.ShowDialog();

            if (_window.Edits.FileSaved)
            {
                // The JsonTreeView should let us know if we have to "Reload" or "Reboot"?

                if (_window.Edits.CameraChanged
                    || _window.Edits.RpiChanged
                    || _window.Edits.RobotEssentialsChanged)
                {
                    // We should reboot - if - Camera or RPI or Robot IDs are changed.
                    MessageBox.Show(
                        "Please restart the app for your changes to take effect",
                        "Config Changes Requires Restart");
                    Application.Current.Shutdown();
                }
                else
                {
                    // _window.Edits.RobotMotionsChanged

                    // We should reload - if - robot locations speed / motion types are changed.
                    ReLoadAndApplySprayingSystemConfigSettings();
                }
            }
        }

        private void ReLoadAndApplySprayingSystemConfigSettings()
        {
            _userOptions.ReloadSprayingSystemConfiguration();

            RobotViewModel.OnConfigSettingsUpdate();
            RobotVariablesViewModel.OnConfigSettingsUpdate();
            CameraViewModel.OnConfigSettingsUpdate();
            RpiViewModel.OnConfigSettingsUpdate();
            ProcessOptionsViewModel.OnConfigSettingsUpdate();
        }

        #endregion
    }

}
