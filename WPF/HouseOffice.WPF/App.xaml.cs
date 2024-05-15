using HouseOffice.DAL;
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

        services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));
        services.AddSingleton<INavigationService, NavigationService>();

        _serviceProvider = services.BuildServiceProvider();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {

        await using var context = new ApplicationContext();
        await context.Database.EnsureCreatedAsync().ConfigureAwait(false);


        Application.Current.Dispatcher.Invoke(() =>
        {
            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        });

        
    }
}