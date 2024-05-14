using System.Windows.Input;

namespace HouseOffice.WPF.Commands;

public abstract class BaseCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter) => true;

    public abstract void Execute(object? parameter);
}