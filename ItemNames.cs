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
using System.IO;
using System.Linq;

namespace XCDESave
{
    // items.tsv is ITM_itemlist from https://xenoblade.github.io/xb1de/bdat/bdat_common/ITM_itemlist.html: row ID (= the save's item ID), type, name.
    public static class ItemNames
    {
        // Gem effects on equipment are stored with this type ID (seen in real saves), not ItemType.Gem.
        public const ItemType GEM_EFFECT = ItemType.Unk_1;

        static readonly Dictionary<string, ItemType> BDAT_TYPES = new Dictionary<string, ItemType>
        {
            ["Weapon"] = ItemType.Weapon, ["Gem"] = ItemType.Gem, ["HeadArmor"] = ItemType.HeadArmour, ["BodyArmor"] = ItemType.TorsoArmour,
            ["ArmArmor"] = ItemType.ArmArmour, ["LegArmor"] = ItemType.LegArmour, ["FootArmor"] = ItemType.FootArmour, ["Crystal"] = ItemType.Crystal,
            ["Collectable"] = ItemType.Collectable, ["Material"] = ItemType.Material, ["KeyItem"] = ItemType.KeyItem, ["ArtBook"] = ItemType.ArtsManual
        };

        public static readonly (ushort Id, ItemType Type, string Name)[] ALL = Load();
        static readonly Dictionary<ushort, (ushort Id, ItemType Type, string Name)> BY_ID = ALL.ToDictionary(i => i.Id);

        static (ushort, ItemType, string)[] Load()
        {
            using var reader = new StreamReader(typeof(ItemNames).Assembly.GetManifestResourceStream("items.tsv"));
            return reader.ReadToEnd().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.TrimEnd('\r').Split('\t'))
                .Select(f => (ushort.Parse(f[0]), BDAT_TYPES[f[1]], f[2])).ToArray();
        }

        public static string Name(ushort id) => BY_ID.TryGetValue(id, out var i) ? i.Name : "?";
        public static ItemType TypeOf(ushort id) => BY_ID.TryGetValue(id, out var i) ? i.Type : ItemType.None;
    }
}
