using System.Windows;

namespace SprayingSystem.SprayingSystemConfigViewer
{
    /// <summary>
    /// Interaction logic for JsonTreeView.xaml
    /// </summary>
    public partial class JsonTreeView : Window
    {
        private JsonTreeViewModel _viewModel;

        public JsonTreeView(string filename)
        {
            InitializeComponent();

            _viewModel = new JsonTreeViewModel(filename);
            DataContext = _viewModel;
        }

        public JsonTreeViewModel.EditStatus Edits
        {
            get { return _viewModel.Edits; }
        }
    }
}
