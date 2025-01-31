using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Mechanics.BoonSystem;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using SpiritMod.Items.Banners;

namespace SpiritMod.NPCs.Hydra
{
	public class Hydra : ModNPC
	{
		private const int MAXHEADS = 4;

		private bool initialized = false;

		private readonly List<NPC> heads = new();

		public int headsSpawned = 0;

		public int newHeadCountdown = -1;
		public int headsDue = 1;

		private int attackIndex = 0;
		private int attackCounter = 0;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Lernean Hydra");

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 36;
			NPC.damage = 0;
			NPC.defense = 10;
			NPC.lifeMax = 700;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.value = 900f;
			NPC.knockBackResist = 0;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.immortal = true;
			NPC.dontTakeDamage = true;
			NPC.hide = true;

			Banner = ModContent.NPCType<HydraHead>();
			BannerItem = ModContent.ItemType<HydraRedBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(NPC.type))
				return 0f;

			int tile = Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType;
			return (tile == TileID.Marble) && spawnInfo.SpawnTileY > Main.rockLayer && Main.hardMode ? 0.7f : 0f;
		}

		public override void AI()
		{
			NPC.TargetClosest(true);

			if (!initialized)
			{
				initialized = true;

				for (int i = 0; i < 2; i++)
					SpawnHead(NPC.lifeMax);
			}

			foreach (NPC head in heads.ToArray())
				if (!head.active)
					heads.Remove(head);

			if (heads.Count <= 0)
			{
				NPC.life = 0;
				NPC.StrikeNPC(1, 0, 0);
				return;
			}

			if (++attackCounter > 300 / heads.Count)
			{
				attackIndex %= heads.Count;
				attackCounter = 0;
				var modNPC = heads[attackIndex++].ModNPC as HydraHead;
				modNPC.attacking = true;
			}

			if (--newHeadCountdown == 0)
			{
				for (int i = 0; i < headsDue; i++)
					SpawnHead(Math.Max(NPC.lifeMax - (50 * headsSpawned), 100));

				headsDue = 1;
			}
		}

		public void SpawnHead(int life)
		{
			if (heads.Count >= MAXHEADS)
				return;

			headsSpawned++;
			int npcIndex = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<HydraHead>(), 0, NPC.whoAmI);

			NPC head = Main.npc[npcIndex];
			head.life = head.lifeMax = life;
			heads.Add(head);

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
		}
	}

	public enum HeadColor
	{
		Red = 0,
		Green = 1,
		Purple = 2
	}

	public class HydraHead : ModNPC
	{
		public const float FIREBALLGRAVITY = 0.3f;

		private bool initialized = false;

		private NPC Parent => Main.npc[(int)NPC.ai[0]];

		private HeadColor headColor;
		private Vector2 posToBe = Vector2.Zero;
		private float rotation;
		private float sway;
		private float centralRotation;
		private int centralDistance;
		private float rotationSpeed;
		private float swaySpeed;
		private Vector2 orbitRange;
		public bool attacking = false;
		private float headRotationOffset;
		private int attackCounter;
		private int frameY;
		private int frameCounter;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lernean Hydra");
			Main.npcFrameCount[NPC.type] = 3;
			NPCHelper.ImmuneTo(this, BuffID.Poisoned, BuffID.Confused, BuffID.OnFire, BuffID.Venom);

			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				CustomTexturePath = $"{Texture}_Bestiary",
				Position = new Vector2(28f, 8f),
				PortraitPositionXOverride = 12f,
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
		}

		public override void SetDefaults()
		{
			NPC.width = 44;
			NPC.height = 32;
			NPC.damage = 55;
			NPC.defense = 12;
			NPC.lifeMax = 700;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath5;
			NPC.value = Item.buyPrice(0, 1, 0, 0);
			NPC.knockBackResist = 0;
			NPC.noGravity = true;
			NPC.noTileCollide = true;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<HydraRedBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.UIInfoProvider = new CustomEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], false, 25);

			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble,
				new FlavorTextBestiaryInfoElement("A legendary creature, born of monster and god. Killing a head does nothing but worsen the fight."),
			});
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public override void AI()
		{
			NPC.velocity *= 0.96f;
			NPC.velocity.Y *= 0.96f;

			if (!initialized)
			{
				initialized = true;
				headColor = (HeadColor)Main.rand.Next(3);

				rotation = Main.rand.NextFloat(6.28f);
				sway = Main.rand.NextFloat(6.28f);

				centralDistance = Main.rand.Next(75, 125);
				rotationSpeed = Main.rand.NextFloat(0.03f, 0.05f);
				orbitRange = Main.rand.NextVector2Circular(70, 30);
				swaySpeed = Main.rand.NextFloat(0.015f, 0.035f);

				NPC.GetGlobalNPC<BoonNPC>().ApplyBoon(NPC);
			}

			if (!Parent.active)
			{
				NPC.active = false;
				return;
			}

			RotateToPlayer();

			if (!attacking)
			{
				frameCounter++;
				if (frameCounter % 5 == 0)
					frameY++;
				frameY %= NumFrames();
				headRotationOffset = 0f;
				rotation += rotationSpeed;
				sway += swaySpeed;

				headRotationOffset = NPC.DirectionTo(Main.player[Parent.target].Center).ToRotation();
			}
			else
				AttackBehavior();

			NPC.spriteDirection = NPC.direction = Parent.direction;
			centralRotation = 0.3f * ((float)Math.Sin(sway) + (NPC.direction * 1.5f));
			posToBe = DecidePosition();

			MoveToPosition();

			if (!Parent.active)
				NPC.active = false;
		}

		private Vector2 DecidePosition()
		{
			Vector2 pos = new Vector2(0, -1).RotatedBy(centralRotation) * centralDistance;
			pos += orbitRange.RotatedBy(rotation);
			pos += Parent.Center;
			return pos;
		}

		private void MoveToPosition() => NPC.Center = Vector2.Lerp(NPC.Center, posToBe, 0.05f);

		private void AttackBehavior()
		{
			if (attackCounter++ == 0)
				frameY = 0;
			if (attackCounter % 20 == 0)
				frameY++;

			frameY %= Main.npcFrameCount[NPC.type];

			if (headColor == HeadColor.Red)
				headRotationOffset = -1.57f + (NPC.direction * 0.7f);
			else
				headRotationOffset = NPC.DirectionTo(Main.player[Parent.target].Center).ToRotation();

			if (attackCounter == 60)
			{
				LaunchProjectile();
				attacking = false;
				attackCounter = 0;
			}
		}

		private void LaunchProjectile()
		{
			Player target = Main.player[Parent.target];
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));

			Vector2 direction = NPC.DirectionTo(target.Center);

			if (headColor == HeadColor.Red)
				direction = GetArcVel();

			NPC.velocity = -Vector2.Normalize(direction) * Main.rand.Next(7, 10);

			switch (headColor)
			{
				case HeadColor.Red:
					SoundEngine.PlaySound(SoundID.DD2_DrakinBreathIn, NPC.Center);
					SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, NPC.Center);

					for (int k = 0; k < 14; k++)
					{
						int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, -(NPC.position.X - target.position.X) / distance * 2, -(NPC.position.Y - target.position.Y) / distance * 4, 0, default, Main.rand.NextFloat(.65f, .85f));
						Main.dust[d].fadeIn = 1f;
						Main.dust[d].velocity *= .95f;
						Main.dust[d].noGravity = true;
					}

					for (int j = 0; j < 12; j++)
					{
						Vector2 vector2 = Vector2.UnitX * -NPC.width / 2f;
						vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * 3.141591734f / 6f), default) * new Vector2(10f, 24f);
						vector2 = Utils.RotatedBy(vector2, (NPC.rotation - 1.57079637f), default);
						int num8 = Dust.NewDust(NPC.Center, 0, 0, DustID.Torch, 0f, 0f, 160, default, 1f);
						Main.dust[num8].scale = 1.1f;
						Main.dust[num8].noGravity = true;
						Main.dust[num8].position = NPC.Center + vector2;
						Main.dust[num8].velocity = NPC.velocity * 0.1f;
						Main.dust[num8].velocity = Vector2.Normalize(NPC.Center - NPC.velocity * 3f - Main.dust[num8].position) * 1.25f;
					}

					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction, ModContent.ProjectileType<HydraFireGlob>(), NPCUtils.ToActualDamage(NPC.damage), 3);

					break;
				case HeadColor.Green:
					for (int k = 0; k < 20; k++)
					{
						int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Dusts.AcidDust>(), -(NPC.position.X - target.position.X) / distance * 2, -(NPC.position.Y - target.position.Y) / distance * 4, 0, default, Main.rand.NextFloat(.65f, .85f));
						Main.dust[d].fadeIn = .6f;
						Main.dust[d].velocity *= .95f;
						Main.dust[d].noGravity = true;
					}

					SoundEngine.PlaySound(SoundID.NPCHit6 with { PitchVariance = 0.5f, Volume = 0.7f }, NPC.Center);
					SoundEngine.PlaySound(SoundID.Item95 with { PitchVariance = 0.4f }, NPC.Center);

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						if (Main.rand.NextBool())
						{
							float rotationOffset = Main.rand.NextFloat(0.25f, 0.5f);
							for (float i = -rotationOffset; i <= rotationOffset; i += rotationOffset)
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction.RotatedBy(i) * 8, ModContent.ProjectileType<HydraPoisonGlob>(), NPCUtils.ToActualDamage(NPC.damage), 3);
						}
						else
						{
							float rotationOffset = Main.rand.NextFloat(0.15f, 0.4f);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction.RotatedBy(rotationOffset) * 8, ModContent.ProjectileType<HydraPoisonGlob>(), NPCUtils.ToActualDamage(NPC.damage), 3);
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction.RotatedBy(-rotationOffset) * 8, ModContent.ProjectileType<HydraPoisonGlob>(), NPCUtils.ToActualDamage(NPC.damage), 3);
						}
					}
					break;
				case HeadColor.Purple:
					for (int k = 0; k < 14; k++)
					{
						int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.VenomStaff, -(NPC.position.X - target.position.X) / distance * 2, -(NPC.position.Y - target.position.Y) / distance * 4, 0, default, Main.rand.NextFloat(.65f, .85f));
						Main.dust[d].fadeIn = 1f;
						Main.dust[d].velocity *= .95f;
						Main.dust[d].noGravity = true;
					}
					SoundEngine.PlaySound(SoundID.NPCHit23 with { PitchVariance = 0.2f }, NPC.Center);
					SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, NPC.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 15, ModContent.ProjectileType<HydraVenomGlob>(), NPCUtils.ToActualDamage(NPC.damage), 3);
					break;
				default:
					break;
			}
		}

		private Vector2 GetArcVel()
		{

			Vector2 DistanceToTravel = Main.player[Parent.target].Center - NPC.Center;
			float MaxHeight = MathHelper.Clamp(DistanceToTravel.Y - 35, -100, -10);
			float TravelTime = (float)Math.Sqrt(-2 * MaxHeight / FIREBALLGRAVITY) + (float)Math.Sqrt(2 * Math.Max(DistanceToTravel.Y - MaxHeight, 0) / FIREBALLGRAVITY);
			return new Vector2(MathHelper.Clamp(DistanceToTravel.X / TravelTime, -10, 10), -(float)Math.Sqrt(-2 * FIREBALLGRAVITY * MaxHeight));
		}

		private void RotateToPlayer()
		{
			float rotGoal = headRotationOffset;
			float currentRot = NPC.rotation;

			float rotDifference = ((((rotGoal - currentRot) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
			NPC.rotation = MathHelper.Lerp(currentRot, currentRot + rotDifference, 0.05f);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			SoundEngine.PlaySound(SoundID.NPCHit1, NPC.Center);

			if (NPC.life <= 0 && Parent.ModNPC is Hydra modNPC)
			{
				if (modNPC.newHeadCountdown < 0)
					modNPC.newHeadCountdown = 240 + (30 * modNPC.headsSpawned);
				modNPC.headsDue++;

				if (Main.netMode != NetmodeID.Server)
					SpawnGores();
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			LeadingConditionRule greenCondition = new LeadingConditionRule(new DropRuleConditions.NPCConditional("From venomous heads", (npc) => CheckHeadColor(npc, HeadColor.Green)));
			greenCondition.OnSuccess(ItemDropRule.Common(ItemID.PoisonStaff, 50));
			greenCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HydraMaskVenom>(), 33));

			LeadingConditionRule redCondition = new LeadingConditionRule(new DropRuleConditions.NPCConditional("From flaming heads", (npc) => CheckHeadColor(npc, HeadColor.Red)));
			redCondition.OnSuccess(ItemDropRule.Common(ItemID.MagmaStone, 50));
			redCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HydraMaskFire>(), 33));

			LeadingConditionRule purpleCondition = new LeadingConditionRule(new DropRuleConditions.NPCConditional("From acidic heads", (npc) => CheckHeadColor(npc, HeadColor.Purple) && NPC.downedPlantBoss));
			purpleCondition.OnSuccess(ItemDropRule.Common(ItemID.VialofVenom, 3, 1, 3));
			LeadingConditionRule purpleMaskCondition = new LeadingConditionRule(new DropRuleConditions.NPCConditional("From acidic heads", (npc) => CheckHeadColor(npc, HeadColor.Purple)));
			purpleMaskCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HydraMaskAcid>(), 33));

			npcLoot.Add(greenCondition);
			npcLoot.Add(redCondition);
			npcLoot.Add(purpleCondition);
			npcLoot.Add(purpleMaskCondition);
			npcLoot.AddCommon<Items.Accessory.GoldenApple>(100);
		}

		private static bool CheckHeadColor(NPC npc, HeadColor col) => npc.ModNPC is HydraHead hydra && hydra.headColor == col;

		private void SpawnGores()
		{
			string headGore = GetColor() + "HydraHead";
			string neckGore = GetColor() + "HydraNeck";

			Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>(headGore).Type, 1f);

			float goreRotation = NPC.rotation - (NPC.direction == -1 ? 3.14f : 0);

			BezierCurve curve = GetCurve(goreRotation);
			int numPoints = 20;
			Vector2[] chainPositions = curve.GetPoints(numPoints).ToArray();
			for (int i = 1; i < numPoints; i++)
			{
				Vector2 position = chainPositions[i];
				Gore.NewGore(NPC.GetSource_Death(), position, Vector2.Zero, Mod.Find<ModGore>(neckGore).Type, 1f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			float drawRotation = NPC.rotation - (NPC.direction == -1 ? 3.14f : 0);
			string colorString = GetColor();
			string texturePath = Texture + colorString;
			Texture2D headTex;
			if (attacking)
				headTex = ModContent.Request<Texture2D>(texturePath, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			else
				headTex = ModContent.Request<Texture2D>(texturePath + "_Idle", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			Texture2D neckTex = ModContent.Request<Texture2D>(texturePath + "_Neck", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			BezierCurve curve = GetCurve(drawRotation);

			int numPoints = 20; //Should make dynamic based on curve length, but I'm not sure how to smoothly do that while using a bezier curve
			Vector2[] chainPositions = curve.GetPoints(numPoints).ToArray();

			//Draw each chain segment, skipping the very first one, as it draws partially behind the player
			for (int i = 1; i < numPoints; i++)
			{
				Vector2 position = chainPositions[i];

				float rotation = (chainPositions[i] - chainPositions[i - 1]).ToRotation() - MathHelper.PiOver2; //Calculate rotation based on direction from last point
				float yScale = Vector2.Distance(chainPositions[i], chainPositions[i - 1]) / neckTex.Height; //Calculate how much to squash/stretch for smooth chain based on distance between points

				Vector2 scale = new Vector2(1, yScale); // Stretch/Squash chain segment
				Color chainLightColor = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16); //Lighting of the position of the chain segment
				Vector2 origin = new Vector2(neckTex.Width / 2, neckTex.Height); //Draw from center bottom of texture
				spriteBatch.Draw(neckTex, position - screenPos, null, NPC.GetNPCColorTintedByBuffs(chainLightColor), rotation, origin, scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			}

			int frameHeight = headTex.Height / NumFrames();
			Rectangle frame = new Rectangle(0, frameHeight * frameY, headTex.Width, frameHeight);
			spriteBatch.Draw(headTex, NPC.Center - Main.screenPosition, frame, NPC.GetNPCColorTintedByBuffs(drawColor), drawRotation, new Vector2(headTex.Width, frameHeight) / 2, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}

		private BezierCurve GetCurve(float headRotation)
		{
			Vector2 direction = NPC.Center - Parent.Bottom;
			Vector2 centralPos = (new Vector2(0, -1) * direction.Length());

			//Control point relative to the parent npc
			Vector2 BaseControlPoint = Parent.Bottom + (centralPos.RotatedBy(-centralRotation / 2) * 0.5f);

			//Control point connecting to behind the npc, to make the neck look more like a neck
			float headControlLength = 100;
			Vector2 HeadControlPoint = NPC.Center - Vector2.UnitX.RotatedBy(headRotation + ((NPC.spriteDirection < 0) ? MathHelper.Pi : 0)) * headControlLength;

			//Control point to smooth out the bezier, taking the midway point beween the other 2 control points and moving it backwards from them perpindicularly 
			float smootheningFactor = 0.4f;
			Vector2 SmootheningControlPoint = Vector2.Lerp(BaseControlPoint, HeadControlPoint, 0.5f) + (HeadControlPoint - BaseControlPoint).RotatedBy(-NPC.spriteDirection * MathHelper.PiOver2) * smootheningFactor;

			return new BezierCurve(new Vector2[] { Parent.Bottom, BaseControlPoint, SmootheningControlPoint, HeadControlPoint, NPC.Center });
		}

		private string GetColor()
		{
			return (int)headColor switch
			{
				0 => "_Red",
				1 => "_Green",
				2 => "_Purple",
				_ => "_Red",
			};
		}

		private int NumFrames()
		{
			if (attacking)
				return 3;
			else
			{
				return GetColor() switch
				{
					"_Purple" => 23,
					_ => 13,
				};
			}
		}
	}

	public class HydraFireGlob : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Spit");
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
			Projectile.damage = 60;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Main.rand.NextBool(4))
			{
				Vector2 dustVel = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.4f);
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, dustVel.X, dustVel.Y);
			}

			Projectile.frameCounter++;

			if (Projectile.frameCounter % 4 == 0)
			{
				Projectile.frame++;
				Projectile.frame %= Main.projFrames[Projectile.type];
			}

			Projectile.localAI[0] += 1f;

			if (Projectile.localAI[0] == 12f)
			{
				Projectile.localAI[0] = 0f;
				for (int j = 0; j < 12; j++)
				{
					Vector2 vector2 = Vector2.UnitX * -Projectile.width / 2f;
					vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * 3.141591734f / 6f), default) * new Vector2(8f, 24f);
					vector2 = Utils.RotatedBy(vector2, (Projectile.rotation), default);
					int num8 = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, 0f, 0f, 160, default, 1f);
					Main.dust[num8].scale = 1.1f;
					Main.dust[num8].noGravity = true;
					Main.dust[num8].position = Projectile.Center + vector2;
					Main.dust[num8].velocity = Projectile.velocity * 0.1f;
					Main.dust[num8].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num8].position) * 1.25f;
				}
			}
			Projectile.velocity.Y += HydraHead.FIREBALLGRAVITY;
		}

		public void AdditiveCall(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			float scale = Projectile.scale;
			Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/NPCs/Hydra/HydraFireGlob_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Color color = Color.White * 0.65f;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frameRect = new Rectangle(0, Projectile.frame * frameHeight, tex.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);

			spriteBatch.Draw(tex, Projectile.position - screenPos + drawOrigin + new Vector2(0f, Projectile.gfxOffY), frameRect, color, Projectile.rotation, drawOrigin, scale * 1.23f, default, default);
			spriteBatch.Draw(tex, Projectile.position - screenPos + drawOrigin + new Vector2(0f, Projectile.gfxOffY), frameRect, color, Projectile.rotation, drawOrigin, scale * 1.43f, default, default);

		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.OnFire, 200);

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			Rectangle frameRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Color.White * .5f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, frameRect, color, Projectile.rotation, drawOrigin, Projectile.scale * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY), frameRect, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item45 with { PitchVariance = 0.2f }, Projectile.Center);
			SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);

			for (int i = 0; i < 20; i++)
				Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(4, 4));
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HydraExplosion>(), NPCUtils.ToActualDamage(Projectile.damage), 3, Projectile.owner);
		}
	}

	public class HydraExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Spit");
			Main.projFrames[Projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			Projectile.width = 85;
			Projectile.height = 85;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3());
			Projectile.frameCounter++;
			Projectile.hostile = Projectile.frame > 1;
			if (Projectile.frameCounter % 3 == 0)
			{
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.active = false;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.OnFire, 200);

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, new Vector2(tex.Width / 2, frameHeight / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class HydraPoisonGlob : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Spit");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.hostile = true;
			Projectile.tileCollide = true;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.36f;
			Projectile.scale = num395 + 0.45f;

			Projectile.frameCounter++;
			if (Projectile.frameCounter % 4 == 0)
			{
				Projectile.frame++;
				Projectile.frame %= Main.projFrames[Projectile.type];
			}

			Projectile.velocity.Y += .061f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Lighting.AddLight(Projectile.Center, 0.113f, 0.227f, 0.05f);

			if (Main.rand.NextBool(7))
			{
				Vector2 dustVel = -Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.4f);
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.AcidDust>(), dustVel.X, dustVel.Y);
				Main.dust[d].scale = Main.rand.NextFloat(.6f, .8f);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			Rectangle frameRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.75f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Color.White * .5f * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, frameRect, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.position - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY), frameRect, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Poisoned, 200);

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item54 with { PitchVariance = 0.2f }, Projectile.Center);
			SoundEngine.PlaySound(SoundID.NPCDeath3 with { PitchVariance = 0.2f }, Projectile.Center);
			SoundEngine.PlaySound(SoundID.Item112 with { PitchVariance = 0.2f, Volume = 0.6f }, Projectile.Center);

			for (int i = 0; i < 18; i++)
				Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<Dusts.AcidDust>(), Main.rand.NextVector2Circular(3, 3));
		}
	}

	public class HydraVenomGlob : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydra Spit");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.tileCollide = true;
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(Projectile, new GradientTrail(Color.Orchid, Color.MediumPurple * .75f), new RoundCap(), new DefaultTrailPosition(), 8f, 95f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2").Value, 0.01f, 1f, 1f));

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			Lighting.AddLight(Projectile.Center, 0.45f, 0.15f, 0.45f);

			if (Main.rand.NextBool(2))
			{
				float x1 = Projectile.Center.X - Projectile.velocity.X / 10f;
				float y1 = Projectile.Center.Y - Projectile.velocity.Y / 10f;
				int num1 = Dust.NewDust(new Vector2(x1, y1), 2, 2, DustID.VenomStaff);
				Main.dust[num1].alpha = Projectile.alpha;
				Main.dust[num1].velocity = Projectile.velocity;
				Main.dust[num1].fadeIn += 0.6684f;
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1.1235f;
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter % 2 == 0)
			{
				Projectile.frame++;
				Projectile.frame %= Main.projFrames[Projectile.type];
			}
		}

		public override void Kill(int timeLeft) => SoundEngine.PlaySound(SoundID.NPCDeath3 with { PitchVariance = 0.2f }, Projectile.Center);

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, new Vector2(tex.Width * 0.75f, frameHeight / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Venom, 200);
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}