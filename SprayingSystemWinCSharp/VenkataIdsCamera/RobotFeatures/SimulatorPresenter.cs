using RCAPINet;
using System.Windows;

namespace SprayingSystem.RobotFeatures
{
    public class SimulatorPresenter
    {
        private Spel _spel;

        public SimulatorPresenter(Spel spel)
        {
            _spel = spel;
        }

        public void Initialize(Spel spel)
        {
            _spel = spel;
        }

        public void Show()
        {
            try
            {
                _spel.ShowWindow(RCAPINet.SpelWindows.Simulator);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

