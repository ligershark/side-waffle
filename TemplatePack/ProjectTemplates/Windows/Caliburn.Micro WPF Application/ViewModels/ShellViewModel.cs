using Caliburn.Micro;

namespace $safeprojectname$.ViewModels
{
	public class ShellViewModel : PropertyChangedBase, IHaveDisplayName
    {
        public ShellViewModel()
        {
            DisplayName = "Shell Window";
        }

        /// <summary>
        /// Gets or Sets the Display Name
        /// </summary>
        public string DisplayName { get; set; }
    }
}
