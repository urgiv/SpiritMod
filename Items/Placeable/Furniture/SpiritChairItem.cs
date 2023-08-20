using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	[Sacrifice(1)]
	public class SpiritChairItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Duskwood Chair");
		}


		public override void SetDefaults()
		{
			Item.width = 64;
			Item.height = 34;
			Item.value = 150;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<SpiritChair>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpiritWoodItem>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}