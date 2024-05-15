using HouseOffice.WPF.Services;

namespace HouseOffice.WPF.ViewModels;

public class AddRequestViewModel : ViewModelBase
{
    public INavigationService NavigationService { get; set; }

    public AddRequestViewModel(INavigationService navigationService)
    {
       NavigationService = navigationService;
    }
}