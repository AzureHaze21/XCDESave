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

namespace XCDESave.Editor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            void Use(Type converter, params Type[] types)
            {
                foreach (Type t in types) TypeDescriptor.AddAttributes(t, new TypeConverterAttribute(converter));
            }
            Use(typeof(KnownFieldsConverter), typeof(XCDESaveData), typeof(Party), typeof(ArtsLevel));
            Use(typeof(ItemConverter), typeof(Item), typeof(EquipItem), typeof(CrystalItem));
            Use(typeof(PartyMemberConverter), typeof(PartyMember));
            Use(typeof(ItemIDConverter), typeof(ItemID));

            ApplicationConfiguration.Initialize();
            Application.Run(new EditorForm());
        }
    }

    // Makes save objects expandable in the grid and hides Unk_ fields: they are unidentified bytes (one is 1.1 MB).
    class KnownFieldsConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) =>
            new PropertyDescriptorCollection(base.GetProperties(context, value, attributes).Cast<PropertyDescriptor>().Where(p => !p.Name.StartsWith("Unk_")).ToArray());
    }

    class EditorForm : Form
    {
        const string FILTER = "XCDE save (bfsgame*.sav)|bfsgame*.sav|All files (*.*)|*.*";
        readonly PropertyGrid grid = new PropertyGrid { Dock = DockStyle.Fill };
        internal static XCDESaveData save; // static so the grid's type converters can resolve equipment slots
        string path;

        public EditorForm()
        {
            Text = "XCDE Save Editor";
            Size = new Size(800, 700);

            var file = new ToolStripMenuItem("&File");
            file.DropDownItems.Add(new ToolStripMenuItem("&Open...", null, (s, e) => Open(), Keys.Control | Keys.O));
            file.DropDownItems.Add(new ToolStripMenuItem("&Save As...", null, (s, e) => SaveAs(), Keys.Control | Keys.S));
            var tools = new ToolStripMenuItem("&Tools");
            tools.DropDownItems.Add(new ToolStripMenuItem("Fill &Collectables", null, (s, e) => Fill(save?.Collectables, ItemType.Collectable, ItemFiller.COLLECTABLES)));
            tools.DropDownItems.Add(new ToolStripMenuItem("Fill &Materials", null, (s, e) => Fill(save?.Materials, ItemType.Material, ItemFiller.MATERIALS)));
            tools.DropDownItems.Add(new ToolStripMenuItem("Fill &Arts Manuals", null, (s, e) => Fill(save?.ArtsManuals, ItemType.ArtsManual, ItemFiller.ARTS_MANUALS)));
            var menu = new MenuStrip();
            menu.Items.Add(file);
            menu.Items.Add(tools);

            Controls.Add(grid);
            Controls.Add(menu);
            MainMenuStrip = menu;
        }

        void Open()
        {
            using var dlg = new OpenFileDialog { Filter = FILTER };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                byte[] data = File.ReadAllBytes(dlg.FileName);
                XCDESaveData loaded = XCDESaveSerialization.Deserialize(data);
                // The library must write this file back unchanged, otherwise saving would silently corrupt it.
                if (!XCDESaveSerialization.Serialize(loaded).SequenceEqual(data))
                    throw new InvalidDataException("This save does not survive a load/save round-trip through XCDESave unchanged, so editing it is not safe.");
                save = loaded;
                path = dlg.FileName;
                grid.SelectedObject = save;
                Text = $"XCDE Save Editor - {path}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Could not open save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Fill(Item[] box, ItemType type, (int From, int To)[] ranges)
        {
            if (box == null) return;
            var (added, noRoom) = ItemFiller.Fill(box, type, ranges);
            grid.Refresh();
            MessageBox.Show(this, $"Added {added} new {type} item(s); all stacks set to at least {ItemFiller.MAX_QUANTITY}." +
                (noRoom > 0 ? $"\n{noRoom} item(s) did not fit: the box is full." : "") + "\n\nUse Save As to write the file.", "Fill " + type);
        }

        void SaveAs()
        {
            if (save == null) return;
            using var dlg = new SaveFileDialog { Filter = FILTER, InitialDirectory = Path.GetDirectoryName(path), FileName = Path.GetFileName(path) };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                byte[] data = XCDESaveSerialization.Serialize(save);
                // The grid's array editor can resize arrays; a save of the wrong size would not load in game.
                if (data.Length != XCDESaveData.SIZE)
                    throw new InvalidDataException($"Save data is 0x{data.Length:X} bytes, expected 0x{XCDESaveData.SIZE:X}. Was an array resized? Nothing was written.");
                File.WriteAllBytes(dlg.FileName, data);
                path = dlg.FileName;
                Text = $"XCDE Save Editor - {path}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Could not save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
