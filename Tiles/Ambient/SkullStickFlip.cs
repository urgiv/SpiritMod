using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class SkullStickFlip : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16 };
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Skull Stick");
			AddMapEntry(new Color(107, 90, 64), name);
			AdjTiles = new int[] { TileID.Lamps };
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 4;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Placeable.Furniture.SkullStick>());
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (!Main.dayTime) {
				r = .235f;
				g = .174f;
				b = .052f;
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (!Main.dayTime) {
				Tile tile = Main.tile[i, j];
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen) {
					zero = Vector2.Zero;
				}
				int height = tile.TileFrameY == 36 ? 18 : 16;
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Tiles/Ambient/SkullStick_Glow").Value, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(100, 100, 100, 100), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}