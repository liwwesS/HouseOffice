using HouseOffice.DAL.Entities;
using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Helpers;
using HouseOffice.WPF.Repositories;
using HouseOffice.WPF.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using HouseOffice.DAL;
using Microsoft.EntityFrameworkCore;

namespace HouseOffice.WPF.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        public UserSession UserSession { get; set; }

        public IUserRepository UserRepository { get; set; }
        public IRequestRepository RequestRepository { get; set; }

        public INavigationService NavigationService { get; set; }

        public ObservableCollection<UserRequest> UserRequests { get; set; }
        public ObservableCollection<string> StatusComboBox { get; set; }

        public ICommand UpdateStatusCommand { get; set; }
        public ICommand DataGridSelectionChangedCommand { get; set; }
        public ICommand ComboBoxSelectionChangedCommand { get; set; }

        public RelayCommand LogoutCommand { get; set; }

        public ICommand OpenEditDialogCommand { get; }
        public ICommand CloseEditDialogCommand { get; }
        public ICommand DeleteRequestCommand { get; }
        public ICommand ChangeRequestCommand { get; }

        public UserRequest SelectedUserRequest { get; set; }
        public User SelectedUser { get; set; }

        public string SelectedStatus { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SNILS { get; set; }
        public string Password { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssued { get; set; }
        public DateTime PassportDate { get; set; }

        public bool IsDialogOpen { get; set; }

        public Visibility EditVisibility { get; set; } = Visibility.Collapsed;

        private readonly UserRequestUpdater _userRequestUpdater;

        public AdminViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository, IRequestRepository requestRepository)
        {
            NavigationService = navigationService;
            UserSession = userSession;
            UserRepository = userRepository;
            RequestRepository = requestRepository;

            UserRequests = new ObservableCollection<UserRequest>();
            StatusComboBox = new ObservableCollection<string>();

            _userRequestUpdater = new UserRequestUpdater(UserRequests);

            DataGridSelectionChangedCommand = new RelayCommand(OnDataGridSelectionChanged);
            ComboBoxSelectionChangedCommand = new RelayCommand(OnComboBoxSelectionChanged);
            UpdateStatusCommand = new RelayCommand(OnUpdateStatusAsync);

            OpenEditDialogCommand = new RelayCommand(OnOpenEditDialogCommand);
            CloseEditDialogCommand = new RelayCommand(OnCloseEditDialogCommand);
            DeleteRequestCommand = new RelayCommand(OnDeleteRequestCommand);
            ChangeRequestCommand = new RelayCommand(OnChangeRequestCommand);

            LogoutCommand = new RelayCommand(o => { NavigationService.NavigateTo<LoginViewModel>(); }, o => true);

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
            UserRequests = new ObservableCollection<UserRequest>(userRequests);

            _userRequestUpdater.UpdateRequests(userRequests);
        }

        private async void OnOpenEditDialogCommand(object parameter)
        {
            if (parameter is UserRequest selectedUserRequest)
            {
                IsDialogOpen = true;
                EditVisibility = Visibility.Visible;

                await using var context = new ApplicationContext();
                SelectedUser = await context.Users.FirstOrDefaultAsync(x => x.Id == selectedUserRequest.Users.Id);

                Email = SelectedUser.Email;
                LastName = SelectedUser.LastName;
                FirstName = SelectedUser.FirstName;
                MiddleName = SelectedUser.MiddleName;
                SNILS = SelectedUser.SNILS;
                Password = SelectedUser.Password;
                PassportSeries = SelectedUser.PassportSeries;
                PassportNumber = SelectedUser.PassportNumber;
                PassportIssued = SelectedUser.PassportIssued;
                PassportDate = SelectedUser.PassportDate;

                EventMediator.OnDialogOpen();
            }
        }

        private void OnCloseEditDialogCommand(object parameter)
        {
            IsDialogOpen = false;
            EditVisibility = Visibility.Collapsed;
            EventMediator.OnDialogClose();
        }

        private async void OnChangeRequestCommand(object parameter)
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show("Вы уверены, что хотите изменить данные пользователя?", "Подтверждение изменения", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            SelectedUser.Email = Email;
            SelectedUser.LastName = LastName;
            SelectedUser.FirstName = FirstName;
            SelectedUser.MiddleName = MiddleName;
            SelectedUser.SNILS = SNILS;
            SelectedUser.Password = Password;
            SelectedUser.PassportSeries = PassportSeries;
            SelectedUser.PassportNumber = PassportNumber;
            SelectedUser.PassportIssued = PassportIssued;
            SelectedUser.PassportDate = PassportDate;

            await UserRepository.UpdateUserAsync(SelectedUser);

            await LoadDataAsync();
            IsDialogOpen = false;
            EventMediator.OnDialogClose();
        }

        private async void OnDeleteRequestCommand(object parameter)
        {
            if (parameter is UserRequest selectedUserRequest)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить заявление?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                await RequestRepository.DeleteAsync(selectedUserRequest);
                UserRequests.Remove(selectedUserRequest);

                await LoadDataAsync();
            }
        }

        private void OnDataGridSelectionChanged(object parameter)
        {
            var args = parameter as SelectionChangedEventArgs;
            if (args?.Source is not DataGrid dataGrid) return;

            SelectedUserRequest = dataGrid.SelectedItem as UserRequest;
        }

        private void OnComboBoxSelectionChanged(object parameter)
        {
            var args = parameter as SelectionChangedEventArgs;
            if (args?.Source is not ComboBox comboBox) return;

            var selectedStatus = comboBox.SelectedItem as string;
            if (selectedStatus == null) return;

            SelectedStatus = selectedStatus;
        }

        private async void OnUpdateStatusAsync(object parameter)
        {
            await UpdateUserRequestAsync();
        }

        private static async Task<int> GetStatusIdAsync(string statusType)
        {
            await using var context = new ApplicationContext();
            var status = await context.Status.FirstOrDefaultAsync(x => x.StatusType == statusType);
            return status?.Id ?? -1;
        }

        private async Task UpdateUserRequestAsync()
        {
            if (SelectedUserRequest == null || SelectedStatus == null) return;

            var result = MessageBox.Show("Вы уверены, что хотите изменить статус?", "Подтверждение изменения статуса", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            var selectedStatusId = await GetStatusIdAsync(SelectedStatus);

            if (selectedStatusId == -1)
            {
                return;
            }

            SelectedUserRequest.StatusId = selectedStatusId;

            await RequestRepository.UpdateAsync(SelectedUserRequest);
            _userRequestUpdater.UpdateRequests(await RequestRepository.GetAllAsync());
        }
    }
}
