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
using System.Linq;

namespace XCDESave
{
    public static class ItemFiller
    {
        public const ushort MAX_QUANTITY = 99;

        // Main-game item IDs (row IDs of https://xenoblade.github.io/xb1de/bdat/bdat_common/ITM_itemlist.html),
        // minus placeholder rows (numeric names, "Stone" fillers, "Records" entries). Future Connected rows are left out.
        public static readonly (int From, int To)[] COLLECTABLES = { (1852, 2151) };
        public static readonly (int From, int To)[] MATERIALS = { (2152, 2587), (2634, 2634), (2646, 2646) };
        public static readonly (int From, int To)[] ARTS_MANUALS = { (2880, 2911), (2920, 3007), (3016, 3103) };

        // Turns box[slot] into a new item. Index = slot and SerialNo = box max + 1 match what real saves contain.
        public static void Put(Item[] box, int slot, ushort id, ItemType type, ushort quantity)
        {
            Item item = box[slot];
            item.Index = (ushort)slot;
            item.Type = type;
            item.FullID = new ItemID { ID = id, TypeID = type };
            item.SerialNo = box.Where(i => i.Exists).Select(i => i.SerialNo).DefaultIfEmpty(0u).Max() + 1;
            item.Favourite = false;
            item.Exists = true;
            item.Quantity = quantity;
        }

        // Puts every ID into the box at MAX_QUANTITY: tops up existing stacks, uses empty slots for the rest.
        // Returns (added, notAddedBecauseBoxFull).
        public static (int Added, int NoRoom) Fill(Item[] box, ItemType type, (int From, int To)[] ranges)
        {
            int added = 0, noRoom = 0;

            foreach (int id in ranges.SelectMany(r => Enumerable.Range(r.From, r.To - r.From + 1)))
            {
                Item item = Array.Find(box, i => i.Exists && i.FullID.ID == id);
                if (item != null)
                {
                    item.Quantity = Math.Max(item.Quantity, MAX_QUANTITY);
                    continue;
                }
                int slot = Array.FindIndex(box, i => !i.Exists);
                if (slot < 0) { noRoom++; continue; }
                Put(box, slot, (ushort)id, type, MAX_QUANTITY);
                added++;
            }
            return (added, noRoom);
        }
    }
}
