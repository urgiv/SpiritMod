using SpiritMod.NPCs.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class CyberflyPinkItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink Cyberfly");
			Tooltip.SetDefault("'Real or digital?'");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 32;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.useTime = Item.useAnimation = 20;
			Item.bait = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = true;
			Item.makeNPC = ModContent.NPCType<CyberflyPink>();
		}
	}
}
