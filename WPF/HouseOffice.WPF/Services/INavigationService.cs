using HouseOffice.WPF.ViewModels;

namespace HouseOffice.WPF.Services
{
    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }
        ViewModelBase CurrentAdminView { get; }

        void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
        void NavigateToNested<TViewModel>() where TViewModel : ViewModelBase;
    }
}
