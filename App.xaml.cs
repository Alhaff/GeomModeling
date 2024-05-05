using GeometricModeling.Services;
using GeometricModeling.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GeometricModeling
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
  
    public partial class App : Application
    {
        private readonly IHost host;

        public App()
        {
            host = Host.CreateDefaultBuilder()
                       .ConfigureServices((context,services) =>
                       {
                           ConfigureServices(context.Configuration, services);
                       })
                       .Build();
        }
        private void ConfigureServices(IConfiguration configuration,
          IServiceCollection services)
        {
            services.AddSingleton<Func<Type, ViewModelBase>>(provider =>
               viewModelType => (ViewModelBase)provider.GetRequiredService(viewModelType));
            services.AddSingleton<DrawingEngineService>();
            services.AddSingleton<Transformation2DService>();
            services.AddSingleton<DrawingEngineViewModel>();
            services.AddSingleton<ModelParametersViewModel>();
            services.AddSingleton<SinusoidalSpiralViewModel>();
            services.AddSingleton<Lab3ViewModel>();
            services.AddSingleton<LinearTransformationViewModel>();
            services.AddSingleton<AffineTransformationViewModel>();
            services.AddSingleton<ProjectiveTransformationViewModel>();
            services.AddSingleton<DrawingParametersViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton(provider =>
           new MainWindow
           {
               DataContext = provider.GetRequiredService<MainWindowViewModel>()
           });
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();
            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}
