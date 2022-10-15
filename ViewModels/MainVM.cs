using RedCipher.VMs.Core.Screens;

namespace RedCipher.VMs.Core
{
    /// <summary>
    /// The ViewModel, controlling the MainWindow.
    /// </summary>
    public class MainVM : ViewModelBase
    {
        private readonly NavigationService navigation;

        public MainVM()
        {
            navigation = NavigationService.Instance;
            navigation.OnVMChanged += WhenVMChanges;
            navigation.Navigate(new MainIndexScreen());
        }

        public ViewModelBase CurrentVM => navigation.CurrentVM;
        private void WhenVMChanges() => OnPropertyChanged(nameof(CurrentVM));

    }
}