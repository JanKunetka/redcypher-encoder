using System.Drawing;
using System.Drawing.Imaging;

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

                if (File.Exists(path)) File.Delete(path);

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
        /// Loads an image file and returns it as a <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="path">The path to file.</param>
        /// <returns></returns>
        /// <exception cref="IOException">Is thrown when there is a problem with the file.</exception>
        public static Bitmap Load(string path)
        {
            GC.Collect(); 
            GC.WaitForPendingFinalizers(); 
            
            if (Path.GetInvalidFileNameChars().All(path.Contains)) throw new IOException($"The path of '{path}' contains invalid symbols.");

            Image img;
            try
            {
                using(FileStream fs = new(path, FileMode.Open))
                {
                    img = Image.FromStream(fs);
                    fs.Close();
                }
            }
            catch (IOException e)
            {
                Image.FromFile(path).Dispose();
                using(FileStream fs = new(path, FileMode.Open))
                {
                    img = Image.FromStream(fs);
                    fs.Close();
                }
            }

            return new Bitmap(img);
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
        
        
        
    }
}