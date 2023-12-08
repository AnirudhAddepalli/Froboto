using RCAPINet;
using SprayingSystem.RobotDriver;
using SprayingSystem.RobotFeatures;
using SprayingSystem.Utility;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Interop;
using SprayingSystem.Models;
using ABI.System;

namespace SprayingSystem.ViewModels
{
    public class RobotViewModel : ViewModelBase
    {
        #region Fields

        private Spel _spel;
        private IRobotDriver _robotDriver;
        private EpsonRc7DriverConfig _robotConfig;
        private bool _isConnected;

        private UserOptions _userOptions;
        private Action<string> Logger;
        private RobotVariablesViewModel _robotVariablesViewModel;

        private Window _wpfWindow;

        #endregion

        public RobotViewModel(
            UserOptions userOptions, RobotVariablesModel varsModel,
            Action<string> logger, Window wpfWindow)
        {
            _userOptions = userOptions;
            Logger = logger;
            _wpfWindow = wpfWindow;

            _robotConfig = new EpsonRc7DriverConfig(userOptions);
            _robotVariablesViewModel = new RobotVariablesViewModel(null, varsModel, logger);

            _robotConfig.RobotConfigurationNumber = userOptions.Options.robot.id;
            _robotConfig.RobotProject = userOptions.Options.robot.project;

            RobotInitialize();

            _connectCmd = new RelayCommand(Connect, CanConnect);

            _tmPresenter = new TaskManagerPresenter(_spel);
            _tmPresenterCmd = new RelayCommand(_tmPresenter.Show, _tmPresenter.CanShow);

            _rmPresenter = new RobotManagerPresenter(_spel);
            _rmPresenterCmd = new RelayCommand(_rmPresenter.Show, _rmPresenter.CanShow);

            _iomPresenter = new IoMonitorPresenter(_spel);
            _iomPresenterCmd = new RelayCommand(_iomPresenter.Show, _iomPresenter.CanShow);

            _ptPresenter = new PointTeachingPresenter(_spel);
            _ptPresenterCmd = new RelayCommand(_ptPresenter.Show, _ptPresenter.CanShow);

            _ctPresenter = new ControllerToolsPresenter(_spel);
            _ctPresenterCmd = new RelayCommand(_ctPresenter.Show, _ctPresenter.CanShow);

            _resetCmd = new RelayCommand(Reset, CanReset);
            _powerDownCmd = new RelayCommand(PowerDown, CanPowerDown);
            _moveToLoadTweezersCmd = new RelayCommand(MoveToLoadTweezers, CanMove);
            _moveToSprayPositionCmd = new RelayCommand(MoveToSprayPosition, CanMove);
            _highPowerOnCmd = new RelayCommand(HighPowerOn, CanHighPowerOn);
        }

        public ICommand TaskManagerPresenterCmd => _tmPresenterCmd;

        public ICommand RobotManagerPresenterCmd => _rmPresenterCmd;

        public ICommand IoMonitorPresenterCmd => _iomPresenterCmd;

        public ICommand PointTeachingPresenterCmd => _ptPresenterCmd;

        public ICommand ControllerToolsPresenterCmd => _ctPresenterCmd;

        public ICommand ResetCmd => _resetCmd;
        public ICommand PowerDownCmd => _powerDownCmd;
        public ICommand ConnectCmd => _connectCmd;
        public ICommand MoveToLoadTweezersCmd => _moveToLoadTweezersCmd;
        public ICommand MoveToSprayPositionCmd => _moveToSprayPositionCmd;
        public ICommand HighPowerOnCmd => _highPowerOnCmd;

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        public bool CanMove(object obj)
        {
            return IsConnected;
        }

        public void MoveToPlungePosition(object obj)
        {
            if (!IsConnected)
                return;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver.GoAfterSpray())
                    Logger("Move to Plunge position");
                else
                    Logger("Move to Plunge position Failed.");
            }
            catch (System.Exception e)
            {
                Logger("Move to Plunge position - error: " + e.Message);
            }
            finally
            {
                stopWatch.Stop();
                Logger($"{stopWatch.ElapsedMilliseconds}(ms): Plunge");
            }
        }

        public void MoveToGridBoxPosition(object obj)
        {
            MoveToGridStorePosition(obj);
        }

        public void MoveToBackBlotPosition(object obj)
        {
            MoveToBackBlot(obj);
        }

        public void MoveToFrontBlotPosition(object obj)
        {
            MoveToFrontBlot(obj);
        }

        public void blotSolenoidCommand(object obj)
        {
            blotSolenoid(obj);
        }

        public void sprayCommand(object obj)
        {
            spray(obj);
        }

        public RobotVariablesViewModel RobotVariablesViewModel
        {
            get { return _robotVariablesViewModel;}
            set
            {
                _robotVariablesViewModel = value;
                OnPropertyChanged();
            }
        }

        #region Private

        private TaskManagerPresenter _tmPresenter;
        private ICommand _tmPresenterCmd;

        private RobotManagerPresenter _rmPresenter;
        private ICommand _rmPresenterCmd;

        private IoMonitorPresenter _iomPresenter;
        private ICommand _iomPresenterCmd;

        private PointTeachingPresenter _ptPresenter;
        private ICommand _ptPresenterCmd;

        private ControllerToolsPresenter _ctPresenter;
        private ICommand _ctPresenterCmd;

        private ICommand _connectCmd;
        private ICommand _resetCmd;
        private ICommand _powerDownCmd;
        private ICommand _moveToLoadTweezersCmd;
        private ICommand _moveToSprayPositionCmd;
        private ICommand _highPowerOnCmd;

        private void RobotInitialize()
        {
            if (_robotDriver == null)
                _robotDriver = new EpsonRc7Driver(_robotConfig);
            _spel = ((EpsonRc7Driver)_robotDriver).Spel;
        }

        private void RobotConnect()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            _robotConnectStatus = false;

            try
            {
                if (_robotDriver.Start())
                {
                    _spel = ((EpsonRc7Driver)_robotDriver).Spel;
                }
            }
            catch (System.Exception exception)
            {
                Logger("Robot status: Connection error - " + exception.Message);
            }
            finally
            {
                stopWatch.Stop();
            }
        }

        private void SetupWindowCommunication()
        {
            try
            {
                if (_userOptions.Options.robot.simulation)
                {
                    _spel = ((EpsonRc7Driver)_robotDriver).Spel;

                    // We set the parent so that the simulator opens on top.
                    SetParentWindow(_spel, _wpfWindow);

                    EpsonRc7Driver epson = _robotDriver as EpsonRc7Driver;
                    epson.OpenSimulatorWindow();
                }
                else
                {
                    SetParentWindow(_spel, _wpfWindow);
                }

                _robotConnectStatus = true;
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool CanConnect(object obj)
        {
            return true;
        }

        private bool _robotConnectStatus;

        private async void Connect(object obj)
        {
            Logger("Robot status: connecting");

            // RobotConnect will return us _robotConnectStatus
            await Task.Run(() => RobotConnect());

            SetupWindowCommunication();
            IsConnected = _robotConnectStatus;

            if (_spel != null)
            {
                Logger("Robot status: connected");

                _tmPresenter.Initialize(_spel);
                _rmPresenter.Initialize(_spel);
                _iomPresenter.Initialize(_spel);
                _ptPresenter.Initialize(_spel);
                _ctPresenter.Initialize(_spel);

                _robotVariablesViewModel.Initialize(_spel, _userOptions.Options.biojet_process);
            }
            else
            {
                Logger("Robot status: failed to connect");
            }
        }

        private bool CanReset(object obj)
        {
            return _spel != null;
        }

        private void Reset(object obj)
        {
            Logger("Robot Reset");


            try
            {
                _spel.Reset();
            }
            catch (System.Exception e)
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
            try
            {
                if (_spel != null)
                {
                    // Wait 2 seconds for anything else to stop.
                    Thread.Sleep(2000);
                    _spel.MotorsOn = false;
                    Thread.Sleep(2000);
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                _spel?.Dispose();
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void MoveToLoadTweezers(object obj)
        {
            if (!IsConnected)
            {
                Logger("Robot needs to be connected and powered on.");
                return;
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver.GoStandby())
                    Logger("Move to load tweezers");
                else
                    Logger("Move to load tweezers Failed.");
            }
            catch (System.Exception exception)
            {
                Logger("Move to load tweezers - error: " + exception.Message);
            }
            finally
            {
                stopWatch.Stop();
                Logger($"{stopWatch.ElapsedMilliseconds}(ms): Load Tweezers");
            }
        }

        private void MoveToSprayPosition(object obj)
        {
            if (!IsConnected)
            {
                Logger("Robot needs to be connected and powered on.");
                return;
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver.GoBeforeSpray())
                    Logger("Move to Spray position");
                else
                    Logger("Move to Spray position Failed.");
            }
            catch (System.Exception exception)
            {
                Logger("Move to Spray position - error: " + exception.Message);
            }
            finally
            {
                stopWatch.Stop();
                Logger($"{stopWatch.ElapsedMilliseconds}(ms): Spray");
            }
        }

        private void blotSolenoid(object obj)
        {
            if (_robotDriver.blotSolenoid())
                Logger("Blotting complete");
            else
                Logger("Blotting failed");
        }

        private void spray(object obj)
        {
            if (_robotDriver.spray())
                Logger("Spraying complete");
            else
                Logger("Spraying failed");
        }

        private void MoveToBackBlot(object obj)
        {
            var posName = "BackBlot";

            if (!IsConnected)
            {
                Logger("Robot needs to be connected and powered on.");
                return;
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver.GoBackBlot())
                    Logger($"Move to {posName} position");
                else
                    Logger($"Move to {posName} position Failed.");
            }
            catch (System.Exception exception)
            {
                Logger($"Move to {posName} position - error: " + exception.Message);
            }
            finally
            {
                stopWatch.Stop();
                Logger($"{stopWatch.ElapsedMilliseconds}(ms): {posName}");
            }
        }

        private void MoveToFrontBlot(object obj)
        {
            var posName = "FrontBlot";

            if (!IsConnected)
            {
                Logger("Robot needs to be connected and powered on.");
                return;
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver.GoFrontBlot())
                    Logger($"Move to {posName} position");
                else
                    Logger($"Move to {posName} position Failed.");
            }
            catch (System.Exception exception)
            {
                Logger($"Move to {posName} position - error: " + exception.Message);
            }
            finally
            {
                stopWatch.Stop();
                Logger($"{stopWatch.ElapsedMilliseconds}(ms): {posName}");
            }

        }

        private void MoveToGridStorePosition(object obj)
        {
            var position = (int)obj;

            if (!IsConnected)
            {
                Logger("Robot needs to be connected and powered on.");
                return;
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (_robotDriver.GoGridStorePosition(position))
                    Logger("Move to Spray position");
                else
                    Logger("Move to Spray position Failed.");
            }
            catch (System.Exception exception)
            {
                Logger("Move to Spray position - error: " + exception.Message);
            }
            finally
            {
                stopWatch.Stop();
                Logger($"{stopWatch.ElapsedMilliseconds}(ms): Grid Store Position");
            }

        }

        public void SetParentWindow(Spel spel, Window wpfWindow)
        {
            if (spel == null)
                return;

            var intPtr = new WindowInteropHelper(wpfWindow).Handle;
            spel.ParentWindowHandle = intPtr.ToInt32();
        }

        private bool CanHighPowerOn(object obj)
        {
            return IsConnected;
        }

        private bool _motorsOnResult;

        public async void HighPowerOn(object obj)
        {
            if (_spel != null && _robotDriver != null)
            {
                try
                {
                    Logger("Robot status: Powering up");

                    _motorsOnResult = false;
                    await Task.Run(() =>
                    {
                        try
                        {
                            _robotDriver.TurnMotorsOn();
                            _robotDriver.SetAccelDecelWithDefaults();
                            _motorsOnResult = true;
                        }
                        catch (System.Exception e)
                        {
                            Logger("Robot status: Error turning power on - " + e.Message);
                        }
                    });

                    if (_motorsOnResult)
                        Logger("Robot status: Power On, High Power");
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        #endregion
    }
}
