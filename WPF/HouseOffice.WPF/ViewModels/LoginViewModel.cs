using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Services;

namespace HouseOffice.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public INavigationService NavigationService { get; set; }

        public RelayCommand NavigateToRegisterCommand { get; }

        public LoginViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            NavigateToRegisterCommand = new RelayCommand(o => { NavigationService.NavigateTo<RegisterViewModel>(); }, o => true);
        }
    }
}