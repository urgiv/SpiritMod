using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Accessory
{
	public class SpectreRing : AccessoryItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Ring");
			Tooltip.SetDefault("Unleashes a bolt of Spectre Energy when the wearer is hurt");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpectreBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
