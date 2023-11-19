using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.ByBiome.Forest.Consumeable;

[Sacrifice(5)]
public class EnchantedApple : FoodItem
{
	internal override Point Size => new(20, 22);

	public override bool CanUseItem(Player player)
	{
		player.AddBuff(BuffID.ManaRegeneration, 3600);
		return true;
	}
}
