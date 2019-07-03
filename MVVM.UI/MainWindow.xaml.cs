using MVVM.UI.ViewModel;
using System;
using System.Windows;

namespace MVVM.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel  viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindows_Loaded;
        }

        private async void MainWindows_Loaded(object sender, RoutedEventArgs e)
        {
          await  _viewModel.LoadAsync();
        }
    }
}
