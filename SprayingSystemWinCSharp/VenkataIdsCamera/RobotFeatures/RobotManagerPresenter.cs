using RCAPINet;
using System.Windows;

namespace SprayingSystem.RobotFeatures
{
    public class RobotManagerPresenter
    {
        private Spel _spel;

        public RobotManagerPresenter(Spel spel)
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
                _spel.RunDialog(SpelDialogs.RobotManager);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
