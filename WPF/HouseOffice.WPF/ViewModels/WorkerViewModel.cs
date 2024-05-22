using HouseOffice.DAL;
using HouseOffice.DAL.Entities;
using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Helpers;
using HouseOffice.WPF.Repositories;
using HouseOffice.WPF.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Word = Microsoft.Office.Interop.Word;

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
        public ICommand DataGridSelectionChangedCommand { get; set; }
        public ICommand ComboBoxSelectionChangedCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }
        public RelayCommand PrintCommand { get; set; }

        public UserRequest SelectedUserRequest { get; set; }
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

            DataGridSelectionChangedCommand = new RelayCommand(OnDataGridSelectionChanged);
            ComboBoxSelectionChangedCommand = new RelayCommand(OnComboBoxSelectionChanged); 
            UpdateStatusCommand = new RelayCommand(OnUpdateStatusAsync);
            PrintCommand = new RelayCommand(OnPrintCommand);

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

            await UpdateRequestStyles();
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
        
        private async Task UpdateRequestStyles()
        {
            foreach (var request in UserRequests)
            {
                if (request.RequestDate.HasValue && (DateTime.Now - request.RequestDate.Value).Days > 30)
                {
                    request.IsOverdue = true;
                }
                else
                {
                    request.IsOverdue = false;
                }
            }
        }

        private void OnPrintCommand(object parameter)
        {
            if (SelectedUserRequest == null)
            {
                MessageBox.Show("Не выбрано заявление из списка");
                return;
            }
            else
            {
                Word.Application wordApp = new Word.Application();
                wordApp.Visible = false;

                try
                {
                    using var db = new ApplicationContext();
                    var user = db.Users.FirstOrDefault(x => x.Id == UserSession.CurrentUser.Id);
                    var userRequest = db.UserRequests.FirstOrDefault(x => x.UserId == UserSession.CurrentUser.Id);

                    var wordDocument = new Word.Document();

                    switch (userRequest.Requests.RequestType)
                    {
                        case "На улучшение жилищных условий":
                            wordDocument = wordApp.Documents.Open($"{Environment.CurrentDirectory}/Templates/Улучшение условий.docx");
                            break;
                    }

                    ReplaceWordStub("{FIO}", user.FIO.ToString(), wordDocument);
                    ReplaceWordStub("{Address}", user.Address.ToString(), wordDocument);
                    wordDocument.SaveAs2($"{Environment.CurrentDirectory}/Documents/Request{userRequest.Id}.docx");
                    wordApp.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }        
        }

        private static void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }
    }
}
