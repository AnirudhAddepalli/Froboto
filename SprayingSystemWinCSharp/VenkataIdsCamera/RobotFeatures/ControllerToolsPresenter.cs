using RCAPINet;
using System.Windows;

namespace SprayingSystem.RobotFeatures
{
    public class ControllerToolsPresenter
    {
        private Spel _spel;

        public ControllerToolsPresenter(Spel spel)
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
                _spel.RunDialog(SpelDialogs.ControllerTools);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
