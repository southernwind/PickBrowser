using System.Windows;

using PickBrowser.ViewModels;

namespace PickBrowser.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        this.InitializeComponent();
		this.DataContext = mainWindowViewModel;
    }
}
