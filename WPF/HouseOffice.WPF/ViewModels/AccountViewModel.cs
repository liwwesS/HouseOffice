using HouseOffice.DAL.Entities;
using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Helpers;
using HouseOffice.WPF.Repositories;
using HouseOffice.WPF.Services;
using System.Collections.ObjectModel;

namespace HouseOffice.WPF.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        public UserSession UserSession { get; set; }

        public IUserRepository UserRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }

        public INavigationService NavigationService { get; set; }

        public ObservableCollection<UserRequest> UserRequests { get; set; }
        private readonly UserRequestUpdater _userRequestUpdater;

        public RelayCommand NavigateToAddRequestCommand { get; }

        public AccountViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository, IRequestRepository requestRepository)
        {
            NavigationService = navigationService;
            UserSession = userSession;
            UserRepository = userRepository;
            RequestRepository = requestRepository;

            UserRequests = new ObservableCollection<UserRequest>();
            _userRequestUpdater = new UserRequestUpdater(UserRequests);

            NavigateToAddRequestCommand = new RelayCommand(o => { NavigationService.NavigateTo<AddUserRequestViewModel>(); }, o => true);

            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var userRequests = await RequestRepository.GetRequestsByUserIdAsync(UserSession.CurrentUser.Id);
            _userRequestUpdater.UpdateRequests(userRequests);
        }
    }
}
