using Terraria.ID;
using Terraria.ModLoader;
using SnowNightBoxTile = SpiritMod.Tiles.MusicBox.SnowNightBox;

namespace SpiritMod.Items.Placeable.MusicBox
{
	[Sacrifice(1)]
	public class SnowNightBox : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Music Box (Snow Biome- Nighttime)");

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<SnowNightBoxTile>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
			Item.canBePlacedInVanityRegardlessOfConditions = true;
		}
	}
}
