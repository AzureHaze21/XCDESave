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
    *   PartyMember Structure
    *   Data for an individual Party Member/Player Character
    *
    *   Offset  Type        Name                    Description
    *   0x000   uint32      Level                   = Character's Level
    *   0x004   uint32      EXP                     = Character's amount of EXP
    *   0x008   uint32      AP                      = Character's amount of AP
    *   0x00C   uint32      AffinityCoins           = Character's No. of Affinity Coins
    *   0x010   uint8[4]    Unk_1                   = ???   [UNKNOWN, PLEASE INVESTIGATE]
    *   0x014   ItemID      HeadEquip               = Equipped Head Armour Item Type + INDEX in inventory (NOT Item's actual ID)
    *   0x018   ItemID      TorsoEquip              = Equipped Torso Armour Item Type + INDEX in inventory (NOT Item's actual ID)
    *   0x01C   ItemID      ArmEquip                = Equipped Arm Armour Item Type + INDEX in inventory (NOT Item's actual ID)
    *   0x020   ItemID      LegEquip                = Equipped Leg Armour Item Type + INDEX in inventory (NOT Item's actual ID)
    *   0x024   ItemID      FootEquip               = Equipped Foot Armour Item Type + INDEX in inventory (NOT Item's actual ID)
    *   0x028   ItemID      WeaponEquip             = Equipped Weapon Item Type + INDEX in inventory (NOT Item's actual ID)
    *   0x02C   uint8[4]    Unk_2                   = ???   [UNKNOWN, PLEASE INVESTIGATE]
    *   0x030   uint32      HeadCosmetic            = ID of equipped Head Cosmetic
    *   0x034   uint32      TorsoCosmetic           = ID of equipped Torso Cosmetic
    *   0x038   uint32      ArmCosmetic             = ID of equipped Arm Cosmetic
    *   0x03C   uint32      LegCosmetic             = ID of equipped Leg Cosmetic
    *   0x040   uint32      FootCosmetic            = ID of equipped Foot Cosmetic
    *   0x044   uint32      WeaponCosmetic          = ID of equipped Weapon Cosmetic
    *   0x048   uint16[9]   Arts                    = IDs of Character's set arts, 1st ID is Talent Art, IDs 2~9 are normal set Arts
    *   0x05A   uint16[9]   MonadoArts              = same as Arts, but 2nd set for when "Activate Monado" is used
    *   0x06C   uint8[0xC]  Unk_3                   = ???   [UNKNOWN, PLEASE INVESTIGATE]
    *   0x078   uint32      SelectedSkillTreeIndex  = Index of the Character's Currently Selected Skill Tree
    *   0x07C   uint32[5]   SkillTreeSPs            = Skill Points (SP) for each of the Character's Skill Trees
    *   0x090   uint32[5]   SkillTreeUnlockedSkills = The No. of Skills Unlocked in each of the Character's Skill Trees
    *   0x0A4   uint8[0x25] Unk_4                   = ???   [UNKNOWN, PLEASE INVESTIGATE]
    *   0x0C9   uint[35]    SkillLinkIDs            = The IDs of each Skill Link Skill on a Character's Skill Link Menu
    *   0x0EC   uint32      ExpertModeLevel         = Character's Level used for Expert Mode feature
    *   0x0F0   uint32      ExpertModeEXP           = Character's EXP used for Expert Mode feature
    *   0x0F4   uint32      ExpertModeReserveEXP    = Character's amount of Reserve EXP saved up for Expert Mode feature
    *   0x0F8   uint8[0x40] Unk_5                   = ???   [UNKNOWN, PLEASE INVESTIGATE]
    */
    public class PartyMember : ISaveObject
    {
        public const uint SIZE = 0x138;

        public uint Level { get; set; }
        public uint EXP { get; set; }
        public uint AP { get; set; }
        public uint AffinityCoins { get; set; }
        public byte[] Unk_1 { get; set; }
        public ItemID HeadEquip { get; set; }
        public ItemID TorsoEquip { get; set; }
        public ItemID ArmEquip { get; set; }
        public ItemID LegEquip { get; set; }
        public ItemID FootEquip { get; set; }
        public ItemID WeaponEquip { get; set; }
        public byte[] Unk_2 { get; set; }
        public uint HeadCosmetic { get; set; }
        public uint TorsoCosmetic { get; set; }
        public uint ArmCosmetic { get; set; }
        public uint LegCosmetic { get; set; }
        public uint FootCosmetic { get; set; }
        public uint WeaponCosmetic { get; set; }
        public ushort[] Arts { get; set; } = new ushort[9];
        public ushort[] MonadoArts { get; set; } = new ushort[9];
        public byte[] Unk_3 { get; set; }
        public uint SelectedSkillTreeIndex { get; set; }
        public uint[] SkillTreeSPs { get; set; } = new uint[5];
        public uint[] SkillTreeUnlockedSkills { get; set; } = new uint[5];
        public byte[] Unk_4 { get; set; }
        public byte[] SkillLinkIDs { get; set; } = new byte[35];
        public uint ExpertModeLevel { get; set; }
        public uint ExpertModeEXP { get; set; }
        public uint ExpertModeReserveEXP { get; set; }
        public byte[] Unk_5 { get; set; }

        private static readonly Dictionary<string, uint> LOC = new Dictionary<string, uint>()
        {
            { "Level", 0x00 },
            { "EXP", 0x04 },
            { "AP", 0x08 },
            { "AffinityCoins", 0x0C },
            { "Unk_1", 0x10 },
            { "HeadEquip", 0x14 },
            { "TorsoEquip", 0x18 },
            { "ArmEquip", 0x1C },
            { "LegEquip", 0x20 },
            { "FootEquip" , 0x24 },
            { "WeaponEquip", 0x28 },
            { "Unk_2", 0x2C },
            { "HeadCosmetic", 0x30 },
            { "TorsoCosmetic", 0x34 },
            { "ArmCosmetic", 0x38 },
            { "LegCosmetic", 0x3C },
            { "FootCosmetic", 0x40 },
            { "WeaponCosmetic", 0x44 },
            { "Arts", 0x48 },
            { "MonadoArts", 0x5A },
            { "Unk_3", 0x6C },
            { "SelectedSkillTreeIndex", 0x78 },
            { "SkillTreeSPs", 0x7C },
            { "SkillTreeUnlockedSkills", 0x90 },
            { "Unk_4", 0xA4 },
            { "SkillLinkIDs", 0xC9 },
            { "ExpertModeLevel", 0xEC },
            { "ExpertModeEXP", 0xF0 },
            { "ExpertModeReserveEXP", 0xF4 },
            { "Unk_5", 0xF8 }
        };

        public PartyMember(byte[] data)
        {
            Level = BitConverter.ToUInt32(data.GetByteSubArray(LOC["Level"], LOC["EXP"] - LOC["Level"]), 0);
            EXP = BitConverter.ToUInt32(data.GetByteSubArray(LOC["EXP"], LOC["AP"] - LOC["EXP"]), 0);
            AP = BitConverter.ToUInt32(data.GetByteSubArray(LOC["AP"], LOC["AffinityCoins"] - LOC["AP"]), 0);
            AffinityCoins = BitConverter.ToUInt32(data.GetByteSubArray(LOC["AffinityCoins"], LOC["Unk_1"] - LOC["AffinityCoins"]), 0);
            Unk_1 = data.GetByteSubArray(LOC["Unk_1"], LOC["HeadEquip"] - LOC["Unk_1"]);
            HeadEquip = new ItemID(data.GetByteSubArray(LOC["HeadEquip"], LOC["TorsoEquip"] - LOC["HeadEquip"]));
            TorsoEquip = new ItemID(data.GetByteSubArray(LOC["TorsoEquip"], LOC["ArmEquip"] - LOC["TorsoEquip"]));
            ArmEquip = new ItemID(data.GetByteSubArray(LOC["ArmEquip"], LOC["LegEquip"] - LOC["ArmEquip"]));
            LegEquip = new ItemID(data.GetByteSubArray(LOC["LegEquip"], LOC["FootEquip"] - LOC["LegEquip"]));
            FootEquip = new ItemID(data.GetByteSubArray(LOC["FootEquip"], LOC["WeaponEquip"] - LOC["FootEquip"]));
            WeaponEquip = new ItemID(data.GetByteSubArray(LOC["WeaponEquip"], LOC["Unk_2"] - LOC["WeaponEquip"]));
            Unk_2 = data.GetByteSubArray(LOC["Unk_2"], LOC["HeadCosmetic"] - LOC["Unk_2"]);
            HeadCosmetic = BitConverter.ToUInt32(data.GetByteSubArray(LOC["HeadCosmetic"], LOC["TorsoCosmetic"] - LOC["HeadCosmetic"]), 0);
            TorsoCosmetic = BitConverter.ToUInt32(data.GetByteSubArray(LOC["TorsoCosmetic"], LOC["ArmCosmetic"] - LOC["TorsoCosmetic"]), 0);
            ArmCosmetic = BitConverter.ToUInt32(data.GetByteSubArray(LOC["ArmCosmetic"], LOC["LegCosmetic"] - LOC["ArmCosmetic"]), 0);
            LegCosmetic = BitConverter.ToUInt32(data.GetByteSubArray(LOC["LegCosmetic"], LOC["FootCosmetic"] - LOC["LegCosmetic"]), 0);
            FootCosmetic = BitConverter.ToUInt32(data.GetByteSubArray(LOC["FootCosmetic"], LOC["WeaponCosmetic"] - LOC["FootCosmetic"]), 0);
            WeaponCosmetic = BitConverter.ToUInt32(data.GetByteSubArray(LOC["WeaponCosmetic"], LOC["Arts"] - LOC["WeaponCosmetic"]), 0);
            for (uint i = 0; i < Arts.Length; i++) Arts[i] = BitConverter.ToUInt16(data.GetByteSubArray(LOC["Arts"] + (i * sizeof(UInt16)), sizeof(UInt16)), 0);
            for (uint i = 0; i < MonadoArts.Length; i++) MonadoArts[i] = BitConverter.ToUInt16(data.GetByteSubArray(LOC["MonadoArts"] + (i * sizeof(UInt16)), sizeof(UInt16)), 0);
            Unk_3 = data.GetByteSubArray(LOC["Unk_3"], LOC["SelectedSkillTreeIndex"] - LOC["Unk_3"]);
            SelectedSkillTreeIndex = BitConverter.ToUInt32(data.GetByteSubArray(LOC["SelectedSkillTreeIndex"], LOC["SkillTreeSPs"] - LOC["SelectedSkillTreeIndex"]), 0);
            for (uint i = 0; i < SkillTreeSPs.Length; i++) SkillTreeSPs[i] = BitConverter.ToUInt32(data.GetByteSubArray(LOC["SkillTreeSPs"] + (i * sizeof(UInt32)), sizeof(UInt32)), 0);
            for (uint i = 0; i < SkillTreeUnlockedSkills.Length; i++) SkillTreeUnlockedSkills[i] = BitConverter.ToUInt32(data.GetByteSubArray(LOC["SkillTreeUnlockedSkills"] + (i * sizeof(UInt32)), sizeof(UInt32)), 0);
            Unk_4 = data.GetByteSubArray(LOC["Unk_4"], LOC["SkillLinkIDs"] - LOC["Unk_4"]);
            for (uint i = 0; i < SkillLinkIDs.Length; i++) SkillLinkIDs[i] = data[LOC["SkillLinkIDs"] + i];
            ExpertModeLevel = BitConverter.ToUInt32(data.GetByteSubArray(LOC["ExpertModeLevel"], LOC["ExpertModeEXP"] - LOC["ExpertModeLevel"]), 0);
            ExpertModeEXP = BitConverter.ToUInt32(data.GetByteSubArray(LOC["ExpertModeEXP"], LOC["ExpertModeReserveEXP"] - LOC["ExpertModeEXP"]), 0);
            ExpertModeReserveEXP = BitConverter.ToUInt32(data.GetByteSubArray(LOC["ExpertModeReserveEXP"], LOC["Unk_5"] - LOC["ExpertModeReserveEXP"]), 0);
            Unk_5 = data.GetByteSubArray(LOC["Unk_5"], SIZE - LOC["Unk_5"]);
        }

        public byte[] ToRawData()
        {
            List<byte> result = new List<byte>();

            result.AddRange(BitConverter.GetBytes(Level));
            result.AddRange(BitConverter.GetBytes(EXP));
            result.AddRange(BitConverter.GetBytes(AP));
            result.AddRange(BitConverter.GetBytes(AffinityCoins));
            result.AddRange(Unk_1);
            result.AddRange(HeadEquip.ToRawData());
            result.AddRange(TorsoEquip.ToRawData());
            result.AddRange(ArmEquip.ToRawData());
            result.AddRange(LegEquip.ToRawData());
            result.AddRange(FootEquip.ToRawData());
            result.AddRange(WeaponEquip.ToRawData());
            result.AddRange(Unk_2);
            result.AddRange(BitConverter.GetBytes(HeadCosmetic));
            result.AddRange(BitConverter.GetBytes(TorsoCosmetic));
            result.AddRange(BitConverter.GetBytes(ArmCosmetic));
            result.AddRange(BitConverter.GetBytes(LegCosmetic));
            result.AddRange(BitConverter.GetBytes(FootCosmetic));
            result.AddRange(BitConverter.GetBytes(WeaponCosmetic));
            foreach (UInt16 a in Arts) result.AddRange(BitConverter.GetBytes(a));
            foreach (UInt16 a in MonadoArts) result.AddRange(BitConverter.GetBytes(a));
            result.AddRange(Unk_3);
            result.AddRange(BitConverter.GetBytes(SelectedSkillTreeIndex));
            foreach (UInt32 sp in SkillTreeSPs) result.AddRange(BitConverter.GetBytes(sp));
            foreach (UInt32 su in SkillTreeUnlockedSkills) result.AddRange(BitConverter.GetBytes(su));
            result.AddRange(Unk_4);
            result.AddRange(SkillLinkIDs);
            result.AddRange(BitConverter.GetBytes(ExpertModeLevel));
            result.AddRange(BitConverter.GetBytes(ExpertModeEXP));
            result.AddRange(BitConverter.GetBytes(ExpertModeReserveEXP));
            result.AddRange(Unk_5);

            return result.ToArray();
        }
    }
}