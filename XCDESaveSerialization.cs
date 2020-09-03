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
    public static class XCDESaveSerialization
    {
        public static XCDESaveData Deserialize(Byte[] data)
        {
            switch (data.Length)
            {
                case XCDESaveData.SIZE:
                    Console.WriteLine("XCDESave detected.");
                    return new XCDESaveData(data);

                default:
                    string message = "Given save data has incorrect filesize!" + Environment.NewLine +
                        Environment.NewLine +
                        "Expected Size:" + Environment.NewLine +
                        String.Format("0x{0:X} for XCDE save data.", XCDESaveData.SIZE) + Environment.NewLine +
                        Environment.NewLine +
                        "Actual Size: 0x" + data.Length.ToString("X");
                    throw new Exception(message);
            }
        }
        public static byte[] Serialize(XCDESaveData save)
        {
            return save.ToRawData();
        }
    }
}
