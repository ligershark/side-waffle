using System.Windows;
using Caliburn.Micro;
using $safeprojectname$.ViewModels;

namespace $safeprojectname$
{
	public class AppBootstrapper : BootstrapperBase
	{
		public AppBootstrapper()
		{
			Initialize();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<ShellViewModel>();
		}
	}
}
