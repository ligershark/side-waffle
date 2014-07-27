using Caliburn.Micro;

namespace $safeprojectname$.ViewModels
{
	public class MainViewModel : PropertyChangedBase, IHaveDisplayName
    {
        public MainViewModel()
        {
            DisplayName = "Main Window";
        }

        /// <summary>
        /// Gets or Sets the Display Name
        /// </summary>
        public string DisplayName { get; set; }
    }
}
