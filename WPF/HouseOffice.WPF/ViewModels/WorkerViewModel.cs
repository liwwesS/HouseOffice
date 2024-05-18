using HouseOffice.DAL;
using HouseOffice.DAL.Entities;
using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Helpers;
using HouseOffice.WPF.Repositories;
using HouseOffice.WPF.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HouseOffice.WPF.ViewModels
{
    public class WorkerViewModel : ViewModelBase
    {
        public UserSession UserSession { get; set; }

        public IUserRepository UserRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }

        public INavigationService NavigationService { get; set; }

        public ObservableCollection<UserRequest> UserRequests { get; set; }
        public ObservableCollection<string> StatusComboBox { get; set; }

        public ICommand UpdateStatusCommand { get; set; }
        public string SelectedStatus { get; set; }

        private readonly UserRequestUpdater _userRequestUpdater;

        public WorkerViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository, IRequestRepository requestRepository)
        {
            NavigationService = navigationService;
            UserSession = userSession;
            UserRepository = userRepository;
            RequestRepository = requestRepository;

            UserRequests = new ObservableCollection<UserRequest>();
            StatusComboBox = new ObservableCollection<string>();

            _userRequestUpdater = new UserRequestUpdater(UserRequests);

            UpdateStatusCommand = new RelayCommand(OnUpdateStatusAsync);

            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await using var context = new ApplicationContext();

            var statusComboBox = await context.Status.Select(x => x.StatusType.ToString()).Distinct().ToListAsync();
            var userRequests = await RequestRepository.GetAllAsync();
            
            foreach (var userRequest in userRequests)
            {
                SelectedStatus = userRequest.Status.StatusType.ToString();
            }

            StatusComboBox = new ObservableCollection<string>(statusComboBox);
            _userRequestUpdater.UpdateRequests(userRequests);
        }

        private async void OnUpdateStatusAsync(object parameter)
        {
            await UpdateUserRequestAsync();
        }

        private async Task UpdateUserRequestAsync()
        {
            if (SelectedStatus == null) return;

            var selectedUserRequest = UserRequests.FirstOrDefault(ur => ur.UserId == UserSession.CurrentUser.Id);
            if (selectedUserRequest == null) return;

            selectedUserRequest.Status.StatusType = SelectedStatus;

            await RequestRepository.UpdateAsync(selectedUserRequest);

            _userRequestUpdater.UpdateRequests(await RequestRepository.GetAllAsync());
        }
    }
}
