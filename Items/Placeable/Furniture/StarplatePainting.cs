using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Placeable.Furniture
{
	[Sacrifice(1)]
	public class StarplatePainting : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Item.buyPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.White;
			Item.maxStack = Item.CommonMaxStack;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<StarplatePaintingTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Canvas>());
			recipe.AddIngredient(ModContent.ItemType<Items.BossLoot.StarplateDrops.CosmiliteShard>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}