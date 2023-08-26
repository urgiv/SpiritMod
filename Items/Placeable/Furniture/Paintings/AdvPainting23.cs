using SpiritMod.Tiles.Furniture.Paintings;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture.Paintings
{
	[Sacrifice(1)]
	public class AdvPainting23 : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 23;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.White;
			Item.maxStack = Item.CommonMaxStack;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<AdvPainting23Tile>();
		}
	}
}