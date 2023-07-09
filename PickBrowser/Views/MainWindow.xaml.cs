using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using PickBrowser.ViewModels;

namespace PickBrowser.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
		this.DataContext = Ioc.Default.GetService<MainWindowViewModel>();
    }

	private void BrowserView_GiveFeedback(object sender, GiveFeedbackEventArgs e) {

    }
}
