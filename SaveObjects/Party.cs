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
using System.Collections.Generic;

namespace XCDESave
{
    public enum Character
    {
        None = 0,
        Shulk,
        Reyn,
        Fiora,
        Dunban,
        Sharla,
        Riki,
        Melia,
        Fiora_2,
        Dickson,
        Mumkhar,
        Alvis,
        Dunban_2,
        Dunban_3,
        Kino,
        Nene,
        Wunwun,
        Tutu,
        Drydry,
        Fofora,
        Faifa,
        Hekasa,
        Setset,
        TeiTei,
        Nonona,
        Dekadeka,
        Evelen,
        Tentoo
    }

    /*
    *   Party Structure
    *   The IDs of each character in the Party + Total Count of characters in Party
    *
    *   Offset      Type                    Name                Description
    *   0x00    (uint16) Character[12]  Characters          = The IDs of the Characters in party, in order
    *   0x18    uint8                   PartyMembersCount   = The No. of Characters in the Party
    */
    public class Party : ISaveObject
    {
        public const uint SIZE = 25;

        public Character[] Characters { get; set; } = new Character[12];
        public byte PartyMembersCount { get; set; }
        public Party(byte[] data)
        {
            for(int i = 0; i < Characters.Length; i++)
            {
                Characters[i] = (Character)BitConverter.ToUInt16(data.GetByteSubArray((uint)(i * 2), 2), 0);
                PartyMembersCount = data[0x18];
            }
        }
        public byte[] ToRawData()
        {
            List<byte> result = new List<byte>();

            foreach (ushort c in Characters)
                result.AddRange(BitConverter.GetBytes(c));
            result.Add(PartyMembersCount);

            return result.ToArray();
        }
    }
}