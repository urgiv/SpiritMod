using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Critters
{
	public class GoldenCarp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Carp");
			Main.npcFrameCount[NPC.type] = 5;
			Main.npcCatchable[NPC.type] = true;
			NPCID.Sets.CountsAsCritter[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 26;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.catchItem = (short)ItemID.GoldenCarp;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 16;
			NPC.rarity = 3;
			NPC.noGravity = true;
			NPC.dontCountMe = true;
			NPC.npcSlots = 0;
			AIType = NPCID.Goldfish;
			NPC.dontTakeDamageFromHostiles = false;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("A fish coated in absolutely gorgeous, shimmering scales that resemble pure gold. Their meat is no good for cooking, but may catch a high selling price."),
			});
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.GoldCoin, 0f, 0f, 100, default, 1.4f);
					Main.dust[num622].velocity *= 1f;
					Main.dust[num622].noGravity = true;
					{
						Main.dust[num622].scale = 0.23f;
					}
				}
			}
		}
		public override void AI()
		{
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), .1f, .1f, .1f);
			NPC.spriteDirection = NPC.direction;
			int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GoldCoin);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0f;
			Player target = Main.player[NPC.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
			if (distance < 65 && target.wet && NPC.wet)
			{
				Vector2 vel = NPC.DirectionFrom(target.Center);
				vel.Normalize();
				vel *= 4.5f;
				NPC.velocity = vel;
				NPC.rotation = NPC.velocity.X * .06f;
				if (target.position.X > NPC.position.X)
				{
					NPC.spriteDirection = -1;
					NPC.direction = -1;
					NPC.netUpdate = true;
				}
				else if (target.position.X < NPC.position.X)
				{
					NPC.spriteDirection = 1;
					NPC.direction = 1;
					NPC.netUpdate = true;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Water ? 0.007666f : 0f;
	}
}
