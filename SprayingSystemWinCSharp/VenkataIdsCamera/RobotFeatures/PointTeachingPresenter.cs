using RCAPINet;
using System.Windows;

namespace SprayingSystem.RobotFeatures
{
    public class PointTeachingPresenter
    {
        private Spel _spel;

        public PointTeachingPresenter(Spel spel)
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
                _spel.TeachPoint("robot1.pts", 1, "Teach Spray Position");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
