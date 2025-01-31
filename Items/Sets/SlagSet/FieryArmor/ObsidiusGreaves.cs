using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SlagSet.FieryArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ObsidiusGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Tyrant's Greaves");
			Tooltip.SetDefault("5% increased minion damage\nIncreases your max number of minions");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 39, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 6;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
			=> Lighting.AddLight(Item.position, 0.4f, .12f, .028f);

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.maxMinions += 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
