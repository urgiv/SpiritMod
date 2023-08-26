using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.BriarDrops;

namespace SpiritMod.Items.Placeable.Furniture
{
	[Sacrifice(1)]
	public class ForagerTableItem : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 28;
			Item.value = Item.value = Item.sellPrice(0, 0, 3, 0);
			Item.rare = ItemRarityID.White;
			Item.maxStack = Item.CommonMaxStack;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<ForagerTableTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 20);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
            recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 6);
			recipe.Register();
		}
	}
}