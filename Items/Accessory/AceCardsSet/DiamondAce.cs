using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using SpiritMod.Buffs;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.AceCardsSet
{
	[Sacrifice(0)]
	public class DiamondAce : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Ace");
			Tooltip.SetDefault("You shouldn't see this");
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 5));
			ItemID.Sets.AnimatesAsSoul[Type] = true;
			ItemID.Sets.IgnoresEncumberingStone[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
		}

		public override bool ItemSpace(Player player) => true;
		public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 100);

		public override bool OnPickup(Player player)
		{
			player.AddBuff(ModContent.BuffType<AceOfDiamondsBuff>(), 300);
			SoundEngine.PlaySound(SoundID.Grab, player.Center);
			return false;
		}
	}
}
