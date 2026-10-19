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

namespace XCDESave
{
    /*
    *   RealTime Structure
    *   A real-world date and time, packed into one little-endian uint64 bitfield
    *
    *   Bits    Name        Description
    *   0-20    Unk_Low     = ???   [UNKNOWN, PLEASE INVESTIGATE] (possibly milliseconds + padding)
    *   21-26   Second      = 0~59
    *   27-32   Minute      = 0~59
    *   33-37   Hour        = 0~23
    *   38-40   DayOfWeek   = 0 = Sunday ~ 6 = Saturday
    *   41-45   Day         = 1~31
    *   46-49   Month       = 1~12
    *   50-63   Year        = Ex. 2026
    */
    public class RealTime : ISaveObject
    {
        public const uint SIZE = 8;

        public uint Unk_Low { get; set; }
        public byte Second { get; set; }
        public byte Minute { get; set; }
        public byte Hour { get; set; }
        public byte DayOfWeek { get; set; }
        public byte Day { get; set; }
        public byte Month { get; set; }
        public ushort Year { get; set; }

        public RealTime(byte[] data)
        {
            ulong v = BitConverter.ToUInt64(data, 0);
            Unk_Low = (uint)(v & 0x1FFFFF);
            Second = (byte)((v >> 21) & 0x3F);
            Minute = (byte)((v >> 27) & 0x3F);
            Hour = (byte)((v >> 33) & 0x1F);
            DayOfWeek = (byte)((v >> 38) & 0x7);
            Day = (byte)((v >> 41) & 0x1F);
            Month = (byte)((v >> 46) & 0xF);
            Year = (ushort)(v >> 50);
        }

        public byte[] ToRawData()
        {
            ulong v = (ulong)(Unk_Low & 0x1FFFFF)
                | ((ulong)(Second & 0x3F) << 21)
                | ((ulong)(Minute & 0x3F) << 27)
                | ((ulong)(Hour & 0x1F) << 33)
                | ((ulong)(DayOfWeek & 0x7) << 38)
                | ((ulong)(Day & 0x1F) << 41)
                | ((ulong)(Month & 0xF) << 46)
                | ((ulong)(Year & 0x3FFF) << 50);
            return BitConverter.GetBytes(v);
        }

        // null when the fields don't form a valid date (e.g. an unused save slot).
        public DateTime? ToDateTime()
        {
            try { return new DateTime(Year, Month, Day, Hour, Minute, Second); }
            catch (ArgumentOutOfRangeException) { return null; }
        }

        public override string ToString()
        {
            return ToDateTime()?.ToString("yyyy-MM-dd HH:mm:ss") ?? $"{Year}-{Month}-{Day} {Hour}:{Minute}:{Second}";
        }
    }
}
