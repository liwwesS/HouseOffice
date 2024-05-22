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

namespace HouseOffice.WPF.ViewModels;

public class AddUserRequestViewModel : ViewModelBase
{
    public UserSession UserSession { get; set; }
    
    public IUserRepository UserRepository { get; set; }
    public IRequestRepository RequestRepository { get; set; }

    public INavigationService NavigationService { get; set; }
    
    public ICommand RequestSelectionChangedCommand { get; set; }
    public RelayCommand AddRequestAndNavigate { get; set; }
    public RelayCommand BackCommand { get; set; }

    public ObservableCollection<string> Requests { get; set; }

    public string SelectedRequest { get; set; }

    public string LandRegistrationFile { get; set; }
    public string PassportFile { get; set; }
    public string ProxyFile { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }

    public Visibility LifeUpgradeVisibility { get; set; } = Visibility.Visible;
    public Visibility CheckHouseVisibility { get; set; } = Visibility.Collapsed;
    public Visibility WorkStageVisibility { get; set; } = Visibility.Collapsed;
    public Visibility FamilyVisibility { get; set; } = Visibility.Collapsed;
    public Visibility KadastrVisibility { get; set; } = Visibility.Collapsed;
    public Visibility LeftColumn { get; set; } = Visibility.Collapsed;

    public AddUserRequestViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository, IRequestRepository requestRepository)
    {
        NavigationService = navigationService;
        UserSession = userSession;
        UserRepository = userRepository;
        RequestRepository = requestRepository;


        Requests = new ObservableCollection<string>();
        AddRequestAndNavigate = new RelayCommand(OnAddRequestAndNavigate);
        RequestSelectionChangedCommand = new RelayCommand(OnRequestSelectionChanged);
        BackCommand = new RelayCommand(o => { NavigationService.NavigateTo<AccountViewModel>(); }, o => true);

        _ = GetDataAsync();
    }

    private async Task GetDataAsync()
    {
        await using var context = new ApplicationContext();
        var requests = await context.Requests.Select(x => x.RequestType.ToString()).Distinct().ToListAsync();
        var selectedRequest = await context.Requests.FirstOrDefaultAsync(x => x.Id == 1);

        Requests = new ObservableCollection<string>(requests);
        SelectedRequest = selectedRequest.RequestType;
    }

    private async Task AddUserRequestAsync()
    {
        await using var context = new ApplicationContext();
        var requestType = await context.Requests.FirstOrDefaultAsync(x => x.RequestType == SelectedRequest);
        var requestId = requestType.Id;

        if (requestId == 3)
        {
            var request = new UserRequest()
            {
                UserId = UserSession.CurrentUser.Id,
                StatusId = 1,
                RequestId = requestId,
                RequestDate = DateTime.Now,
                PassportFile = PassportFile,
                LandRegistrationFile = LandRegistrationFile,
                ProxyFile = ProxyFile,
            };

            await RequestRepository.AddAsync(request);

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == UserSession.CurrentUser.Id);
            if (user != null)
            {
                user.Address = Address;
                user.PhoneNumber = PhoneNumber;
            }

            await context.SaveChangesAsync();
        }
        else
        {
            var request = new UserRequest()
            {
                UserId = UserSession.CurrentUser.Id,
                StatusId = 1,
                RequestId = requestId,
                RequestDate = DateTime.Now
            };

            await RequestRepository.AddAsync(request);

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == UserSession.CurrentUser.Id);
            if (user != null)
            {
                user.Address = Address;
                user.PhoneNumber = PhoneNumber;
            }

            await context.SaveChangesAsync();
        }   
    }
    
    private async void OnAddRequestAndNavigate(object parameter)
    {
        await AddUserRequestAsync();
        NavigationService.NavigateTo<AccountViewModel>();
    }

    private void OnRequestSelectionChanged(object parameter)
    {
        var args = parameter as SelectionChangedEventArgs;
        if (args?.Source is not ComboBox comboBox) return;

        if (comboBox.SelectedItem is not string selectedTransaction) return;
        if (Requests.Contains(selectedTransaction))
        {
            switch (selectedTransaction)
            {
                case "На улучшение жилищных условий":
                    LeftColumn = Visibility.Collapsed;
                    FamilyVisibility = Visibility.Collapsed;
                    CheckHouseVisibility = Visibility.Collapsed;
                    WorkStageVisibility = Visibility.Collapsed;
                    KadastrVisibility = Visibility.Collapsed;
                    LifeUpgradeVisibility = Visibility.Visible;
                    break;
                case "Выдача справки о составе семьи":
                    LeftColumn = Visibility.Visible;
                    CheckHouseVisibility = Visibility.Collapsed;
                    WorkStageVisibility = Visibility.Collapsed;
                    KadastrVisibility = Visibility.Collapsed;
                    LifeUpgradeVisibility = Visibility.Collapsed;
                    FamilyVisibility = Visibility.Visible;
                    break;
                case "На обследование дома":
                    LeftColumn = Visibility.Visible;
                    WorkStageVisibility = Visibility.Collapsed;
                    KadastrVisibility = Visibility.Collapsed;
                    LifeUpgradeVisibility = Visibility.Collapsed;
                    FamilyVisibility = Visibility.Collapsed;
                    CheckHouseVisibility = Visibility.Visible;
                    break;
                case "На снятие дома с кадастрого учёта":
                    LeftColumn = Visibility.Visible;
                    WorkStageVisibility = Visibility.Collapsed;
                    LifeUpgradeVisibility = Visibility.Collapsed;
                    FamilyVisibility = Visibility.Collapsed;
                    CheckHouseVisibility = Visibility.Collapsed;
                    KadastrVisibility = Visibility.Visible;
                    break;
                case "Выдача справки о трудовом стаже":
                    LeftColumn = Visibility.Visible;
                    LifeUpgradeVisibility = Visibility.Collapsed;
                    FamilyVisibility = Visibility.Collapsed;
                    CheckHouseVisibility = Visibility.Collapsed;
                    KadastrVisibility = Visibility.Collapsed;
                    WorkStageVisibility = Visibility.Visible;
                    break;
            }
        }
    }
}