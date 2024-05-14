using System.Windows;

namespace HouseOffice.WPF.Commands;

public class MinimizeCommand : BaseCommand
{
    public override void Execute(object? parameter)
    {
        if (parameter is not RoutedEventArgs args || args.OriginalSource is not FrameworkElement element) return;
        var window = Window.GetWindow(element);

        if (window != null)
        {
            window.WindowState = WindowState.Minimized;
        }
    }
}