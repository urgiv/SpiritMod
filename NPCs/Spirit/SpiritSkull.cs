using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Tiles.Block;
using SpiritMod.Utilities;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Spirit
{
	public class SpiritSkull : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Skull");
			Main.npcFrameCount[NPC.type] = 8;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 52;
			NPC.damage = 35;
			NPC.defense = 10;
			NPC.knockBackResist = 0.2f;
			NPC.lifeMax = 295;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.npcSlots = 0.75f;
			NPC.value = Item.buyPrice(0, 0, 10, 0);

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.SpiritSkullBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.SpiritUndergroundBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("They only look like skulls, but in actuality are once again stones carved many centuries ago. These stones seem to be carved in such a way for souls to be able to inhabit it, yet there's seemingly no record of the Spirit existing prior to its release."),
			});
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			var pos = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY);
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, pos, NPC.frame, NPC.GetNPCColorTintedByBuffs(drawColor), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
			=> GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Spirit/SpiritSkull_Glow").Value, screenPos);

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;

            if (player.ZoneSpirit() && spawnInfo.SpawnTileY > Main.rockLayer && !spawnInfo.PlayerSafe && !spawnInfo.Invasion)
			{
				int[] spawnTiles = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>(), };
				return spawnTiles.Contains(spawnInfo.SpawnTileType) ? 4f : 0f;
			}
			return 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			float velMax = 1f;
			float acceleration = 0.011f;
			NPC.TargetClosest(true);
			Vector2 center = NPC.Center;
			float deltaX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - center.X;
			float deltaY = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - center.Y;
			float distance = (float)Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
			NPC.ai[1] += 1f;
			if ((double)NPC.ai[1] > 600.0) {
				acceleration *= 8f;
				velMax = 4f;
				if ((double)NPC.ai[1] > 650.0) {
					NPC.ai[1] = 0f;
				}
			}
			else if ((double)distance < 250.0) {
				NPC.ai[0] += 0.9f;
				if (NPC.ai[0] > 0f) {
					NPC.velocity.Y = NPC.velocity.Y + 0.019f;
				}
				else {
					NPC.velocity.Y = NPC.velocity.Y - 0.019f;
				}
				if (NPC.ai[0] < -100f || NPC.ai[0] > 100f) {
					NPC.velocity.X = NPC.velocity.X + 0.019f;
				}
				else {
					NPC.velocity.X = NPC.velocity.X - 0.019f;
				}
				if (NPC.ai[0] > 200f) {
					NPC.ai[0] = -200f;
				}
			}
			if ((double)distance > 350.0) {
				velMax = 5f;
				acceleration = 0.3f;
			}
			else if ((double)distance > 300.0) {
				velMax = 3f;
				acceleration = 0.2f;
			}
			else if ((double)distance > 250.0) {
				velMax = 1.5f;
				acceleration = 0.1f;
			}
			float stepRatio = velMax / distance;
			float velLimitX = deltaX * stepRatio;
			float velLimitY = deltaY * stepRatio;
			if (Main.player[NPC.target].dead) {
				velLimitX = (float)((double)((float)NPC.direction * velMax) / 2.0);
				velLimitY = (float)((double)(-(double)velMax) / 2.0);
			}
			if (NPC.velocity.X < velLimitX) {
				NPC.velocity.X = NPC.velocity.X + acceleration;
			}
			else if (NPC.velocity.X > velLimitX) {
				NPC.velocity.X = NPC.velocity.X - acceleration;
			}
			if (NPC.velocity.Y < velLimitY) {
				NPC.velocity.Y = NPC.velocity.Y + acceleration;
			}
			else if (NPC.velocity.Y > velLimitY) {
				NPC.velocity.Y = NPC.velocity.Y - acceleration;
			}
			if ((double)velLimitX > 0.0) {
				NPC.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX);
			}
			if ((double)velLimitX < 0.0) {
				NPC.rotation = (float)Math.Atan2((double)velLimitY, (double)velLimitX) + 3.14f;
			}
			NPC.spriteDirection = -NPC.direction;
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.05f, 0.09f, 0.4f);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon(ModContent.ItemType<Items.Sets.SpiritBiomeDrops.SpiritFlameStaff>(), 90);

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 13);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 12);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 11);
			}
		}
	}
}
