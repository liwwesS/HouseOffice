using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Helpers;
using HouseOffice.WPF.Repositories;
using HouseOffice.WPF.Services;
using System.Windows;
using System.Windows.Controls;

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
            LoginCommand = new RelayCommand(async (o) => await CheckUserAsync(o));
        }

        private async Task CheckUserAsync(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            Password = passwordBox.Password;

            var user = await UserRepository.GetUserByEmailAsync(Login);

            if (user == null)
            {
                MessageBox.Show("Введен неверный логин.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (user.Password != Password)
            {
                MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            switch (user.RoleId)
            {
                case 1:
                    UserSession.CurrentUser = user;
                    NavigationService.NavigateTo<WorkerViewModel>();
                    break;
                case 2:
                    UserSession.CurrentUser = user;
                    NavigationService.NavigateTo<WorkerViewModel>();
                    break;
                case 3:
                    UserSession.CurrentUser = user;
                    NavigationService.NavigateTo<WorkerViewModel>();
                    break;
                case 4:
                    UserSession.CurrentUser = user;
                    NavigationService.NavigateTo<AccountViewModel>();
                    break;
            }
        }
    }
}