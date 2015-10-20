﻿using System.Text;
using System.IO;

namespace Ohana3DS_Rebirth.Ohana
{
    class IOUtils
    {
        /// <summary>
        ///     Read an ASCII String from a given Reader at a given address.
        ///     Note that the text MUST end with a Null Terminator (0x0).
        ///     It doesn't advances the position after reading.
        /// </summary>
        /// <param name="input">The Reader of the File Stream</param>
        /// <param name="address">Address where the text begins</param>
        /// <returns></returns>
        public static string readString(BinaryReader input, uint address)
        {
            long originalPosition = input.BaseStream.Position;
            input.BaseStream.Seek(address, SeekOrigin.Begin);
            MemoryStream bytes = new MemoryStream();
            for (;;)
            {
                byte b = input.ReadByte();
                if (b == 0) break;
                bytes.WriteByte(b);
            }
            input.BaseStream.Seek(originalPosition, SeekOrigin.Begin);
            return Encoding.ASCII.GetString(bytes.ToArray());
        }

        /// <summary>
        ///     Read an ASCII String from a given Reader at a given address with given size.
        ///     It will also stop reading if a Null Terminator (0x0) is found.
        ///     It WILL advance the position until the count is reached, or a 0x0 is found.
        /// </summary>
        /// <param name="input">The Reader of the File Stream</param>
        /// <param name="address">Address where the text begins</param>
        /// <param name="count">Number of bytes that the text have</param>
        /// <returns></returns>
        public static string readString(BinaryReader input, uint address, uint count)
        {
            input.BaseStream.Seek(address, SeekOrigin.Begin);
            MemoryStream bytes = new MemoryStream();
            for (int i = 0; i < count; i++)
            {
                byte b = input.ReadByte();
                if (b == 0) break;
                bytes.WriteByte(b);
            }
            return Encoding.ASCII.GetString(bytes.ToArray());
        }
    }
}
