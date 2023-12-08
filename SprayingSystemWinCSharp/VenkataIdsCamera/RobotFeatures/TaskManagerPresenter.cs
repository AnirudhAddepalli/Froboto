using RCAPINet;
using System.Windows;

namespace SprayingSystem.RobotFeatures
{
    public class TaskManagerPresenter
    {
        private Spel _spel;

        public TaskManagerPresenter(Spel spel)
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
                _spel.ShowWindow(SpelWindows.TaskManager);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
