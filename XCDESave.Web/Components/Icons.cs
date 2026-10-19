namespace XCDESave.Web.Components
{
    // shortcut: one emoji per item type stands in for real item thumbnails; swap in images once we have them.
    static class Icons
    {
        public static string For(ItemType type) => type switch
        {
            ItemType.Weapon => "⚔️",
            ItemType.HeadArmour => "🪖",
            ItemType.TorsoArmour => "🦺",
            ItemType.ArmArmour => "🧤",
            ItemType.LegArmour => "👖",
            ItemType.FootArmour => "👢",
            ItemType.Crystal => "💎",
            ItemType.Gem => "🔷",
            ItemType.Collectable => "🌿",
            ItemType.Material => "🦴",
            ItemType.KeyItem => "🗝️",
            ItemType.ArtsManual => "📘",
            _ => "❔"
        };
    }
}
