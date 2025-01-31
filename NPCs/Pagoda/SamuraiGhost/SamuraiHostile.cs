using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using Terraria.GameContent.Bestiary;
using SpiritMod.Buffs.Tiles;

namespace SpiritMod.NPCs.Pagoda.SamuraiGhost
{
	public class SamuraiHostile : ModNPC
	{
		const int ChargeTime = 200; //how many frames between charges

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Samurai");
			Main.npcFrameCount[NPC.type] = 9;
			NPCID.Sets.TownCritter[NPC.type] = true;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
			NPCHelper.BuffImmune(Type);

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Position = new Vector2(-10, 16),
				Velocity = 1f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.width = 42;
			NPC.height = 70;
			NPC.damage = 33;
			NPC.noGravity = true;
			NPC.defense = 4;
			NPC.lifeMax = 90;
			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 120f;
			NPC.knockBackResist = .1f;
			NPC.noTileCollide = true;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.PhantomSamuraiBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("The spirit of a deadly warrior that once lived many moons ago. Punished for breaking his honor code, he now swears to defend what's left of his dynasty to seek redemption."),
			});
		}

		float frameCounter = 0;
		public override void FindFrame(int frameHeight)
		{
			frameCounter += 0.15f;
			frameCounter %= 3f;

			if (NPC.ai[2] == ChargeTime)
				frameCounter = 0;

			if (NPC.ai[3] == 1)
				NPC.frameCounter = frameCounter + 6;
			else if (NPC.ai[2] > ChargeTime - 50)
				NPC.frameCounter = frameCounter + 3;
			else
				NPC.frameCounter = frameCounter;

			if (NPC.frameCounter == 8)
				NPC.ai[3] = 0;

			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Wraith, 2.5f * hitDirection, -2.5f, 54, new Color(0, 255, 142), .6f);

			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);

				for (int i = 0; i < 40; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Wraith, 0f, -2f, 117, new Color(0, 255, 142), .6f);
					dust.noGravity = true;

					dust.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					dust.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					if (dust.position != NPC.Center)
						dust.velocity = NPC.DirectionTo(dust.position) * 6f;
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddFood(ModContent.ItemType<Ramen>(), 16);

		float chargeRotation = 0;
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];

			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.14f;
			NPC.scale = num395 + 0.95f;
			float velMax = 1f;
			float acceleration = 0.011f;
			Vector2 center = NPC.Center;
			float deltaX = player.position.X + (player.width / 2) - center.X;
			float deltaY = player.position.Y + (player.height / 2) - center.Y;
			float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

			NPC.ai[1]++;
			if (NPC.ai[3] == 0)
			{
				if (NPC.ai[1] > 600.0)
				{
					acceleration *= 8f;
					velMax = 4f;

					if (NPC.ai[1] > 650.0)
						NPC.ai[1] = 0f;
				}
				else if (distance < 250.0)
				{
					NPC.ai[0] += 0.9f;
					if (NPC.ai[0] > 0f)
						NPC.velocity.Y = NPC.velocity.Y + 0.019f;
					else
						NPC.velocity.Y = NPC.velocity.Y - 0.019f;

					if (NPC.ai[0] < -100f || NPC.ai[0] > 100f)
						NPC.velocity.X = NPC.velocity.X + 0.019f;
					else
						NPC.velocity.X = NPC.velocity.X - 0.019f;

					if (NPC.ai[0] > 200f)
					{
						NPC.ai[0] = -200f;
						NPC.netUpdate = true;
					}
				}
				if (distance > 350.0)
				{
					velMax = 5f;
					acceleration = 0.3f;
				}
				else if (distance > 300.0)
				{
					velMax = 3f;
					acceleration = 0.2f;
				}
				else if (distance > 250.0)
				{
					velMax = 1.5f;
					acceleration = 0.1f;
				}
				float stepRatio = velMax / distance;
				float velLimitX = deltaX * stepRatio;
				float velLimitY = deltaY * stepRatio;
				if (Main.player[NPC.target].dead)
				{
					velLimitX = (float)(NPC.direction * velMax / 2.0);
					velLimitY = (float)((-velMax) / 2.0);
				}

				if (NPC.velocity.X < velLimitX)
					NPC.velocity.X = NPC.velocity.X + acceleration;
				else if (NPC.velocity.X > velLimitX)
					NPC.velocity.X = NPC.velocity.X - acceleration;

				if (NPC.velocity.Y < velLimitY)
					NPC.velocity.Y = NPC.velocity.Y + acceleration;
				else if (NPC.velocity.Y > velLimitY)
					NPC.velocity.Y = NPC.velocity.Y - acceleration;

				if (velLimitX > 0.0)
					NPC.rotation = (float)Math.Atan2(velLimitY, velLimitX);
				if (velLimitX < 0.0)
					NPC.rotation = (float)Math.Atan2(velLimitY, velLimitX) + 3.14f;

				if (NPC.ai[2] < ChargeTime - 50)
					NPC.rotation = 0;
				else if (NPC.ai[2] > ChargeTime - 40)
					NPC.rotation = chargeRotation;

				if (NPC.ai[2] == ChargeTime - 40)
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/SamuraiUnsheathe"), NPC.Center);
				if (NPC.ai[2] > ChargeTime - 50 && NPC.ai[2] < ChargeTime - 40)
					chargeRotation = NPC.rotation;
			}
			NPC.ai[2]++;
			if (NPC.ai[2] == ChargeTime)
			{
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
					direction.Normalize();
					direction.X *= 18;
					direction.Y *= 9;
					NPC.velocity.X = direction.X;
					NPC.velocity.Y = direction.Y;
					NPC.ai[3] = 1;
					NPC.netUpdate = true;
				}
				SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);

				for (int i = 0; i < 20; i++) {
					int num = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Demonite, 0f, -2f, 0, default, .8f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if (Main.dust[num].position != NPC.Center)
						Main.dust[num].velocity = NPC.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			if (NPC.ai[2] >= ChargeTime + 20 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.ai[2] = 0;
				NPC.ai[3] = 0;
				NPC.netUpdate = true;
			}
			NPC.spriteDirection = NPC.direction;
		}
	}
}
