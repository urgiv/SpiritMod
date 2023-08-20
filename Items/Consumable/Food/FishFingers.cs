using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable.Food
{
	[Sacrifice(5)]
	public class FishFingers : FoodItem
	{
		internal override Point Size => new(36, 26);
		// public override void StaticDefaults() => Tooltip.SetDefault("'Goes great with custard!'");
	}
}
