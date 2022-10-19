using System.Drawing;
using RedCipher.Models.FileProcessing;

namespace RedCipher.Models.Coder
{
    /// <summary>
    /// Encodes/Decodes data into binary string.
    /// </summary>
    public class CipherCoder
    {
        public enum State
        {
            Hiding,
            FillingWithZeros
        };

        public Bitmap Encode(string message, Bitmap bmp)
        {
            State state = State.Hiding;
            int charIndex = 0;
            int charValue = 0;
            long pixelElementIndex = 0;
            int zeros = 0;

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);
                    int R = pixel.R - pixel.R % 2;
                    int G = pixel.G - pixel.G % 2;
                    int B = pixel.B - pixel.B % 2;

                    for (int n = 0; n < 3; n++)
                    {
                        if (pixelElementIndex % 8 == 0)
                        {
                            if (state == State.FillingWithZeros && zeros == 8)
                            {
                                if ((pixelElementIndex - 1) % 3 < 2)
                                {
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                return bmp;
                            }

                            if (charIndex >= message.Length) state = State.FillingWithZeros;
                            else { charValue = message[charIndex++]; }
                        }

                        switch (pixelElementIndex % 3)
                        {
                            case 0:
                                if (state != State.Hiding) break;
                                R += charValue % 2;
                                charValue /= 2;
                                break;
                            case 1:
                                if (state != State.Hiding) break;
                                G += charValue % 2;
                                charValue /= 2;
                                break;
                            case 2:
                                if (state == State.Hiding)
                                {
                                    B += charValue % 2;
                                    charValue /= 2;
                                }
                                bmp.SetPixel(j, i, Color.FromArgb(R, G, B)); 
                                break;
                        }

                        pixelElementIndex++;

                        if (state == State.FillingWithZeros) zeros++;
                    }
                }
            }

            return bmp;
        }

        public string Decode(Bitmap bmp)
        {
            string extractedText = "";
            int colorUnitIndex = 0;
            int charValue = 0;


            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

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