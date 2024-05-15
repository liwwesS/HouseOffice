using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Services;
using System.Windows.Input;

namespace HouseOffice.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public INavigationService Navigation { get; set; }

        public ICommand DragMoveCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand MinimizeCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
            Navigation.NavigateTo<AddUserRequestViewModel>();

            DragMoveCommand = new DragMoveCommand();
            CloseCommand = new CloseCommand();
            MinimizeCommand = new MinimizeCommand();
        }
    }
}
