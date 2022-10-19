using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using RedCipher.Models;

namespace RedCipher.VMs.Core.Screens
{
    /// <summary>
    /// Controls the Main Screen of the app.
    /// </summary>
    public class MainIndexScreen : ViewModelBase
    {
        private const string DefaultIcon = "pack://application:,,,/Views;component/Resource/img/img_AddFile.png";
        private const string DefaultFileNameText = "Click to open file";
        private const string dialogFileFilter = "Image files (*.png;*.bmp)|*.png;*.bmp";
        
        private readonly CipherCommunicator communicator;

        #region Properties

        private string secretMessage;
        public string SecretMessage
        {
            get => secretMessage;
            set
            {
                secretMessage = value;
                OnPropertyChanged();
            }
        }

        private ImageSource iconImage;
        public ImageSource IconImage
        {
            get => iconImage;
            set
            {
                iconImage = value;
                OnPropertyChanged();
            }
        }

        private string fileNameTitle;
        public string FileNameTitle
        {
            get => fileNameTitle;
            set
            {
                fileNameTitle = value;
                OnPropertyChanged();
            }
        }
        #endregion
        
        public RelayCommand OpenFileCommand { get; }
        public RelayCommand ClearFileCommand { get; }
        public RelayCommand EncodeMessageCommand { get; }
        public RelayCommand DecodeMessageCommand { get; }

        public MainIndexScreen()
        {
            communicator = new CipherCommunicator();
            communicator.OnEncodingSuccessful += WhenEncodingSuccessful;
            
            OpenFileCommand = new RelayCommand(WhenOpenFile);
            ClearFileCommand = new RelayCommand(WhenClearFile);
            EncodeMessageCommand = new RelayCommand(WhenEncodeMessage);
            DecodeMessageCommand = new RelayCommand(WhenDecodeMessage);

            IconImage = new BitmapImage(new Uri(DefaultIcon));
            FileNameTitle = DefaultFileNameText;
        }
        private void WhenOpenFile(object _)
        {
            OpenFileDialog dialog = new();
            dialog.FileName = "Image";

            bool? results = dialog.ShowDialog();
            if (results != true) return;
            
            communicator.SetFile(dialog.FileName);
            FileNameTitle = communicator.FileName;
            IconImage = new BitmapImage(new Uri(dialog.FileName));
        }
        
        private void WhenClearFile(object _)
        {
            communicator.ClearFile();
            FileNameTitle = DefaultFileNameText;
            IconImage = new BitmapImage(new Uri(DefaultIcon));
        }
        
        private void WhenEncodeMessage(object _)
        {
            if (!communicator.IsAnyDataLoaded()) { MessageBox.Show("No file was loaded yet."); return; }
            
            communicator.Encode(SecretMessage);
        }

        private void WhenEncodingSuccessful(string fileName)
        {
            SaveFileDialog dialog = new();
            dialog.Filter = dialogFileFilter;
            dialog.FileName = fileName;
            bool? results = dialog.ShowDialog();
            
            if (results != true) return;
            
            communicator.SaveFile(dialog.FileName);
        }
        
        private void WhenDecodeMessage(object _)
        {
            if (!communicator.IsAnyDataLoaded()) { MessageBox.Show("No file was loaded yet."); return; }
            SecretMessage = communicator.Decode();
        }
        
    }
}