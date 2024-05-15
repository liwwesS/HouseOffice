using HouseOffice.WPF.Services;

namespace HouseOffice.WPF.ViewModels;

public class AddUserRequestViewModel : ViewModelBase
{
    public INavigationService NavigationService { get; set; }

    public AddUserRequestViewModel(INavigationService navigationService)
    {
       NavigationService = navigationService;
    }
}