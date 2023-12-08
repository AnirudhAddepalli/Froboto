using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using SprayingSystem.ViewModels;

namespace SprayingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        private AppViewModel _viewModel;

        public MainWindow2()
        {
            InitializeComponent();

            _viewModel = new AppViewModel(RtbHandler,this);
            DataContext = _viewModel;

            InitializeLogArea();
        }

        public MediaElement RpiVideoPlayer
        {
            get { return rpiVideoPlayer; }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _viewModel.PowerDownCmd.Execute(null);
        }

        #region Log

        private void InitializeLogArea()
        {
            richTextBoxLog.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        }

        private void RtbHandler(string line)
        {
            richTextBoxLog.AppendText(line + "\n");
            richTextBoxLog.ScrollToEnd();
        }

        private void LogSaveCmd_Click(object sender, RoutedEventArgs e)
        {
            var fileName = GetLogFilename();
            if (string.IsNullOrEmpty(fileName))
                return;

            fileName = AddExtensionIfNotPresent(fileName);

            LogRobotVariables();
            LogGridModel();

            TextRange range;
            FileStream fStream;

            range = new TextRange(richTextBoxLog.Document.ContentStart, richTextBoxLog.Document.ContentEnd);
            fStream = new FileStream(@fileName, FileMode.Create);
            range.Save(fStream, DataFormats.Text);

            fStream.Close();
        }

        private string GetLogFilename()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                return saveFileDialog.FileName;
            return string.Empty;
        }

        private void LogRobotVariables()
        {
            _viewModel.RobotVariablesViewModel.WriteToLog();
        }

        private void LogGridModel()
        {
            _viewModel.GridViewModel.WriteToLog();
        }

        private string AddExtensionIfNotPresent(string filename)
        {
            if (filename.ToUpper().EndsWith(".TXT"))
                return filename;
            return filename + ".txt";
        }

        #endregion

    }
}
