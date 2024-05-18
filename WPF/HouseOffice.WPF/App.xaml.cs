using HouseOffice.DAL;
using HouseOffice.WPF.Helpers;
using HouseOffice.WPF.Repositories;
using HouseOffice.WPF.Services;
using HouseOffice.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HouseOffice.WPF;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainViewModel>()
        });

        services.AddDbContext<ApplicationContext>();

        services.AddSingleton<MainViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegisterViewModel>();
        services.AddTransient<AccountViewModel>();
        services.AddTransient<WorkerViewModel>();
        services.AddTransient<AdminViewModel>();
        services.AddTransient<BossViewModel>();
        services.AddTransient<AddUserRequestViewModel>();

        services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<UserSession>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IRequestRepository, RequestRepository>();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await using var context = new ApplicationContext();
        await context.Database.EnsureCreatedAsync().ConfigureAwait(false);

        Current.Dispatcher.Invoke(() =>
        {
            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        });      
    }
}