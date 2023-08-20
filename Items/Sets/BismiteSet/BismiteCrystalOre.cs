using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismiteCrystalOre : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBrick[Type] = true;

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(30, 100, 25), name);
			HitSound = SoundID.Tink;
			DustType = DustID.Plantera_Green;
			Main.tileBlendAll[Type] = true;
		}
	}
}