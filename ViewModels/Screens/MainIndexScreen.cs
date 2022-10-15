using Microsoft.Win32;

namespace RedCipher.VMs.Core.Screens
{
    public class MainIndexScreen : ViewModelBase
    {
        public RelayCommand OpenFileCommand { get; }

        public MainIndexScreen()
        {
            OpenFileCommand = new RelayCommand(WhenOpenFile);
        }

        private void WhenOpenFile(object _)
        {
            OpenFileDialog dialog = new();
            dialog.FileName = "Document";

            bool? results = dialog.ShowDialog();
            
        }
    }
}