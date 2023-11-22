using System.Windows.Threading;
using MoexIIS.Apps.ViewModels;

namespace MoexIIS.Apps.Views;

public sealed partial class MainView
{
    private MainViewModel ViewModel { get; } = new();
    private DispatcherTimer Timer { get; } = new()
    {
        Interval = TimeSpan.FromSeconds(30),
    };

    public MainView()
    {
        InitializeComponent();

        DataContext = ViewModel;

        Timer.Tick += Timer_Tick;
        Timer.Start();

        _ = ViewModel.UpdateModelAsync();
    }

    private async void Timer_Tick(object? sender, EventArgs e)
    {
        await ViewModel.UpdateModelAsync();
    }
}
