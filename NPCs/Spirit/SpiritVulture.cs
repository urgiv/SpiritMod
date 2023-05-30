using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.SpiritBiomeDrops;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles.Block;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Spirit
{
	public class SpiritVulture : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Floater");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 32;
			NPC.damage = 36;
			NPC.defense = 19;
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 260f;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 14;
			NPC.noTileCollide = false;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.SpiritFloaterBanner>();
			AIType = NPCID.CaveBat;
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.SpiritUndergroundBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("Floating slabs of runic stone now given sentience by the Spirit. Despite seeming hostile, it only wishes for peace to lay and rest eternally once more."),
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
            if (!player.GetSpiritPlayer().ZoneSpirit)
                return 0f;

            if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.SpawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.SpawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				int[] TileArray2 = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<Spiritsand>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>(), };
				return TileArray2.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType) && NPC.downedMechBossAny && spawnInfo.SpawnTileY > Main.rockLayer && player.position.Y / 16 < (Main.rockLayer + Main.maxTilesY - 330) / 2f && !spawnInfo.PlayerSafe && !spawnInfo.Invasion ? 2f : 0f;
			}
			return 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * (frameHeight - 1);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 13);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 12);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 11);
				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Asphalt, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.NextBool(2)) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Asphalt, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Asphalt, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}

			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon(ModContent.ItemType<SoulWeaver>(), 25);

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			if (Main.rand.NextBool(150)) {
				Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 5f;

				int amountOfProjectiles = Main.rand.Next(1, 1);
				for (int i = 0; i < amountOfProjectiles; ++i) {
					float A = (float)Main.rand.Next(-1, 1) * 0.01f;
					float B = (float)Main.rand.Next(-1, 1) * 0.01f;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<SpiritRockBlast>(), 19, 1, Main.myPlayer, 0, 0);
				}
			}
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0f, 0.675f, 2.50f);
		}
	}
}
