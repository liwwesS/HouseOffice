using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Services;

namespace HouseOffice.WPF.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public INavigationService NavigationService { get; set; }

        public RelayCommand NavigateToLoginCommand { get; }

        public RegisterViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            NavigateToLoginCommand = new RelayCommand(o => { NavigationService.NavigateTo<LoginViewModel>(); }, o => true);
        }
    }
}
