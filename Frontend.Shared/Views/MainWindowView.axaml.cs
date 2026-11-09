using Avalonia.Controls;
using FrontendShared.ViewModels;

namespace FrontendShared.Views;

public partial class MainWindowView : Window
{
    private MainWindowViewModel _data;

    public MainWindowView()
    {
        _data = DataContext as MainWindowViewModel?? throw new NullReferenceException("DataContext is null");
        InitializeComponent();
    }
}