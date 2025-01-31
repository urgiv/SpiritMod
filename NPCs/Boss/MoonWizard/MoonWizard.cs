﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.BossLoot.MoonWizardDrops;
using SpiritMod.Items.BossLoot.MoonWizardDrops.JellynautHelmet;
using System.IO;
using SpiritMod.Items.Consumable;
using SpiritMod.Utilities;
using System.Collections.Generic;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Buffs.DoT;
using Terraria.GameContent.Bestiary;
using SpiritMod.Items.BossLoot.MoonWizardDrops.MJWPet;
using Terraria.GameContent.ItemDropRules;
using SpiritMod.Biomes.Events;
using SpiritMod.Items.Placeable.Relics;
using ReLogic.Content;

namespace SpiritMod.NPCs.Boss.MoonWizard
{
	[AutoloadBossHead]
	public class MoonWizard : ModNPC, IBCRegistrable
	{
		private static Asset<Texture2D> _glowTex;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Jelly Wizard");
			Main.npcFrameCount[NPC.type] = 21;
			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
			NPCHelper.ImmuneTo<FesteringWounds>(this, BuffID.Confused, BuffID.Poisoned, BuffID.Venom);

			var drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Position = new Vector2(4f, 34f),
				PortraitPositionYOverride = 20f,
				PortraitPositionXOverride = 0f,
				Direction = 1,
				Velocity = 1f,
				Scale = 2f,
				PortraitScale = 2f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);

			_glowTex = Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/MoonWizard_Glow");
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 2000;
			NPC.defense = 10;
			NPC.value = 40000;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0f;
			NPC.width = 17;
			NPC.height = 35;
			NPC.damage = 30;
			NPC.scale = 2f;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit13;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.boss = true;

			SpawnModBiomes = new int[1] { ModContent.GetInstance<JellyDelugeBiome>().Type };
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/MoonJelly");
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("Those moon jellies are smarter than they seem. With only a bit of coordination and a sharp sense of fashion, they can become a far tougher adversary than any one jelly could ever be."),
			});
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * (Main.masterMode ? 0.85f : 1.0f) * 0.8f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * (Main.masterMode ? 0.85f : 1.0f) * 0.8f);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(attackCounter);
			writer.Write(timeBetweenAttacks);
			writer.Write(phaseTwo);
			writer.Write(jellyRapidFire);
			writer.WriteVector2(dashDirection);
			writer.Write(dashDistance);
			writer.WriteVector2(TeleportPos);
			writer.Write(numMoves);
			writer.Write(node);
			writer.Write(SkyPos);
			writer.Write(SkyPosY);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			attackCounter = reader.ReadInt32();
			timeBetweenAttacks = reader.ReadInt32();
			phaseTwo = reader.ReadBoolean();
			jellyRapidFire = reader.ReadBoolean();
			dashDirection = reader.ReadVector2();
			dashDistance = reader.ReadSingle();
			TeleportPos = reader.ReadVector2();
			numMoves = reader.ReadInt32();
			node = reader.ReadInt32();
			SkyPos = reader.ReadInt32();
			SkyPosY = reader.ReadInt32();
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, Vector2 screenPos, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, screenPos, opacity, startScale, endScale);

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, Vector2 screenPos, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizard/MoonWizard_Afterimage").Value, NPC.Center + offset - Main.screenPosition + new Vector2(0, NPC.gfxOffY) - NPC.velocity * (float)i * trailLengthModifier, NPC.frame, color, NPC.rotation, NPC.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (jellyRapidFire)
			{
				Vector2 position1 = NPC.Bottom - screenPos;
				Texture2D glow = TextureAssets.GlowMask[239].Value;
				float num11 = (float)(Main.GlobalTimeWrappedHourly % 1.0 / 1.0);
				float num12 = num11;
				if ((double)num12 > 0.5)
					num12 = 1f - num11;
				if ((double)num12 < 0.0)
					num12 = 0.0f;
				float num13 = (float)(((double)num11 + 0.5) % 1.0);
				float num14 = num13;
				if ((double)num14 > 0.5)
					num14 = 1f - num13;
				if ((double)num14 < 0.0)
					num14 = 0.0f;
				Rectangle r2 = glow.Frame(1, 1, 0, 0);
				Vector2 drawOrigin = r2.Size() / 2f;
				Vector2 position3 = position1 + new Vector2(0.0f, -46f);
				Color color3 = new Color(87, 238, 255) * .8f;
				Main.spriteBatch.Draw(glow, position3, r2, color3, NPC.rotation, drawOrigin, NPC.scale * .6f, SpriteEffects.FlipHorizontally, 0.0f);
				float num15 = 1f + num11 * 0.75f;
				Main.spriteBatch.Draw(glow, position3, r2, color3 * num12, NPC.rotation, drawOrigin, NPC.scale * .6f * num15, SpriteEffects.FlipHorizontally, 0.0f);
				float num16 = 1f + num13 * 0.75f;
				Main.spriteBatch.Draw(glow, position3, r2, color3 * num14, NPC.rotation, drawOrigin, NPC.scale * .6f * num16, SpriteEffects.FlipHorizontally, 0.0f);
			}

			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			float num366 = num395 + 2.45f;

			if (NPC.velocity != Vector2.Zero || phaseTwo)
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, -18f), 0.5f, Color.White * .7f, Color.White * .1f, screenPos, 0.75f, num366, 1.65f);
			
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, new Vector2(NPC.Center.X, NPC.Center.Y - 18) - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, NPC.GetNPCColorTintedByBuffs(drawColor), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			DrawSpecialGlow(screenPos);
		}

		public void DrawSpecialGlow(Vector2 screenPos)
		{
			float sine = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * MathHelper.TwoPi)) / 2f + 0.5f;
			SpriteEffects effect = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 basePos = new Vector2(NPC.Center.X, NPC.Center.Y - 18) - screenPos + new Vector2(0, NPC.gfxOffY) - NPC.velocity;
			Color col = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.LightBlue);

			for (int i = 0; i < 4; i++)
			{
				Color fade = NPC.GetAlpha(col) * (1f - sine);
				var rot = ((i / 4f * MathHelper.TwoPi) + NPC.rotation).ToRotationVector2();
				Vector2 vector29 = basePos + (rot * sine * 7);
				Main.spriteBatch.Draw(_glowTex.Value, vector29, NPC.frame, fade, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, effect, 0f);
			}

			Main.spriteBatch.Draw(_glowTex.Value, basePos, NPC.frame, col, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, effect, 0f);
		}

		int attackCounter;
		float trueFrame = 0;
		int timeBetweenAttacks = 120;
		bool phaseTwo = false;

		//0-3: idle
		//4-9 propelling
		//10-13 skirt up
		//14-21: turning
		//22-28: kick
		public override void AI()
		{
			Lighting.AddLight(NPC.Center, 0.075f * 2, 0.231f * 2, 0.255f * 2);
			NPC.TargetClosest();
			if (NPC.ai[0] == 0)
			{
				attackCounter++;
				NPC.velocity = Vector2.Zero;
				NPC.rotation = 0;

				if (NPC.life > 250)
					UpdateFrame(0.15f, 0, 3);
				else
					UpdateFrame(0.15f, 54, 61);

				if (!phaseTwo && NPC.life < NPC.lifeMax / 3)
				{
					phaseTwo = true;
					NPC.ai[0] = 9;
				}

				if (attackCounter > timeBetweenAttacks)
				{
					attackCounter = 0;

					if (phaseTwo)
						NPC.ai[0] = Main.rand.Next(9) + 1;
					else
						NPC.ai[0] = Main.rand.Next(7) + 1;

					NPC.netUpdate = true;

					if (NPC.ai[0] == 7 && Main.player[NPC.target].velocity.Y > 0)
						NPC.ai[0] = 8;

					Player player = Main.player[NPC.target];
					if (!player.active || player.dead || Main.dayTime)
					{
						NPC.TargetClosest(false);
						NPC.velocity.Y = -200;
						NPC.active = false;
					}
					Vector2 dist = player.Center - NPC.Center;
					if (dist.Length() > 1000)
					{
						switch (Main.rand.Next(2))
						{
							case 0:
								NPC.ai[0] = 5;
								break;
							case 1:
								NPC.ai[0] = 2;
								break;
						}

						NPC.netUpdate = true;
					}

				}
			}
			else
			{
				switch (NPC.ai[0])
				{
					case 1: //teleport dash
						TeleportDash();
						break;
					case 2:
						Dash();
						break;
					case 3:
						SmashAttack();
						break;
					case 4:
						TeleportMove();
						break;
					case 5:
						SineAttack();
						break;
					case 6:
						KickAttack();
						break;
					case 7:
						SkyStrikeLeft();
						break;
					case 8:
						SkyStrikeRight();
						break;
					case 9:
						JellyfishRapidFire();
						break;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.IsABestiaryIconDummy)
				trueFrame = (trueFrame + .15f) % 4;

			NPC.frame.Width = 60;
			NPC.frame.X = (int)trueFrame % 3 * NPC.frame.Width;
			NPC.frame.Y = ((int)trueFrame - ((int)trueFrame % 3)) / 3 * NPC.frame.Height;
		}

		public void UpdateFrame(float speed, int minFrame, int maxFrame)
		{
			trueFrame += speed;
			if (trueFrame < minFrame)
				trueFrame = minFrame;
			if (trueFrame > maxFrame)
				trueFrame = minFrame;
		}
		public override void BossLoot(ref string name, ref int potionType) => potionType = ModContent.ItemType<MoonJelly>();

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddMasterModeRelicAndPet<MJWRelicItem, MJWPetItem>();
			npcLoot.AddBossBag<MJWBag>();

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.AddCommon<MJWMask>(7);
			notExpertRule.AddCommon<MJWTrophy>(10);
			notExpertRule.AddOneFromOptions<Moonshot, NautilusClub, JellynautBubble, MoonjellySummonStaff>(1);
			notExpertRule.AddCommon<TinyLunazoaItem>(1, 30, 40);

			npcLoot.Add(notExpertRule);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default, 0.27f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Granite, 2.5f * hitDirection, -2.5f, 0, default, 0.87f);
			}
			if (NPC.life < 100)
			{
				SoundEngine.PlaySound(SoundID.NPCHit27, NPC.Center);
				for (int i = 0; i < Main.rand.Next(1, 3); i++)
				{
					int p = Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(2.2f)), ModContent.ProjectileType<MoonBubble>(), 0, 3);
					Main.projectile[p].scale = Main.rand.NextFloat(.2f, .4f);
				}
			}
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				Gore.NewGore(NPC.GetSource_OnHit(NPC), new Vector2(NPC.Center.X, NPC.Center.Y - 50), new Vector2(0, 3), Mod.Find<ModGore>("WizardHat_Gore").Type);
				for (int i = 0; i < Main.rand.Next(9, 15); i++)
					Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(2.2f)), ModContent.ProjectileType<MoonBubble>(), 0, 3);
			}
		}

		Vector2 dashDirection = Vector2.Zero;
		float dashDistance = 0f;
		int numMoves = -1;
		int node = 0;
		#region attacks
		bool jellyRapidFire = false;
		void JellyfishRapidFire()
		{
			jellyRapidFire = true;
			attackCounter++;
			if (attackCounter == 1)
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/BossSFX/MoonWizard_Laugh2"), NPC.Center);

			if (attackCounter % 20 == 0 && attackCounter < 180 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 vel = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
				Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.Next(-60, 60), Main.rand.Next(-60, 60)), vel, ModContent.ProjectileType<JellyfishOrbiter>(), NPCUtils.ToActualDamage(40, 1.5f, 2f), 0.0f, Main.myPlayer, 0.0f, (float)NPC.whoAmI);
				proj.scale = Main.rand.NextFloat(.6f, 1f);
			}

			UpdateFrame(0.15f, 54, 61);
			if (attackCounter > 220)
			{
				jellyRapidFire = false;
				NPC.ai[0] = 1;

				timeBetweenAttacks = 60;
				attackCounter = 0;
			}
		}

		int SkyPos = 0;
		int SkyPosY = 0;

		void SkyStrikeLeft()
		{
			if (NPC.life > 250)
				UpdateFrame(0.15f, 1, 3);
			else
				UpdateFrame(0.15f, 54, 61);

			Player player = Main.player[NPC.target];

			if (attackCounter == 0)
			{
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/BossSFX/MoonWizard_Laugh1"), NPC.Center);
				SkyPos = (int)(player.position.X - 1000);
				SkyPosY = (int)(player.position.Y - 500);
			}
			if (attackCounter % 4 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 strikePos = new Vector2(SkyPos, SkyPosY);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), strikePos, new Vector2(0, 0), ModContent.ProjectileType<SkyMoonZapper>(), NPCUtils.ToActualDamage(20, 1.5f, 2f), 0, NPC.target);
				SkyPos += 170;
			}
			if (attackCounter == 60)
			{
				NPC.ai[0] = 1;

				timeBetweenAttacks = 0;
				attackCounter = 0;
			}
			attackCounter++;
		}

		void SkyStrikeRight()
		{
			if (NPC.life > 250)
				UpdateFrame(0.15f, 1, 3);
			else
				UpdateFrame(0.15f, 54, 61);

			Player player = Main.player[NPC.target];
			if (attackCounter == 0)
			{
				SkyPos = (int)(player.position.X + 1000);
				SkyPosY = (int)(player.position.Y - 500);
			}
			if (attackCounter % 4 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 strikePos = new Vector2(SkyPos, SkyPosY);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), strikePos, new Vector2(0, 0), ModContent.ProjectileType<SkyMoonZapper>(), NPCUtils.ToActualDamage(20, 1.5f, 2f), 0, NPC.target);
				SkyPos -= 170;
			}
			if (attackCounter == 60)
			{
				NPC.ai[0] = 1;

				timeBetweenAttacks = 0;
				attackCounter = 0;
			}
			attackCounter++;
		}

		void SineAttack()
		{
			Player player = Main.player[NPC.target];
			UpdateFrame(0.15f, 10, 13);
			if (attackCounter == 10)
			{
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/BossSFX/MoonWizard_Laugh1"), NPC.Center);
			}
			attackCounter++;
			if (attackCounter % 30 == 0)
			{
				SoundEngine.PlaySound(SoundID.Item15, NPC.Center);
			}
			if (attackCounter >= 30)
			{
				if (attackCounter % 80 == 40)
				{
					Vector2 direction = player.Center - (NPC.Center - new Vector2(0, 60));
					direction.Normalize();
					direction *= 5;

					int projIndex = 0;
					for (int i = 0; i < 2; i++)
					{
						Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center - new Vector2(0, 60), direction, ModContent.ProjectileType<SineBall>(), NPCUtils.ToActualDamage(30, 1.5f, 2f), 3, NPC.target);
						if (i > 0)
						{
							proj.ai[1] = projIndex + 1;
						}
						else
						{
							proj.ai[0] = 180;
							projIndex = proj.whoAmI;
						}
						proj.hostile = true;
						proj.friendly = false;
					}
				}
				if (attackCounter > 260)
				{
					NPC.ai[0] = 1;

					timeBetweenAttacks = 30;
					attackCounter = 0;
				}
			}
		}

		void SmashAttack()
		{
			Player player = Main.player[NPC.target];
			NPC.rotation = 3.14f;
			if (attackCounter < 30 && attackCounter > 10)
			{
				NPC.position.X = player.position.X;
				NPC.position.Y = player.position.Y - 300;
			}

			attackCounter++;

			if (attackCounter < 30)
				UpdateFrame(0.2f, 4, 9);
			else
			{
				UpdateFrame(0.4f, 4, 9);
				if (attackCounter == 55)
				{
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/BossSFX/MoonWizard_Attack"), NPC.Center);
					NPC.velocity.Y = 13;
					NPC.velocity.Y += 1.6f;
					if (phaseTwo)
					{
						NPC.velocity.Y = 20;
						attackCounter++;
					}

					SoundEngine.PlaySound(SoundID.Item81, NPC.Center);
				}
			}
			if ((Main.tile[(int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16)].BlockType == BlockType.Solid || attackCounter > 75) && Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < Main.rand.Next(9, 15); i++)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(3.2f)), ModContent.ProjectileType<MoonBubble>(), NPCUtils.ToActualDamage(40, 1.5f, 2f), 3);
				}
				Teleport();
				NPC.ai[0] = 0;

				timeBetweenAttacks = 20;
				attackCounter = 0;
			}
		}

		void TeleportDash()
		{
			Player player = Main.player[NPC.target];
			UpdateFrame(0.15f, 4, 9);
			float speed = 16f;
			if (phaseTwo)
			{
				speed = 21;
			}
			if (attackCounter == 20)
			{
				dashDirection = player.Center - NPC.Center;
				dashDistance = dashDirection.Length();
				dashDirection.Normalize();
				dashDirection *= speed;
				NPC.velocity = dashDirection;

				if (Main.netMode != NetmodeID.MultiplayerClient)
					node = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position, Vector2.Zero, ModContent.ProjectileType<LightningNode>(), NPCUtils.ToActualDamage(12, 1.5f, 2f), 0, Main.myPlayer, NPC.whoAmI + 1);
				NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
				SoundEngine.PlaySound(SoundID.Item81, NPC.Center);
			}
			if (attackCounter < 20)
			{
				dashDirection = player.Center - NPC.Center;
				NPC.rotation = dashDirection.ToRotation() + 1.57f;
			}
			attackCounter++;
			if (attackCounter > Math.Abs(dashDistance / speed) + 40)
			{
				Main.projectile[node].Kill();
				Teleport();
				NPC.ai[0] = 0;

				timeBetweenAttacks = 35;
				if (phaseTwo)
				{
					timeBetweenAttacks = 34;
				}
				attackCounter = 0;
			}
		}

		/*void CreateNodes()
		{
			UpdateFrame(0.15f, 10, 13);
			Player player = Main.player[NPC.target];
			if (attackCounter == 30 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < 4; i++)
				{
					NPC.ai[3] = Main.rand.Next(360);
					double anglex = Math.Sin(NPC.ai[3] * (Math.PI / 180));
					double angley = Math.Abs(Math.Cos(NPC.ai[3] * (Math.PI / 180)));
					Vector2 angle = new Vector2((float)anglex, (float)angley);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position - (angle * Main.rand.Next(100, 400)), Vector2.Zero, ModContent.ProjectileType<MoonBlocker>(), NPC.damage / 2, 0, player.whoAmI);
					NPC.netUpdate = true;
				}
			}
			attackCounter++;
			if (attackCounter > 55)
			{
				NPC.ai[0] = 0;

				timeBetweenAttacks = 8;
				attackCounter = 0;
			}
		}*/

		void Dash()
		{
			Player player = Main.player[NPC.target];
			UpdateFrame(0.15f, 4, 9);
			float speed = 13f;
			if (attackCounter == 0)
			{
				SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, NPC.position);
				int distance = Main.rand.Next(300, 500);
				NPC.ai[3] = Main.rand.Next(360);
				double anglex = Math.Sin(NPC.ai[3] * (Math.PI / 180));
				double angley = Math.Abs(Math.Cos(NPC.ai[3] * (Math.PI / 180)));
				Vector2 angle = new Vector2((float)anglex, (float)angley);
				dashDirection = (player.Center - (angle * distance)) - NPC.Center;
				dashDistance = dashDirection.Length();
				dashDirection.Normalize();
				dashDirection *= speed;
				NPC.velocity = dashDirection;

				if (Main.netMode != NetmodeID.MultiplayerClient)
					node = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position, Vector2.Zero, ModContent.ProjectileType<LightningNode>(), NPCUtils.ToActualDamage(12, 1.5f, 2f), 0, Main.myPlayer, NPC.whoAmI + 1);

				NPC.netUpdate = true;
			}
			attackCounter++;
			NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
			if (attackCounter > Math.Abs(dashDistance / speed))
			{
				Main.projectile[node].Kill();
				if (phaseTwo)
				{
					NPC.velocity = Vector2.Zero;
					attackCounter = 0;
					NPC.ai[0] = 3;

					timeBetweenAttacks = 25;
				}
				else
				{
					NPC.ai[0] = 0;

					timeBetweenAttacks = 30;
					attackCounter = 0;
				}
			}
		}

		void KickAttack()
		{
			if (attackCounter == 0)
			{
				NPC.spriteDirection = NPC.direction;
			}
			attackCounter++;
			if (attackCounter < 70)
				UpdateFrame(0.2f, 14, 28);
			else
			{
				if (NPC.life > 250)
					UpdateFrame(0.15f, 0, 3);
				else
					UpdateFrame(0.15f, 54, 61);
			}
			if (attackCounter > 90)
			{
				NPC.ai[0] = 1;

				timeBetweenAttacks = 0;
				attackCounter = 0;
			}
			if (attackCounter == 60)
			{

				for (int k = 0; k < 18; k++)
				{
					Dust d = Dust.NewDustPerfect(new Vector2(NPC.Center.X + 75 * NPC.spriteDirection, NPC.Center.Y - 30), 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
					d.noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.Item4, NPC.Center);

				int Ball = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 75 * NPC.spriteDirection, NPC.Center.Y - 30, NPC.spriteDirection * 3.5f, -2f, ModContent.ProjectileType<WizardBall>(), NPCUtils.ToActualDamage(50, 1.5f, 2f), 3f, 0);
				Main.projectile[Ball].ai[0] = NPC.whoAmI;
				Main.projectile[Ball].ai[1] = Main.rand.Next(7, 9);
				Main.projectile[Ball].netUpdate = true;
			}
		}

		Vector2 TeleportPos = Vector2.Zero;

		void TeleportMove()
		{
			Player player = Main.player[NPC.target];
			if (numMoves == -1)
			{
				numMoves = 2;
			}
			if (attackCounter == 0)
			{
				int distance = Main.rand.Next(100, 200);
				double anglex = Math.Sin(NPC.ai[3] * (Math.PI / 180));
				double angley = Math.Cos(NPC.ai[3] * (Math.PI / 180));
				TeleportPos = player.Center + new Vector2((float)anglex * distance, (float)angley * distance); // REVISE
			}

			attackCounter++;

			if (attackCounter > 30)
				UpdateFrame(0.155f, 29, 37);
			else if (attackCounter < 30 && numMoves != 2)
				UpdateFrame(0.2f, 38, 49);
			else if (attackCounter < 30 && numMoves == 2)
			{
				if (NPC.life > 250)
					UpdateFrame(0.15f, 1, 3);
				else
					UpdateFrame(0.15f, 54, 61);
			}

			Dust.NewDustPerfect(TeleportPos, 226, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-6, 0)));

			if (attackCounter > 82)
			{
				attackCounter = 0;

				if (Main.netMode != NetmodeID.Server)
				{
					Gore.NewGore(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y - 50), new Vector2(0, 3), Mod.Find<ModGore>("WizardHat_Gore").Type);
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/BossSFX/MoonWizard_Taunt"), NPC.Center);
				}
				NPC.position = TeleportPos - new Vector2(NPC.width / 2, NPC.height / 2);
				SoundEngine.PlaySound(SoundID.Item94, NPC.position);
				numMoves--;

				for (int i = 0; i < 20; i++)
				{
					int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.RotatedBy(0.314 * i) * 3.95f, ModContent.ProjectileType<MoonBubble>(), NPCUtils.ToActualDamage(40, 1.5f, 2f), 0, NPC.target, 1);
					Main.projectile[p].timeLeft = 180;
				}
				if (numMoves == 0)
				{
					NPC.ai[0] = 0;

					timeBetweenAttacks = 90;
					attackCounter = 0;
					numMoves = -1;
				}
			}
		}

		void Teleport()
		{
			SoundEngine.PlaySound(SoundID.Item66, NPC.Center);
			Player player = Main.player[NPC.target];
			int distance = Main.rand.Next(300, 500);
			bool teleported = false;
			while (!teleported)
			{
				NPC.ai[3] = Main.rand.Next(360);
				double anglex = Math.Sin(NPC.ai[3] * (Math.PI / 180));
				double angley = Math.Cos(NPC.ai[3] * (Math.PI / 180));
				NPC.position.X = player.Center.X + (int)(distance * anglex);
				NPC.position.Y = player.Center.Y + (int)(distance * angley);
				if (Main.tile[(int)(NPC.position.X / 16), (int)(NPC.position.Y / 16)].HasTile || Main.tile[(int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16)].HasTile)
				{
					NPC.alpha = 255;
				}
				else
				{
					teleported = true;
					NPC.alpha = 0;
				}

				NPC.netUpdate = true;
			}
			for (int i = 0; i < 50; i++)
			{
				Dust.NewDustPerfect(NPC.Center, 226, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-6, 0)));
			}
		}
		#endregion

		public override bool PreKill()
		{
			MyWorld.downedMoonWizard = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			NPC.PlayDeathSound("MJWDeathSound");
			return true;
		}

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 2.5f;
			name = "Moon Jelly Wizard";
			downedCondition = () => MyWorld.downedMoonWizard;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<MoonWizard>()
				},
				new List<int> {
					ModContent.ItemType<DreamlightJellyItem>()
				},
				new List<int> {
					ModContent.ItemType<MJWTrophy>(),
					ModContent.ItemType<MJWMask>(),
					ModContent.ItemType<MJWBox>()
				},
				new List<int> {
					ModContent.ItemType<Cornucopion>(),
					ModContent.ItemType<MoonjellySummonStaff>(),
					ModContent.ItemType<NautilusClub>(),
					ModContent.ItemType<JellynautBubble>(),
					ModContent.ItemType<Moonshot>(),
					ModContent.ItemType<MoonJelly>()
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<DreamlightJellyItem>()}] anywhere aboveground at nighttime. A [i:{ModContent.ItemType<DreamlightJellyItem>()}] can be caught with a bug net during the Jelly Deluge, and is non-consumable";
			texture = "SpiritMod/Textures/BossChecklist/MoonWizardTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/MoonWizard/MoonWizard_Head_Boss";
		}
	}
}