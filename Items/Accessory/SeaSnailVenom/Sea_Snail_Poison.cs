using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SeaSnailVenom
{
	[AutoloadEquip(EquipType.Back)]
	public class Sea_Snail_Poison : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sea Snail Venom");
			Tooltip.SetDefault("You leave a trail of mucus that envenoms enemies");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.value = Item.sellPrice(gold: 1, silver: 20);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player) => player.GetSpiritPlayer().seaSnailVenom = true;
	}
}