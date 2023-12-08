using RCAPINet;
using System.Windows;

namespace SprayingSystem.RobotFeatures
{
    public class IoMonitorPresenter
    {
        private Spel _spel;

        public IoMonitorPresenter(Spel spel)
        {
            _spel = spel;
        }

        public void Initialize(Spel spel)
        {
            _spel = spel;
        }

        public bool CanShow(object obj)
        {
            return _spel != null;
        }

        public void Show(object obj)
        {
            try
            {
                _spel.ShowWindow(SpelWindows.IOMonitor);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
