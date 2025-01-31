using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.RlyehianDrops
{
	[AutoloadEquip(EquipType.Head)]
	public class RlyehMask : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("R'lyehian Mask");

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 3000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}
