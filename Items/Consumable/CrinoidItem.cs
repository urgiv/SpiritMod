using SpiritMod.NPCs.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class CrinoidItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Crinoid");

		public override void SetDefaults()
		{
			Item.width = Item.height = 20;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 0, 40);
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 20;
            Item.bait = 25;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = true;
			Item.makeNPC = ModContent.NPCType<Crinoid>();
		}
	}
}
