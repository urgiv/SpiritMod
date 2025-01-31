using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SeraphSet.SeraphArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class SeraphArmor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Breastplate");
			Tooltip.SetDefault("10% increased magic damage");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_Glow");
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 24;
			Item.value = Item.buyPrice(gold: 6);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 18;
		}

		public override void UpdateEquip(Player player) => player.GetDamage(DamageClass.Magic) += .10f;
		
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) => glowMaskColor = Color.White;
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<MoonStone>(), 13);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
