/*
 * Copyright (C) 2020  damysteryman
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.IO;

namespace XCDESave
{
    public class XCDESaveThumbnail
    {
        public enum Type
        {
            TMB,
            BMP
        }

        private readonly byte[] BMP_HEADER = { 0x42, 0x4D, 0x36, 0xE1, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00, 0x40, 0x01, 0x00, 0x00, 0xB4, 0x00, 0x00, 0x00, 0x01, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE1, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private const int BMP_HEADER_SIZE = 54;
        private const int WIDTH = 320;
        private const int HEIGHT = 180;
        private const int TMB_SIZE = WIDTH * HEIGHT * 3;
        private const int BMP_SIZE = TMB_SIZE + BMP_HEADER_SIZE;
        private byte[] tmb_rawdata;

        public byte[] TMB { get { return tmb_rawdata; }}
        public byte[] BMP { get { return TmbToBmp(tmb_rawdata); }}

        public XCDESaveThumbnail(string path)
        {
            byte[] data = File.ReadAllBytes(path);

            if (path.EndsWith(".bmp"))
            {
                if (data.Length == BMP_SIZE)
                    tmb_rawdata = BmpToTmb(data);
                else
                    throw new Exception($"BMP File is invalid: Incorrect file size. Expected: {BMP_SIZE}, Actual: {data.Length}.");
            }
            else
            {
                if (data.Length == TMB_SIZE)
                    tmb_rawdata = data;
                else
                    throw new Exception($"TMB File is invalid: Incorrect file size. Expected: {TMB_SIZE}, Actual: {data.Length}.");
            }
        }
        private XCDESaveThumbnail(byte[] data)
        {
            if (data.Length == TMB_SIZE)
                tmb_rawdata = data;
            else
                throw new Exception($"TMB File is invalid: Incorrect file size. Expected: {TMB_SIZE}, Actual: {data.Length}.");
        }

        private byte[] ReversePixels(byte[] input)
        {
            byte[] raw = new byte[input.Length];
            byte[] output = new byte[input.Length];
            Array.Copy(input, raw, input.Length);
            Array.Reverse(raw);

            int row_bytes_count = WIDTH * 3;
            for (int y = 0; y < HEIGHT; y++)
            {
                int start = row_bytes_count * y;

                for (int x = 0; x < row_bytes_count / 2; x = x + 3)
                {
                    byte[] temp = new byte[] { raw[start + x], raw[start + x + 1], raw[start + x + 2] };
                    Array.Copy(raw, start + row_bytes_count - 3 - x, output, start + x, 3);
                    Array.Copy(temp, 0, output, start + row_bytes_count - 3 - x, 3);
                }
            }

            return output;
        }

        private byte[] TmbToBmp(byte[] input)
        {
            byte[] output = new byte[BMP_HEADER.Length + TMB_SIZE];
            Array.Copy(BMP_HEADER, 0, output, 0, BMP_HEADER.Length);
            Array.Copy(ReversePixels(input), 0, output, BMP_HEADER_SIZE, input.Length);

            return output;
        }

        private byte[] BmpToTmb(byte[] input) => ReversePixels(input.GetByteSubArray((uint)BMP_HEADER.Length, TMB_SIZE));

        public void Export(string path, Type ft)
        {
            switch (ft)
            {
                case Type.TMB:
                    File.WriteAllBytes(path, TMB);
                    break;

                case Type.BMP:
                    File.WriteAllBytes(path, BMP);
                    break;
            }   
        }
    }
}
