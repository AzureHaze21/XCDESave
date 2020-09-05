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
    /*
    *   XCDESaveData Structure
    *   Save file for Xenoblade Chronchles: Definitive Edition [WORK IN PROGRESS, PLEASE RESEARCH]
    *
    *   Offset      Type                Name            Description
    *   0x000000    uint8[0x10]         Unk_0x000000    = ???   [UNKNOWN, PLEASE INVESTIGATE]
    *   0x000010    uint32              Noponstones     = No. of Noponstones
    *   0x000014    uint8[0x3AFC]       Unk_0x000014    = ???   [UNKNOWN, PLEASE INVESTIGATE]
    *   0x003B10    EquipItem[500]      WeaponBox       = Weapons in inventory (500pcs)
    *   0x0098d0    EquipItem[500]      HeadArmourBox   = Head Equipment in inventory (500pcs)
    *   0x00F690    EquipItem[500]      TorsoArmourBox  = Torso Equipment in inventory (500pcs)
    *   0x015450    EquipItem[500]      ArmArmourBox    = Arm Equipment in inventory (500pcs)
    *   0x01B210    EquipItem[500]      LegArmourBox    = Leg Equipment in inventory (500pcs)
    *   0x020FD0    EquipItem[500]      FootArmourBox   = Foot/Auxiliary Equipment in inventory (500pcs)
    *   0x026D90    CrystalItem[500]    CrystalBox      = Crystals in inventory (500pcs)
    *   0x02C380    CrystalItem[500]    GemBox          = Gems in inventory (500pcs)
    *   0x031970    Item[500]           CollectableBox  = Collectables in inventory (500pcs)
    *   0x034080    Item[500]           MaterialBox     = Materials in inventory (500pcs)
    *   0x036790    Item[500]           KeyItemBox      = Key items in inventory (500pcs)
    *   0x038EA0    Item[500]           ArtsManualBox   = Arts Manuals in inventory (500pcs)
    *   0x03B5B0    uint8[0x116590]     Unk_0x03B5B0    = ???   [UNKNOWN, PLEASE INVESTIGATE]
    *   0x151B40    uint32              Money           = Amount of Money
    *   0x151B44    uint8[0x7D4]        Unk_0x151B44    = ???   [UNKNOWN, PLEASE INVESTIGATE]
    *   0x152318    Party               Party           = Party structure, No. and IDs of characters in party
    *   0x152331    uint8[0x37]         Unk_0x152331    = ???   [UNKNOWN, PLEASE INVESTIGATE]
    *   0x152368    PartyMember[16]     PartyMembers    = Party Member Character Data
    *   0x1536E8    ArtsLevel[188]      ArtsLevels      = Arts Levels + Unlock Levels for each Art (except Ponspector Arts)
    */
    public class XCDESaveData : ISaveObject
    {
        public Dictionary<uint, string> ItemTypes;
        public const int SIZE = 0x153860;
        public byte[] Unk_0x000000 { get; set; }
        public byte[] Unk_0x000014 { get; set; }
        public uint Noponstones { get; set; }
        public EquipItem[] Weapons { get; set; } = new EquipItem[500];
        public EquipItem[] HeadArmour { get; set; } = new EquipItem[500];
        public EquipItem[] TorsoArmour { get; set; } = new EquipItem[500];
        public EquipItem[] ArmArmour { get; set; } = new EquipItem[500];
        public EquipItem[] LegArmour { get; set; } = new EquipItem[500];
        public EquipItem[] FootArmour { get; set; } = new EquipItem[500];
        public CrystalItem[] Crystals { get; set; } = new CrystalItem[500];
        public CrystalItem[] Gems { get; set; } = new CrystalItem[500];
        public Item[] Collectables { get; set; } = new Item[500];
        public Item[] Materials { get; set; } = new Item[500];
        public Item[] KeyItems { get; set; } = new Item[500];
        public Item[] ArtsManuals { get; set; } = new Item[500];
        public byte[] Unk_0x03B5B0 { get; set; }
        public uint Money { get; set; }
        public byte[] Unk_0x151B44 { get; set; }
        public Party Party { get; set; }
        public byte[] Unk_0x152331 { get; set; }
        public PartyMember[] PartyMembers { get; set; } = new PartyMember[16];
        public ArtsLevel[] ArtsLevels { get; set; } = new ArtsLevel[188];

        private static readonly Dictionary<string, uint> LOC = new Dictionary<string, uint>()
        {
            { "Unk_0x000000", 0x0 },
            { "Noponstones", 0x10 },
            { "Unk_0x000014", 0x14 },
            { "WeaponBox", 0x3B10 },
            { "HeadArmourBox", 0x98D0 },
            { "TorsoArmourBox", 0xF690 },
            { "ArmArmourBox", 0x15450 },
            { "LegArmourBox", 0x1B210 },
            { "FootArmourBox", 0x20FD0 },
            { "CrystalBox", 0x26D90 },
            { "GemBox", 0x2C380 },
            { "CollectableBox", 0x31970 },
            { "MaterialBox", 0x34080 },
            { "KeyItemBox", 0x36790},
            { "ArtsManualBox", 0x38EA0 },
            { "Unk_0x03B5B0", 0x3B5B0 },
            { "Money", 0x151B40 },
            { "Unk_0x151B44", 0x151B44 },
            { "Party", 0x152318 },
            { "Unk_0x152331", 0x152331 },
            { "PartyMembers", 0x152368 },
            { "ArtsLevels", 0x1536E8 }
        };

        public XCDESaveData(byte[] data)
        {
            Unk_0x000000 = data.GetByteSubArray(LOC["Unk_0x000000"], 0x10);
            Noponstones = BitConverter.ToUInt32(data.GetByteSubArray(LOC["Noponstones"], 0x4), 0);
            Unk_0x000014 = data.GetByteSubArray(LOC["Unk_0x000014"], LOC["WeaponBox"] - LOC["Unk_0x000014"]);
            for (uint i = 0; i < Weapons.Length; i++) Weapons[i] = new EquipItem(data.GetByteSubArray(LOC["WeaponBox"] + (i * EquipItem.SIZE), EquipItem.SIZE));
            for (uint i = 0; i < HeadArmour.Length; i++) HeadArmour[i] = new EquipItem(data.GetByteSubArray(LOC["HeadArmourBox"] + (i * EquipItem.SIZE), EquipItem.SIZE));
            for (uint i = 0; i < TorsoArmour.Length; i++) TorsoArmour[i] = new EquipItem(data.GetByteSubArray(LOC["TorsoArmourBox"] + (i * EquipItem.SIZE), EquipItem.SIZE));
            for (uint i = 0; i < ArmArmour.Length; i++) ArmArmour[i] = new EquipItem(data.GetByteSubArray(LOC["ArmArmourBox"] + (i * EquipItem.SIZE), EquipItem.SIZE));
            for (uint i = 0; i < LegArmour.Length; i++) LegArmour[i] = new EquipItem(data.GetByteSubArray(LOC["LegArmourBox"] + (i * EquipItem.SIZE), EquipItem.SIZE));
            for (uint i = 0; i < FootArmour.Length; i++) FootArmour[i] = new EquipItem(data.GetByteSubArray(LOC["FootArmourBox"] + (i * EquipItem.SIZE), EquipItem.SIZE));
            for (uint i = 0; i < Crystals.Length; i++) Crystals[i] = new CrystalItem(data.GetByteSubArray(LOC["CrystalBox"] + (i * CrystalItem.SIZE), CrystalItem.SIZE));
            for (uint i = 0; i < Gems.Length; i++) Gems[i] = new CrystalItem(data.GetByteSubArray(LOC["GemBox"] + (i * CrystalItem.SIZE), CrystalItem.SIZE));
            for (uint i = 0; i < Collectables.Length; i++) Collectables[i] = new Item(data.GetByteSubArray(LOC["CollectableBox"] + (i * Item.SIZE), Item.SIZE));
            for (uint i = 0; i < Materials.Length; i++) Materials[i] = new Item(data.GetByteSubArray(LOC["MaterialBox"] + (i * Item.SIZE), Item.SIZE));
            for (uint i = 0; i < KeyItems.Length; i++) KeyItems[i] = new Item(data.GetByteSubArray(LOC["KeyItemBox"] + (i * Item.SIZE), Item.SIZE));
            for (uint i = 0; i < ArtsManuals.Length; i++) ArtsManuals[i] = new Item(data.GetByteSubArray(LOC["ArtsManualBox"] + (i * Item.SIZE), Item.SIZE));
            Unk_0x03B5B0 = data.GetByteSubArray(LOC["Unk_0x03B5B0"], LOC["Money"] - LOC["Unk_0x03B5B0"]);
            Money = BitConverter.ToUInt32(data.GetByteSubArray(LOC["Money"], 0x4), 0);
            Unk_0x151B44 = data.GetByteSubArray(LOC["Unk_0x151B44"], LOC["Party"] - LOC["Unk_0x151B44"]);
            Party = new Party(data.GetByteSubArray(LOC["Party"], Party.SIZE));
            Unk_0x152331 = data.GetByteSubArray(LOC["Unk_0x152331"], LOC["PartyMembers"] - LOC["Unk_0x152331"]);
            for (uint i = 0; i < PartyMembers.Length; i++) PartyMembers[i] = new PartyMember(data.GetByteSubArray(LOC["PartyMembers"] + (i * PartyMember.SIZE), PartyMember.SIZE));
            for (uint i = 0; i < ArtsLevels.Length; i++) ArtsLevels[i] = new ArtsLevel(data.GetByteSubArray(LOC["ArtsLevels"] + (i * ArtsLevel.SIZE), ArtsLevel.SIZE));
        }

        public byte[] ToRawData()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Unk_0x000000);
            result.AddRange(BitConverter.GetBytes(Noponstones));
            result.AddRange(Unk_0x000014);
            foreach (EquipItem i in Weapons) result.AddRange(i.ToRawData());
            foreach (EquipItem i in HeadArmour) result.AddRange(i.ToRawData());
            foreach (EquipItem i in TorsoArmour) result.AddRange(i.ToRawData());
            foreach (EquipItem i in ArmArmour) result.AddRange(i.ToRawData());
            foreach (EquipItem i in LegArmour) result.AddRange(i.ToRawData());
            foreach (EquipItem i in FootArmour) result.AddRange(i.ToRawData());
            foreach (CrystalItem i in Crystals) result.AddRange(i.ToRawData());
            foreach (CrystalItem i in Gems) result.AddRange(i.ToRawData());
            foreach (Item i in Collectables) result.AddRange(i.ToRawData());
            foreach (Item i in Materials) result.AddRange(i.ToRawData());
            foreach (Item i in KeyItems) result.AddRange(i.ToRawData());
            foreach (Item i in ArtsManuals) result.AddRange(i.ToRawData());
            result.AddRange(Unk_0x03B5B0);
            result.AddRange(BitConverter.GetBytes(Money));
            result.AddRange(Unk_0x151B44);
            result.AddRange(Party.ToRawData());
            result.AddRange(Unk_0x152331);
            foreach (PartyMember p in PartyMembers) result.AddRange(p.ToRawData());
            foreach (ArtsLevel a in ArtsLevels) result.AddRange(a.ToRawData());
            
            return result.ToArray();
        }
    }
}
