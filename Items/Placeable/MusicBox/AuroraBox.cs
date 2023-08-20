using Terraria.ID;
using Terraria.ModLoader;
using AuroraBoxTile = SpiritMod.Tiles.MusicBox.AuroraBox;

namespace SpiritMod.Items.Placeable.MusicBox
{
	[Sacrifice(1)]
	public class AuroraBox : ModItem
	{
		// public override void SetStaticDefaults() => DisplayName.SetDefault("Music Box (Aurora)");

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<AuroraBoxTile>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
			Item.hasVanityEffects = true;
		}
	}
}
