using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace RedCipher.Models.FileProcessing
{
    /// <summary>
    /// Contains useful method for working with files.
    /// </summary>
    public static class FileSystem
    {

        /// <summary>
        /// Save binary as a file.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="data">The data of the file.</param>
        public static void Save(string path, Bitmap data)
        {
            if (Path.GetInvalidFileNameChars().All(path.Contains)) throw new IOException($"The path of '{path}' contains invalid symbols.");

            try
            {
                string extension = Path.GetExtension(path);
                ImageFormat format = extension switch
                {
                    ".png" => ImageFormat.Png,
                    ".bmp" => ImageFormat.Bmp,
                    _ => throw new ArgumentOutOfRangeException(extension, "This format is not supported.")
                };

                data.Save(path, format);
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
        }
        
        /// <summary>
        /// Get the name and extension of a file from a path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>Filename</returns>
        public static string GetFileName(string path)
        {
            return path.Replace('/', '\\').Split('\\')[^1];
        }
        
        /// <summary>
        /// Does the file under a specific path exist?
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>TRUE if file exists.</returns>
        public static bool IsFile(string path) => File.Exists(path);
    }
}