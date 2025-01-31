using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon.Snapspore;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class SnapsporeStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snapspore Wand");
			Tooltip.SetDefault("Summons a Snapspore to fight for you\nSnapspores bounce toward enemies and emits poison clouds");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.Green;
			Item.mana = 10;
			Item.damage = 17;
			Item.knockBack = 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<SnapsporeMinion>();
			Item.UseSound = SoundID.Grass;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => player.altFunctionUse != 2;

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
				player.MinionNPCTargetAim(true);
			return null;
		}
	}
}