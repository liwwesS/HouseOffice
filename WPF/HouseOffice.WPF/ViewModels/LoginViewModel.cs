using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Helpers;
using HouseOffice.WPF.Repositories;
using HouseOffice.WPF.Services;
using System.Windows;

namespace HouseOffice.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public UserSession UserSession { get; set; }
        public IUserRepository UserRepository { get; set; }

        public INavigationService NavigationService { get; set; }

        public RelayCommand NavigateToRegisterCommand { get; }
        public RelayCommand LoginCommand { get; }

        public string Login { get; set; }
        public string Password { get; set; }

        public LoginViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
        {
            NavigationService = navigationService;
            UserSession = userSession;
            UserRepository = userRepository;

            NavigateToRegisterCommand = new RelayCommand(o => { NavigationService.NavigateTo<RegisterViewModel>(); }, o => true);
        }

        private async Task CheckUserAsync()
        {
            var user = await UserRepository.GetUserByEmailAsync(Login);

            if (user == null)
            {
                MessageBox.Show("Введен неверный логин.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserSession.CurrentUser = user;
        }
    }
}