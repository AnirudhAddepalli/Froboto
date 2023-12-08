using System;
using System.Windows.Input;
using SprayingSystem.Models;
using SprayingSystem.Utility;

namespace SprayingSystem.ViewModels
{
    public class GridViewModel : ViewModelBase
    {
        #region Fields

        private AppViewModel _appViewModel;

        private ICommand _storeGridCmd;

        private UserOptions _userOptions;
        private GridInfo _gridInfo;
        private Action<string> Logger;

        private int MaxGridPositions = 8;

        #endregion

        public GridViewModel(UserOptions userOptions, GridInfo gridInfo, Action<string> logger,
            AppViewModel appViewModel)
        {
            _appViewModel = appViewModel;
            _userOptions = userOptions;
            _gridInfo = gridInfo;
            Logger = logger;

            StoreGridCmd = new RelayCommand(StoreGrid, CanStoreGrid);
        }

        public ICommand StoreGridCmd
        {
            get { return _storeGridCmd; }
            set
            {
                _storeGridCmd = value;
                OnPropertyChanged();
            }
        }

        public string GridBoxName
        {
            get { return _gridInfo.BoxName; }
            set
            {
                _gridInfo.BoxName = value;
                OnPropertyChanged();
            }
        }

        public string SampleName
        {
            get { return _gridInfo.SampleName; }
            set
            {
                _gridInfo.SampleName = value;
                OnPropertyChanged();
            }
        }

        public string GridPosition
        {
            get { return _gridInfo.Position; }
            set
            {
                _gridInfo.Position = value;
                OnPropertyChanged();
            }
        }

        public void WriteToLog()
        {
            Logger("\nGrid Info:");
            Logger(string.Format($"Grid Box Name : {GridBoxName}"));
            Logger(string.Format($"Sample Name   : {SampleName}"));
            Logger(string.Format($"Grid Position : {GridPosition}"));
        }

        #region Private

        private bool CanStoreGrid(object obj)
        {
            return IsValidPosition() && _appViewModel.RobotViewModel.IsConnected;
        }

        private async void StoreGrid(object obj)
        {
            Logger("Storing Grid at position " + _gridInfo.Position);

            if (IsValidPosition())
            {
                var position = int.Parse(GridPosition);
                _appViewModel.RobotViewModel.MoveToGridBoxPosition(position);
                _appViewModel.GridViewModel.WriteToLog();
                return;
            }

            Logger("Invalid characters or range in Grid Position value.");
        }

        private bool IsValidPosition()
        {
            int value;
            if (int.TryParse(GridPosition, out value))
            {
                if (value >= 1 && value <= MaxGridPositions)
                    return true;
            }

            return false;
        }

        #endregion
    }
}
