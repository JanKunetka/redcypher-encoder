using System.Drawing;

namespace RedCipher.Models.Coder
{
    /// <summary>
    /// Encodes/Decodes data into binary string.
    /// </summary>
    public class CipherCoder
    {
        /// <summary>
        /// Encode a text message into an image.
        /// </summary>
        /// <param name="message">The message to encode.</param>
        /// <param name="image">The image bitmap to encode into.</param>
        /// <returns>The image bitmap file with an encoded message.</returns>
        public Bitmap Encode(string message, Bitmap image)
        {
            CoderState state = CoderState.Hiding;
            int charIndex = 0;
            int charValue = 0;
            long pixelElementIndex = 0;
            int zeros = 0;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = image.GetPixel(j, i);
                    int R = pixel.R - pixel.R % 2;
                    int G = pixel.G - pixel.G % 2;
                    int B = pixel.B - pixel.B % 2;

                    for (int n = 0; n < 3; n++)
                    {
                        if (pixelElementIndex % 8 == 0)
                        {
                            if (state == CoderState.FillingWithZeros && zeros == 8)
                            {
                                if ((pixelElementIndex - 1) % 3 < 2)
                                {
                                    image.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                return image;
                            }

                            if (charIndex >= message.Length) state = CoderState.FillingWithZeros;
                            else { charValue = message[charIndex++]; }
                        }

                        switch (pixelElementIndex % 3)
                        {
                            case 0:
                                if (state != CoderState.Hiding) break;
                                R += charValue % 2;
                                charValue /= 2;
                                break;
                            case 1:
                                if (state != CoderState.Hiding) break;
                                G += charValue % 2;
                                charValue /= 2;
                                break;
                            case 2:
                                if (state == CoderState.Hiding)
                                {
                                    B += charValue % 2;
                                    charValue /= 2;
                                }
                                image.SetPixel(j, i, Color.FromArgb(R, G, B)); 
                                break;
                        }

                        pixelElementIndex++;

                        if (state == CoderState.FillingWithZeros) zeros++;
                    }
                }
            }

            return image;
        }

        /// <summary>
        /// Decodes information from image, encoded by the <see cref="Encode"/>() method. If no information is found, returns an empty string.
        /// </summary>
        /// <param name="image">The image to decode from.</param>
        /// <returns>A hidden message or an empty string.</returns>
        public string Decode(Bitmap image)
        {
            string extractedText = "";
            int colorUnitIndex = 0;
            int charValue = 0;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = image.GetPixel(j, i);

                    for (int n = 0; n < 3; n++)
                    {
                        charValue = (colorUnitIndex % 3) switch
                        {
                            0 => charValue * 2 + pixel.R % 2,
                            1 => charValue * 2 + pixel.G % 2,
                            2 => charValue * 2 + pixel.B % 2,
                            _ => charValue
                        };

                        colorUnitIndex++;

                        if (colorUnitIndex % 8 != 0) continue;
                        
                        charValue = ReverseBits(charValue);
                        if (charValue == 0) { return extractedText; }

                        char c = (char)charValue;
                        extractedText += c.ToString();
                    }
                }
            }

            return extractedText;
        }

        /// <summary>
        /// Reverses the order of the inserted bits.
        /// </summary>
        /// <param name="bits">The bit value to reverse.</param>
        /// <returns>The value reversed.</returns>
        private int ReverseBits(int bits)
        {
            int result = 0;
            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + bits % 2;
                bits /= 2;
            }
            return result;
        }
    }
}