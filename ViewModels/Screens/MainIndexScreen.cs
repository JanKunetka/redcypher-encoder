using System.IO;
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
        private const string DefaultFileNameText = "Click to open image";
        private const string DialogFileSaveFilter = "Image files (*.png;*.bmp)|*.png;*.bmp";
        private const string DialogFileOpenFilter = "Image files (*.png;*.bmp)|*.png;*.bmp";
        private const string NewFileIdentifier = "_copy";

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

        private ImageSource? iconImage;

        public ImageSource? IconImage
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

            ResetImageInfo();
        }

        private void WhenOpenFile(object _)
        {
            OpenFileDialog dialog = new();
            dialog.FileName = "Image";
            dialog.Filter = DialogFileOpenFilter;

            bool? results = dialog.ShowDialog();
            if (results != true) return;

            communicator.SetFile(dialog.FileName);
            FileNameTitle = communicator.FileName;
            IconImage = new BitmapImage(new Uri(dialog.FileName));
        }

        private void WhenClearFile(object _)
        {
            communicator.ClearFile();
            ResetImageInfo();
        }

        private void WhenEncodeMessage(object _)
        {
            if (!communicator.IsAnyDataLoaded())
            {
                MessageBox.Show("No file was loaded yet.");
                return;
            }

            if (SecretMessage.Length <= 0)
            {
                MessageBox.Show("First you have to write a message to encode.");
                return;
            }

            communicator.Encode(SecretMessage);
        }

        private void WhenEncodingSuccessful(string fileName)
        {
            string newFilePath =
                fileName.Insert(fileName.Length - Path.GetExtension(fileName).Length, NewFileIdentifier);

            SaveFileDialog dialog = new();
            dialog.Filter = DialogFileSaveFilter;
            dialog.FileName = newFilePath;
            bool? results = dialog.ShowDialog();

            if (results != true) return;

            communicator.SaveFile(dialog.FileName);
        }

        private void WhenDecodeMessage(object _)
        {
            if (!communicator.IsAnyDataLoaded())
            {
                MessageBox.Show("No file was loaded yet.");
                return;
            }

            SecretMessage = communicator.Decode();
        }

        /// <summary>
        /// Resets the information about the picture to default.
        /// </summary>
        private void ResetImageInfo()
        {
            IconImage = new BitmapImage(new Uri(DefaultIcon));
            FileNameTitle = DefaultFileNameText;
        }
    }
}