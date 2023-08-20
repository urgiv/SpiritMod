using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class MetalBand : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Metal Band");
			// Tooltip.SetDefault("Increases pickup range for ores\n'No you can't listen to this'");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().metalBand = true;
	}
}
