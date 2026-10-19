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

using System.ComponentModel;
using System.Globalization;

namespace XCDESave.Editor
{
    // Collapsed inventory rows read "1173 Night Goggles x1" instead of raw fields.
    class ItemConverter : KnownFieldsConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
            destinationType == typeof(string) && value is Item i
                ? (i.Exists ? $"{i.FullID.ID} {ItemNames.Name(i.FullID.ID)} x{i.Quantity}" : "(empty)")
                : base.ConvertTo(context, culture, value, destinationType);
    }

    // Collapsed party member rows read "Shulk Lv10"; PartyMembers[i] belongs to Character i+1.
    class PartyMemberConverter : KnownFieldsConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
            destinationType == typeof(string) && value is PartyMember p && EditorForm.save != null
                ? $"{(Character)(Array.IndexOf(EditorForm.save.PartyMembers, p) + 1)} Lv{p.Level}"
                : base.ConvertTo(context, culture, value, destinationType);
    }

    // ItemIDs show names and get a dropdown. Two kinds exist:
    // - a PartyMember's *Equip fields hold a SLOT in an inventory box: shown as "3: Night Goggles", dropdown lists that box.
    // - everything else holds an item ID: shown as "1173 Night Goggles", dropdown lists items of the matching type.
    // Typing just the number also works.
    class ItemIDConverter : KnownFieldsConverter
    {
        static bool IsEquipSlot(ITypeDescriptorContext c) => c?.Instance is PartyMember;
        static bool IsGem(ITypeDescriptorContext c) => c?.PropertyDescriptor?.Name.StartsWith("Gem") == true;
        static ItemID Current(ITypeDescriptorContext c) => (ItemID)c.PropertyDescriptor.GetValue(c.Instance);

        static Item[] Box(ItemType type)
        {
            XCDESaveData s = EditorForm.save;
            return type switch
            {
                ItemType.Weapon => s?.Weapons, ItemType.HeadArmour => s?.HeadArmour, ItemType.TorsoArmour => s?.TorsoArmour,
                ItemType.ArmArmour => s?.ArmArmour, ItemType.LegArmour => s?.LegArmour, ItemType.FootArmour => s?.FootArmour,
                _ => null
            };
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string) || !(value is ItemID id)) return base.ConvertTo(context, culture, value, destinationType);
            if (IsEquipSlot(context))
            {
                Item item = Box(id.TypeID)?.ElementAtOrDefault(id.ID);
                return $"{id.ID}: {(item != null && item.Exists ? ItemNames.Name(item.FullID.ID) : "?")}";
            }
            return id.ID == 0 ? "0 (none)" : $"{id.ID} {ItemNames.Name(id.ID)}";
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string s)) return base.ConvertFrom(context, culture, value);
            ushort n = ushort.Parse(new string(s.TrimStart().TakeWhile(char.IsDigit).ToArray()));
            ItemType type = IsEquipSlot(context) ? Current(context).TypeID
                : n == 0 ? ItemType.None
                : IsGem(context) ? ItemNames.GEM_EFFECT
                : ItemNames.TypeOf(n);
            return new ItemID { ID = n, TypeID = type };
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => context?.PropertyDescriptor != null;

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ItemID current = Current(context);
            if (IsEquipSlot(context))
                return new StandardValuesCollection((Box(current.TypeID) ?? new Item[0]).Where(i => i.Exists)
                    .Select(i => new ItemID { ID = i.Index, TypeID = current.TypeID }).ToArray());

            ItemType want = IsGem(context) ? ItemType.Gem
                : context.Instance is Item item && item.Type != ItemType.None ? item.Type
                : current.TypeID;
            return new StandardValuesCollection(ItemNames.ALL.Where(i => want == ItemType.None || i.Type == want)
                .Select(i => new ItemID { ID = i.Id, TypeID = IsGem(context) ? ItemNames.GEM_EFFECT : i.Type }).ToArray());
        }
    }
}
