using System.Drawing;
using RedCipher.Models.Coder;
using RedCipher.Models.FileProcessing;

namespace RedCipher.Models
{
    /// <summary>
    /// Communicates with higher levels of the application.
    /// </summary>
    public class CipherCommunicator
    {
        public event Action<string>? OnEncodingSuccessful;
        
        private readonly CipherCoder coder;
        private string fileName;
        private Bitmap? imageData;

        public CipherCommunicator() => coder = new CipherCoder();

        /// <summary>
        /// Set a file for processing by the app.
        /// </summary>
        /// <param name="path">The path of the file to load.</param>
        public void SetFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            fileName = FileSystem.GetFileName(path);
            imageData = new Bitmap(path);
        }

        /// <summary>
        /// Clear the loaded data.
        /// </summary>
        public void ClearFile()
        {
            fileName = "";
            imageData = null;
        }

        /// <summary>
        /// Save the set file under a specific path.
        /// </summary>
        /// <param name="path">The path to save under.</param>
        public void SaveFile(string path)
        {
            EnsureDataIsLoaded();
            imageData.Save(path);
        }

        /// <summary>
        /// Encode a string into the set file.
        /// </summary>
        /// <param name="message">The message to encode.</param>
        public void Encode(string message)
        {
            EnsureDataIsLoaded();
            if (string.IsNullOrEmpty(message)) return;
            imageData = coder.Encode(message, imageData);
            OnEncodingSuccessful?.Invoke(fileName);
        }

        /// <summary>
        /// Tries to decode a message from teh set file.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            EnsureDataIsLoaded();
            return coder.Decode(imageData);
        }

        public bool IsAnyDataLoaded() => imageData != null;

        /// <summary>
        /// Shuts down the action if no data is loaded.
        /// </summary>
        /// <exception cref="InvalidOperationException">Is thrown when no data is loaded.</exception>
        private void EnsureDataIsLoaded()
        {
            if (IsAnyDataLoaded()) return;
            throw new InvalidOperationException("Cannot apply operation on non-loaded data!");
        }
        
        public string FileName { get => fileName; }
    }
}