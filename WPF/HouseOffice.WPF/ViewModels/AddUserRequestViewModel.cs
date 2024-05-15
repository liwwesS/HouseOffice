using HouseOffice.DAL;
using HouseOffice.DAL.Entities;
using HouseOffice.WPF.Commands;
using HouseOffice.WPF.Helpers;
using HouseOffice.WPF.Repositories;
using HouseOffice.WPF.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace HouseOffice.WPF.ViewModels;

public class AddUserRequestViewModel : ViewModelBase
{
    public UserSession UserSession { get; set; }

    public IUserRepository UserRepository { get; set; }
    public IRequestRepository RequestRepository { get; set; }

    public INavigationService NavigationService { get; set; }

    public RelayCommand AddRequestAndNavigate { get; set; }

    public ObservableCollection<string> Requests { get; set; }

    public string SelectedRequest { get; set; }

    public AddUserRequestViewModel(INavigationService navigationService, UserSession userSession, IUserRepository userRepository, IRequestRepository requestRepository)
    {
        NavigationService = navigationService;
        UserSession = userSession;
        UserRepository = userRepository;
        RequestRepository = requestRepository;

        Requests = new ObservableCollection<string>();
        AddRequestAndNavigate = new RelayCommand(OnAddRequestAndNavigate);

        _ = GetDataAsync();
    }

    private async Task GetDataAsync()
    {
        await using var context = new ApplicationContext();
        var requests = await context.Requests.Select(x => x.RequestType.ToString()).Distinct().ToListAsync();

        Requests = new ObservableCollection<string>(requests);
    }

    private async Task AddUserRequestAsync()
    {
        await using var context = new ApplicationContext();
        var requestType = await context.Requests.FirstOrDefaultAsync(x => x.RequestType == SelectedRequest);
        var requestId = requestType.Id;

        var request = new UserRequest()
        {
            UserId = UserSession.CurrentUser.Id,
            Status = "Создано",
            RequestId = requestId,
        };

        await RequestRepository.AddAsync(request);
    }

    private async void OnAddRequestAndNavigate(object parameter)
    {
        await AddUserRequestAsync();
        NavigationService.NavigateTo<AccountViewModel>();
    }
}