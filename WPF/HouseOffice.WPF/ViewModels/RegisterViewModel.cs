using HouseOffice.DAL.Entities;
using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Helpers;
using HouseOffice.WPF.Repositories;
using HouseOffice.WPF.Services;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;

namespace HouseOffice.WPF.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public UserSession UserSession { get; set; }
        public IUserRepository UserRepository { get; set; }

        public INavigationService NavigationService { get; set; }

        public RelayCommand NavigateToLoginCommand { get; }
        public RelayCommand RegisterCommand { get; }

        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SNILS { get; set; }
        public string Password { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssued { get; set; }
        public string Error { get; set; }
        public DateTime PassportDate { get; set; }

        public RegisterViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository)
        {
            NavigationService = navigationService;
            UserSession = userSession;
            UserRepository = userRepository;

            PassportDate = DateTime.Today;

            NavigateToLoginCommand = new RelayCommand(o => { NavigationService.NavigateTo<LoginViewModel>(); }, o => true);
            RegisterCommand = new RelayCommand(OnRegisterAndNavigate);
        }

        private bool ValidateUser(out List<string> validationErrors)
        {
            var user = new User()
            {
                Email = Email,
                LastName = LastName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                SNILS = SNILS,
                PassportSeries = PassportSeries,
                PassportNumber = PassportNumber,
                PassportIssued = PassportIssued,
                PassportDate = PassportDate,
                Password = Password,
                RoleId = 4
            };

            var context = new ValidationContext(user);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            bool isValid = Validator.TryValidateObject(user, context, results, true);

            validationErrors = results.Select(r => r.ErrorMessage).ToList();
            return isValid;
        }

        private async Task RegisterUserAsync()
        {
            var user = new User()
            {
               Email = Email,
               LastName = LastName,
               FirstName = FirstName,
               MiddleName = MiddleName,
               SNILS = SNILS,
               PassportSeries = PassportSeries,
               PassportNumber = PassportNumber,
               PassportIssued = PassportIssued,
               PassportDate = PassportDate,
               Password = Password,
               RoleId = 4
            };
   
            await UserRepository.AddAsync(user);
            UserSession.CurrentUser = user;
        }

        private async void OnRegisterAndNavigate(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            Password = passwordBox.Password;

            if (ValidateUser(out List<string> validationErrors))
            {
                await RegisterUserAsync();
                NavigationService.NavigateTo<LoginViewModel>();
            }
            else
            {
                string errorMessages = string.Join("\n", validationErrors);
                MessageBox.Show(errorMessages, "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
