using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Armor;
using SpiritMod.Buffs.Glyph;
using SpiritMod.Dusts;
using SpiritMod.Items;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.Atlas;
using SpiritMod.NPCs.Boss.MoonWizard;
using SpiritMod.NPCs.Mimic;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Audio;
using SpiritMod.NPCs.AuroraStag;
using Terraria.Graphics.Effects;
using SpiritMod.Projectiles.Bullet;
using System.Linq;
using SpiritMod.Skies.Overlays;
using SpiritMod.Items.Armor.StarjinxSet;
using SpiritMod.Items.Sets.BloodcourtSet.BloodCourt;
using SpiritMod.Items.Accessory.SeaSnailVenom;
using SpiritMod.Items.Accessory.MoonlightSack;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Buffs.DoT;
using SpiritMod.GlobalClasses.Players;
using SpiritMod.NPCs.AsteroidDebris;
using SpiritMod.Items.Sets.GraniteSet.GraniteArmor.Projectiles;
using SpiritMod.Items.Sets.AccessoriesMisc.CrystalFlower;
using SpiritMod.Items.Accessory.DarkfeatherVisage;
using SpiritMod.Items.Sets.RunicSet.RunicArmor;
using SpiritMod.Items.Accessory.ShieldCore;
using SpiritMod.Items.BossLoot.StarplateDrops.StarArmor;

namespace SpiritMod
{
	public class MyPlayer : ModPlayer
	{
		public const int CAMO_DELAY = 100;

		internal static bool swingingCheck;
		internal static Item swingingItem;

		public List<SpiritPlayerEffect> effects = new();
		public List<SpiritPlayerEffect> removedEffects = new();
		public SpiritPlayerEffect setbonus = null;

		public int Shake = 0;
		public bool clockActive = false;
		public bool rabbitMinion = false;
		public bool bismiteShield = false;
		public int shieldCounter = 0;
		public int bismiteShieldStacks;
		public bool MetalBand = false;
		public bool KoiTotem = false;
		public bool VampireCloak = false;
		public bool starplateGlitchEffect = false;
		public bool HealCloak = false;
		public bool SpiritCloak = false;
		public bool crystalFlower = false;
		public bool nearLure = false;
		public int clockX = 0;
		public int clockY = 0;
		public bool astralSet = false;
		public bool bloodcourtSet = false;
		public int astralSetStacks;
		public bool frigidGloves = false;
		public bool mushroomPotion = false;
		public bool flightPotion = false; 
		public bool magnifyingGlass = false;
		public bool shieldCore = false;
		public bool SoulStone = false;
		public bool assassinMag = false;
		public bool shadowFang = false;
		public bool reachBrooch = false;
		public bool cleftHorn = false;
		public bool mimicRepellent = false;
		public bool daybloomSet = false;
		public bool ToxicExtract = false;
		public bool vitaStone = false;
		public bool throwerGlove = false;
		public int throwerStacks;
		public bool scarabCharm = false;
		public bool bloodCourtHead = false;
		public bool timScroll = false;
		public bool cultistScarf = false;
		public bool AnimeSword = false;
		public bool geodeRanged = false;
		public bool fireMaw = false;
		public bool deathRose = false;
		public bool manaWings = false;
		public bool infernalFlame = false;
		public bool floranSet = false;
		public bool rogueSet = false;
		public bool ChaosCrystal = false;
		public bool wheezeScale = false;
		public bool winterbornCharmMage = false;
		public bool sepulchreCharm = false;
		public bool HellGaze = false;
		public bool leatherGlove = false;
		public bool forbiddenTome = false;
		public bool teslaCoil = false;
		public bool Phantom = false;
		public bool illusionistEye = false;
		public bool stoneplate = false;

		private bool canJustLand = false;
		public bool justLanded = false;

		public float oakHeartStacks = 0;
		public readonly int oakHeartStacksMax = 5;

		private int attackTimer;
		public int beetleStacks = 1;

		public int miningStacks = 1;
		public int damageStacks = 1;
		public int movementStacks = 1;

		public bool bloodfireShield;

		public bool cragboundMinion = false;
		public bool clatterboneShield = false;
		public bool NebulaPearl = false;
		public bool CursedPendant = false;
		public bool starMap = false;
		public bool moonlightSack = false;
		public bool DoomDestiny = false;
		public bool daybloomGarb = false;
		public int frigidGloveStacks;
		public bool gemPickaxe = false;
		public bool butterflyMinion = false;
		public bool DungeonSummon = false;
		public int soulSiphon;
		public bool oliveBranchBuff = false;
		public bool leatherHood = false;
		public int clatterStacks;
		public bool strikeshield = false;

		public float SpeedMPH { get; private set; }
		public GlyphType glyph;

		public int voidStacks = 1;
		public int camoCounter;
		public int veilCounter;
		public int jellynautStacks; //
		public bool blazeBurn;
		public bool phaseShift;
		private readonly float[] phaseSlice = new float[60];
		public int divineCounter;
		public int divineStacks = 1;
		public int stormStacks;
		public int frostCooldown;
		public float frostRotation;
		public bool frostUpdate;
		public int frostTally;
		public int frostCount;
		public bool jellynautHelm;

		public int shadowCount;

		// Armor set booleans.
		public bool duskSet;
		public bool runicSet;
		public bool darkfeatherVisage;
		public bool elderbarkWoodSet;
		public bool primalSet;
		public bool spiritSet;
		public bool leatherSet;
		public bool oceanSet;
		public bool wayfarerSet;
		public bool marbleSet;
		public bool midasTouch;
		public bool bloodfireSet;
		public bool cryoSet;
		public bool frigidSet;
		public bool graniteSet;
		public bool infernalSet;
		public bool fierySet;
		public bool starSet;
		public bool clatterboneSet;
		public bool talonSet;

		//public bool ZoneAsteroid = false;
		public bool ZoneSpider = false;
		//public bool ZoneSynthwave = false;
		public bool ZoneLantern = false;
		//public bool ZoneSpirit = false;
		//public bool ZoneReach = false;

		public bool inGranite = false;
		public bool inMarble = false;

		public bool marbleJustJumped;

		// Accessory booleans.
		public bool infernalShield;
		public bool seaSnailVenom;
		public bool bloodyBauble;
		public bool surferSet;
		public int illusionistTimer;
		public float cryoTimer = 0f;
		public float marbleJump = 420f;
		public bool moonGauntlet;
		public bool starCharm;
		public int infernalHit;
		public int infernalDash;
		public bool longFuse;
		public bool granitechDrones;

		public bool windEffect;
		public bool windEffect2;
		public int infernalSetCooldown;
		public int fierySetTimer = 480;
		public int surferTimer = 330;
		public int bubbleTimer;
		public float starplateGlitchIntensity;
		public int clatterboneTimer;
		public int roseTimer;
		public bool concentrated; // For the leather armor set.
		public int concentratedCooldown = 360;
		public int stompCooldown = 30;
		public bool basiliskMount;
		public bool drakomireMount;
		public int drakomireFlameTimer;
		public bool drakinMount;
		public bool spiritBuff;
		public bool starBuff;
		public bool runeBuff;
		public bool poisonPotion;
		public bool soulPotion;

		public float WingTimeMaxMultiplier = 1f;
		public bool StarjinxSet = false;
		public int starjinxtimer = 0;

		public bool usingHelios = false;
		public bool oldHelios = false;

		public AuroraStag hoveredStag;

		public int candyInBowl;
		private IList<string> candyFromTown = new List<string>();

		public Dictionary<int, int> auroraMonoliths = new()
		{
			{ AuroraOverlay.UNUSED_BASIC, 0 }, { AuroraOverlay.PRIMARY, 0 }, { AuroraOverlay.PRIMARY_ALT1, 0 },
			{ AuroraOverlay.PRIMARY_ALT2, 0 }, { AuroraOverlay.PRIMARY_ALT3, 0 }, { AuroraOverlay.BLOODMOON, 0 },
			{ AuroraOverlay.PUMPKINMOON, 0 }, { AuroraOverlay.FROSTMOON, 0 }, { AuroraOverlay.BLUEMOON, 0 },
			{ AuroraOverlay.SPIRIT, 0 }
		};

		public Dictionary<string, int> fountainsActive = new()
		{
			{ "BRIAR", 0 }
		};

		public override void PostUpdateMiscEffects()
		{
			var config = ModContent.GetInstance<SpiritClientConfig>();
			bool reach = (!Main.dayTime && Player.ZoneBriar() && !reachBrooch && Player.ZoneOverworldHeight) || (Player.ZoneBriar() && Player.ZoneOverworldHeight && MyWorld.downedReachBoss && Main.dayTime);
			bool spirit = Player.ZoneOverworldHeight && Player.ZoneSpirit();

			bool region1 = Player.ZoneSpirit() && Player.ZoneRockLayerHeight && Player.position.Y / 16 > (Main.rockLayer + Main.maxTilesY - 330) / 2f;
			bool region2 = Player.ZoneSpirit() && Player.position.Y / 16 >= Main.maxTilesY - 300;

			bool showJellies = ((Player.ZoneOverworldHeight || Player.ZoneSkyHeight) && MyWorld.jellySky) || NPC.AnyNPCs(ModContent.NPCType<MoonWizard>());
			bool underwater = Player.ZoneBeach && Submerged(30);

			bool greenOcean = Player.ZoneBeach && MyWorld.luminousType == 1 && MyWorld.luminousOcean;
			bool blueOcean = Player.ZoneBeach && MyWorld.luminousType == 2 && MyWorld.luminousOcean;
			bool purpleOcean = Player.ZoneBeach && MyWorld.luminousType == 3 && MyWorld.luminousOcean;

			bool blueMoon = MyWorld.blueMoon && (Player.ZoneOverworldHeight || Player.ZoneSkyHeight);

			if (Main.netMode != NetmodeID.Server)
			{
				if (config.DistortionConfig)
				{
					if (starplateGlitchEffect)
					{
						SpiritMod.glitchEffect.Parameters["Speed"].SetValue(0.3f);
						SpiritMod.glitchScreenShader.UseIntensity(starplateGlitchIntensity);
						Player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", true);
					}
					else if (Player.ZoneSynthwave())
					{
						SpiritMod.glitchEffect.Parameters["Speed"].SetValue(0.115f); //0.4f is default
						SpiritMod.glitchScreenShader.UseIntensity(0.0008f);
						Player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", true);
					}
					else
						Player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", false);
				}
				else
					Player.ManageSpecialBiomeVisuals("SpiritMod:Glitch", false);

				bool showAurora = (Player.ZoneSnow || Player.ZoneSpirit() || Player.ZoneSkyHeight) && !Main.dayTime && !Main.raining && !Player.ZoneCorrupt && !Player.ZoneCrimson && MyWorld.aurora;

				ManageAshrainShader();

				Player.ManageSpecialBiomeVisuals("SpiritMod:AuroraSky", showAurora || auroraMonoliths.Any(x => x.Value >= 1));
				Player.ManageSpecialBiomeVisuals("SpiritMod:SpiritBiomeSky", spirit);
				Player.ManageSpecialBiomeVisuals("SpiritMod:AsteroidSky2", Player.ZoneAsteroid());

				Player.ManageSpecialBiomeVisuals("SpiritMod:GreenAlgaeSky", greenOcean);
				Player.ManageSpecialBiomeVisuals("SpiritMod:BlueAlgaeSky", blueOcean);
				Player.ManageSpecialBiomeVisuals("SpiritMod:PurpleAlgaeSky", purpleOcean);

				Player.ManageSpecialBiomeVisuals("SpiritMod:JellySky", showJellies);

				Player.ManageSpecialBiomeVisuals("SpiritMod:OceanFloorSky", underwater);

				Player.ManageSpecialBiomeVisuals("SpiritMod:SpiritUG1", region1);
				Player.ManageSpecialBiomeVisuals("SpiritMod:SpiritUG2", region2);

				Player.ManageSpecialBiomeVisuals("SpiritMod:ReachSky", reach, Player.Center);
				Player.ManageSpecialBiomeVisuals("SpiritMod:BlueMoonSky", blueMoon, Player.Center);
				Player.ManageSpecialBiomeVisuals("SpiritMod:MeteorSky", Player.ZoneAsteroid());
				Player.ManageSpecialBiomeVisuals("SpiritMod:MeteoriteSky", Player.ZoneMeteor);
				Player.ManageSpecialBiomeVisuals("SpiritMod:BloodMoonSky", Main.bloodMoon && Player.ZoneOverworldHeight);
				Player.ManageSpecialBiomeVisuals("SpiritMod:WindEffect", windEffect, Player.Center);
				Player.ManageSpecialBiomeVisuals("SpiritMod:WindEffect2", windEffect2, Player.Center);
				Player.ManageSpecialBiomeVisuals("SpiritMod:Atlas", NPC.AnyNPCs(ModContent.NPCType<Atlas>()));
			}
		}

		private void ManageAshrainShader()
		{
			Filter ashrain = Filters.Scene["SpiritMod:AshRain"];
			float deltaProgress = 0.1f;
			float maxIntensity = 0.5f;
			float deltaintensity = 0.02f * maxIntensity;

			if (!Player.ZoneUnderworldHeight || !MyWorld.ashRain)
			{
				if (ashrain.IsActive())
				{
					ashrain.GetShader().UseIntensity(Math.Max(ashrain.GetShader().Intensity - deltaintensity, 0));
					ashrain.GetShader().UseProgress(Main.GlobalTimeWrappedHourly * 10 * deltaProgress);
					if (ashrain.GetShader().Intensity <= 0)
						ashrain.Deactivate();
				}
				return;
			}
			else if (!ashrain.IsActive())
				Filters.Scene.Activate("SpiritMod:AshRain", Vector2.Zero).GetShader()
					.UseColor(0.15f, 0.1f, 0.15f)
					.UseIntensity(deltaintensity)
					.UseImage(Mod.Assets.Request<Texture2D>("Textures/noise").Value)
					.UseImage(Mod.Assets.Request<Texture2D>("Textures/3dNoise").Value, 1);
			else
			{
				float intensity = Math.Min(ashrain.GetShader().Intensity + deltaintensity, maxIntensity);

				bool wall = true;
				for (int i = (int)Player.Center.X - 16; i < Player.Center.X + 16; i += 16)
				{
					for (int j = (int)Player.Center.Y - 16; j < Player.Center.Y + 16; j += 16)
					{
						Point p = new Point((int)(i / 16f), (int)(j / 16f));
						Tile t = Framing.GetTileSafely(p);

						if (t.WallType == WallID.None)
						{
							wall = false;
							break;
						}
					}
				}

				if (wall)
					intensity *= 0.95f;

				ashrain.GetShader().UseIntensity(intensity);
				ashrain.GetShader().UseProgress(Main.GlobalTimeWrappedHourly * 10 * deltaProgress);
			}
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("candyInBowl", candyInBowl);
			tag.Add("candyFromTown", candyFromTown);
		}

		public override void LoadData(TagCompound tag)
		{
			candyInBowl = tag.GetInt("candyInBowl");
			candyFromTown = tag.GetList<string>("candyFromTown");
		}

		public override void ResetEffects()
		{
			removedEffects = effects;
			effects = new List<SpiritPlayerEffect>();

			ResetMiscVariables();

			// Reset armor set booleans.
			ResetArmorBools();

			marbleJustJumped = false;

			// Reset accessory booleans.
			ResetAccBools();

			for (int i = 0; i < AuroraOverlay.COUNT; ++i) //Reset aurora monolith values
			{
				if (i == AuroraOverlay.COMPLETELY_UNIMPLEMENTED)
					continue;
				auroraMonoliths[i]--;
			}

			fountainsActive["BRIAR"]--;

			if (Player.FindBuffIndex(ModContent.BuffType<BeetleFortitude>()) < 0)
				beetleStacks = 1;

			if (Player.FindBuffIndex(ModContent.BuffType<ExplorerFight>()) < 0)
				damageStacks = 1;

			if (Player.FindBuffIndex(ModContent.BuffType<ExplorerPot>()) < 0)
				movementStacks = 1;

			if (Player.FindBuffIndex(ModContent.BuffType<ExplorerMine>()) < 0)
				miningStacks = 1;

			if (Player.FindBuffIndex(ModContent.BuffType<CollapsingVoid>()) < 0)
				voidStacks = 1;

			phaseShift = false;
			blazeBurn = false;

			if (glyph != GlyphType.Veil)
				veilCounter = 0;

			if (glyph != GlyphType.Radiant)
				divineStacks = 1;

			if (glyph != GlyphType.Storm)
				stormStacks = 0;

			if (frostCooldown > 0)
				frostCooldown--;

			frostRotation += Items.Glyphs.FrostGlyph.TURNRATE;
			if (frostRotation > MathHelper.TwoPi)
				frostRotation -= MathHelper.TwoPi;

			if (frostUpdate)
			{
				frostUpdate = false;
				if (glyph == GlyphType.Frost)
					Items.Glyphs.FrostGlyph.UpdateIceSpikes(Player);
			}

			frostCount = frostTally;
			frostTally = 0;
		}

		private void ResetAccBools()
		{
			crystalFlower = false;
			mimicRepellent = false;
			forbiddenTome = false;
			deathRose = false;
			infernalShield = false;
			illusionistEye = false;
			moonGauntlet = false;
			longFuse = false;
			granitechDrones = false;

			WingTimeMaxMultiplier = 1f;
			StarjinxSet = false;
			oldHelios = usingHelios;
			usingHelios = false;
	}

	private void ResetMiscVariables()
		{
			oliveBranchBuff = false;
			MetalBand = false;
			strikeshield = false;
			KoiTotem = false;
			setbonus = null;
			moonlightSack = false;
			midasTouch = false;
			seaSnailVenom = false;
			clockActive = false;
			bloodcourtSet = false;
			shieldCore = false;
			elderbarkWoodSet = false;
			cleftHorn = false;
			throwerGlove = false;
			rabbitMinion = false;
			VampireCloak = false;
			SpiritCloak = false;
			HealCloak = false;
			vitaStone = false;
			astralSet = false;
			mushroomPotion = false;
			ChaosCrystal = false;
			teslaCoil = false;
			ToxicExtract = false;
			shadowFang = false;
			gemPickaxe = false;
			cultistScarf = false;
			surferSet = false;
			bloodCourtHead = false;
			scarabCharm = false;
			assassinMag = false;
			stoneplate = false;

			justLanded = canJustLand && Player.velocity.Y == 0 && Player.grappling[0] == -1;
			canJustLand = Player.velocity.Y != 0;

			jellynautHelm = false;
			starplateGlitchEffect = false;
			infernalFlame = false;
			reachBrooch = false;
			windEffect = false;
			windEffect2 = false;
			leatherHood = false;
			floranSet = false;
			SoulStone = false;
			manaWings = false;
			fireMaw = false;
			rogueSet = false;
			timScroll = false;
			wheezeScale = false;
			HellGaze = false;
			geodeRanged = false;
			bloodfireShield = false;
			Phantom = false;
			magnifyingGlass = false;
			daybloomSet = false;
			daybloomGarb = false;
			CursedPendant = false;
			starCharm = false;
			starMap = false;
			frigidGloves = false;
			NebulaPearl = false;
			bismiteShield = false;
			winterbornCharmMage = false;
			clatterboneShield = false;
			leatherGlove = false;
			cragboundMinion = false;
			DungeonSummon = false;
			butterflyMinion = false;
			drakomireMount = false;
			basiliskMount = false;
			spiritBuff = false;
			drakinMount = false;
			poisonPotion = false;
			starBuff = false;
			runeBuff = false;
			soulPotion = false;
		}

		private void ResetArmorBools()
		{
			duskSet = false;
			runicSet = false;
			darkfeatherVisage = false;
			primalSet = false;
			wayfarerSet = false;
			spiritSet = false;
			graniteSet = false;
			fierySet = false;
			leatherSet = false;
			starSet = false;
			bloodfireSet = false;
			oceanSet = false;
			cryoSet = false;
			frigidSet = false;
			marbleSet = false;
			infernalSet = false;
			clatterboneSet = false;
			talonSet = false;
		}

		public bool marbleJumpEffects = false;

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (marbleSet && Player.controlUp && Player.releaseUp && marbleJump <= 0)
			{
				Player.AddBuff(ModContent.BuffType<MarbleDivineWinds>(), 120);
				SoundEngine.PlaySound(SoundID.Item20, Player.position);
				for (int i = 0; i < 8; i++)
				{
					int num = Dust.NewDust(Player.position, Player.width, Player.height, DustID.FireworkFountain_Yellow, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if (Main.dust[num].position != Player.Center)
						Main.dust[num].velocity = Player.DirectionTo(Main.dust[num].position) * 6f;
				}
				marbleJump = 600;
			}

			if (marbleSet && (Player.sliding || Player.velocity.Y == 0f))
				Player.justJumped = true;

			if (Player.controlJump)
			{
				if (marbleJustJumped)
				{
					marbleJustJumped = false;
					if (Player.HasBuff(ModContent.BuffType<MarbleDivineWinds>()))
					{
						if (Main.rand.NextBool(20))
						{
							SoundEngine.PlaySound(SoundID.Item24, Player.Center);
						}
						marbleJumpEffects = true;
						int num23 = Player.height;

						if (Player.gravDir == -1f)
							num23 = 0;

						Player.velocity.Y = (0f - Player.jumpSpeed) * Player.gravDir;
						Player.jump = (int)(Player.jumpHeight * 1.25);
						for (int m = 0; m < 4; m++)
						{
							int num22 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + num23), Player.width, 6, DustID.FireworkFountain_Yellow, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 100, Color.White, .8f);
							Dust dust = Main.dust[num22];
							if (m % 2 == 0)
								dust.velocity.X += Main.rand.Next(10, 31) * 0.1f;
							else
								dust.velocity.X -= Main.rand.Next(31, 71) * 0.1f;
							dust.velocity.Y += Main.rand.Next(-10, 31) * 0.1f;
							dust.noGravity = true;
							dust.scale += Main.rand.Next(-10, 11) * .0025f;
							dust.velocity *= Main.dust[num22].scale * 0.7f;
						}
					}
				}
			}

			// quest related hotkeys
			if (SpiritMod.QuestBookHotkey.JustPressed && QuestManager.QuestBookUnlocked) // swap the quest book's state around, if it's open, close it, and vice versa.
				QuestManager.SetBookState(SpiritMod.Instance.BookUserInterface.CurrentState is not UI.QuestUI.QuestBookUI);

			if (SpiritMod.QuestHUDHotkey.JustPressed && QuestManager.QuestBookUnlocked) // swap the quest book's state around, if it's open, close it, and vice versa.
				SpiritMod.QuestHUD.Toggle();
		}

		public override bool PreItemCheck()
		{
			PrepareShotDetection();
			return true;
		}

		public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
		{
			BeginShotDetection(item);

			if (daybloomGarb && item.IsMagic())
				damage.Flat++;
			if (leatherHood && item.IsRanged())
				damage.Flat++;
			if (elderbarkWoodSet)
				damage.Flat++;
		}

		public override void PostItemCheck() => EndShotDetection();

		private void PrepareShotDetection()
		{
			if (Player.whoAmI == Main.myPlayer && !Player.HeldItem.IsAir && !Main.gamePaused)
				swingingItem = Player.HeldItem;
		}

		private static void BeginShotDetection(Item item)
		{
			if (swingingItem == item)
				swingingCheck = true;
		}

		private static void EndShotDetection()
		{
			swingingItem = null;
			swingingCheck = false;
		}

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			var config = ModContent.GetInstance<SpiritClientConfig>();

			if (KoiTotem && Main.rand.NextBool(10))
			{
				if (attempt.playerFishingConditions.Bait.stack < attempt.playerFishingConditions.Bait.maxStack)
					attempt.playerFishingConditions.Bait.stack++;
			}

			if (config.EnemyFishing && (attempt.common || attempt.uncommon))
			{
				if (Main.bloodMoon && Main.rand.NextBool(20))
				{
					itemDrop = 0;
					attempt.rolledEnemySpawn = ModContent.NPCType<NPCs.BottomFeeder.BottomFeeder>();
					return;
				}

				if (!mimicRepellent && attempt.crate && !attempt.inLava)
				{
					if (Main.rand.NextBool(20))
					{
						itemDrop = 0;
						npcSpawn = ModContent.NPCType<WoodCrateMimic>();
						return;
					}

					if (Main.rand.NextBool(40))
					{
						itemDrop = 0;
						npcSpawn = ModContent.NPCType<IronCrateMimic>();
						return;
					}

					if (Main.rand.NextBool(60))
					{
						itemDrop = 0;
						npcSpawn = ModContent.NPCType<GoldCrateMimic>();
						return;
					}
				}
			}

			if (Player.ZoneSpirit() && NPC.downedMechBossAny && Main.rand.NextBool(Player.cratePotion ? 35 : 65))
				itemDrop = ModContent.ItemType<SpiritCrate>();

			if (Player.ZoneSpirit() && NPC.downedMechBossAny && Main.rand.NextBool(5))
				itemDrop = ModContent.ItemType<SpiritKoi>();

			if (Player.ZoneBriar() && Main.rand.NextBool(5))
				itemDrop = ModContent.ItemType<Items.Sets.BriarDrops.ReachFishingCatch>();

			if (Player.ZoneBriar() && !Main.hardMode && Main.rand.NextBool(Player.cratePotion ? 25 : 45))
				itemDrop = ModContent.ItemType<ReachCrate>();

			if (Player.ZoneBriar() && Main.hardMode && Main.rand.NextBool(Player.cratePotion ? 25 : 45))
				itemDrop = ModContent.ItemType<BriarCrate>();

			if (Player.ZoneBriar() && Main.rand.NextBool(25))
				itemDrop = ModContent.ItemType<ThornDevilfish>();

			if (Player.ZoneGlowshroom && Main.rand.NextBool(27))
				itemDrop = ModContent.ItemType<ShroomFishSummon>();

			if (Player.ZoneBeach && Main.rand.NextBool(125))
				itemDrop = ModContent.ItemType<Items.Sets.ClubSubclass.BassSlapper>();
		}

		public override void AnglerQuestReward(float quality, List<Item> rewardItems)
		{
			if (Main.rand.NextBool(10))
			{
				Item repel = new Item();
				repel.SetDefaults(ModContent.ItemType<MimicRepellent>());
				//repel.stack = 1;
				rewardItems.Add(repel);
			}
			if (Main.rand.NextBool(5))
			{
				Item wood = new Item();
				wood.SetDefaults(ModContent.ItemType<Items.Sets.FloatingItems.Driftwood.DriftwoodTileItem>());
				wood.stack = Main.rand.Next(10, 20);
				rewardItems.Add(wood);
			}
		}

		public override void OnHitAnything(float x, float y, Entity victim)
		{
			if (Player.HeldItem.type == ModContent.ItemType<Items.Sets.TideDrops.Minifish>() && MinifishTimer <= 0)
			{
				MinifishTimer = 120;
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<MinifishProj>()] < 3)
				{
					var spawnPos = Player.Center + Main.rand.NextVector2Square(-50, 50) - new Vector2(0, 50);
					var p = Projectile.NewProjectileDirect(Player.GetSource_OnHit(victim), spawnPos, Vector2.Zero, ModContent.ProjectileType<MinifishProj>(), Player.HeldItem.damage, Player.HeldItem.knockBack, Player.whoAmI);
					p.netUpdate = true;
				}
			}
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			foreach (var effect in effects)
				effect.PlayerOnHitNPC(Player, item, target, damage, knockback, crit);

			if (winterbornCharmMage && Main.rand.NextBool(9))
				target.AddBuff(ModContent.BuffType<MageFreeze>(), 180);

			if (astralSet && crit)
				damage = (int)(damage + (.1f * astralSetStacks));

			if (shadowFang)
			{
				if (target.life <= target.lifeMax / 2 && Main.rand.NextBool(7))
				{
					Projectile.NewProjectile(item.GetSource_OnHit(target), target.position, Vector2.Zero, ModContent.ProjectileType<ShadowSingeProj>(), item.damage / 3 * 2, 4, Main.myPlayer);
					SoundEngine.PlaySound(SoundID.NPCDeath6);
					Player.statLife -= 3;
				}
			}

			if (frigidGloves && crit && item.IsMelee())
				target.AddBuff(BuffID.Frostburn, 180);

			if (forbiddenTome)
			{
				if (target.life <= 0 && !target.SpawnedFromStatue && Player.ownedProjectileCounts[ModContent.ProjectileType<GhastSkullFriendly>()] <= 8)
					SpawnForbiddenTomeGhasts(item.GetSource_OnHit(target), target, damage, knockback);
			}

			if (midasTouch)
				target.AddBuff(BuffID.Midas, 240);

			if (wheezeScale && Main.rand.NextBool(9) && item.IsMelee())
			{
				float rand = Main.rand.NextFloat() * 6.283f;
				Vector2 vel = new Vector2(0, -1).RotatedBy(rand) * 8f;
				Projectile.NewProjectile(item.GetSource_OnHit(target), target.Center, vel, ModContent.ProjectileType<Wheeze>(), item.damage / 2, 0, Main.myPlayer);
			}

			if (ToxicExtract && Main.rand.NextBool(5) && item.IsMagic())
				target.AddBuff(BuffID.Venom, 240);

			if (infernalFlame && item.IsMelee() && crit && Main.rand.NextBool(12))
				Projectile.NewProjectile(item.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<PhoenixProjectile>(), 50, 4, Main.myPlayer);

			if (crystalFlower && target.life <= 0 && Main.rand.NextBool(3))
				CrystalFlowerItem.OnKillEffect(item.GetSource_OnHit(target), Player, target, damage);
		}

		private void SpawnForbiddenTomeGhasts(IEntitySource src, NPC target, int damage, float knockback)
		{
			int count = 0;

			for (int i = 0; i < Main.maxProjectiles; ++i)
			{
				Projectile p = Main.projectile[i];

				if (p.active && p.owner == Player.whoAmI && p.friendly)
					count++;
			}

			if (count > 8)
				return;

			for (int i = 0; i < 20; i++)
			{
				Dust dust = Main.dust[Dust.NewDust(target.position, target.width, target.height, DustID.UltraBrightTorch, 0f, -2f, 117, new Color(0, 255, 142), .6f)];

				dust.noGravity = true;
				dust.position.X += ((Main.rand.Next(-50, 51) / 20) - 1.5f);
				if (dust.position != target.Center)
					dust.velocity = target.DirectionTo(dust.position) * 6f;
			}

			if (Main.myPlayer == Player.whoAmI)
			{
				int upperClamp = (int)MathHelper.Clamp(target.lifeMax, 0, 75);
				var vel = new Vector2(Main.rand.Next(-6, 6), Main.rand.Next(-5, -1));
				int dmg = (int)MathHelper.Clamp(damage / 2, 1, upperClamp);

				Projectile.NewProjectile(src, target.position, vel, ModContent.ProjectileType<GhastSkullFriendly>(), dmg, knockback, Player.whoAmI);
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			foreach (var effect in effects)
				effect.PlayerOnHitNPCWithProj(Player, proj, target, damage, knockback, crit);

			if (shadowFang)
			{
				if (target.life <= target.lifeMax / 2 && Main.rand.NextBool(7))
				{
					Projectile.NewProjectile(proj.GetSource_OnHit(target), target.position, Vector2.Zero, ModContent.ProjectileType<ShadowSingeProj>(), proj.damage / 3 * 2, 4, Main.myPlayer);
					Player.statLife -= 3;
				}
			}

			if (throwerGlove && proj.IsRanged())
				throwerStacks++;

			if (midasTouch)
				target.AddBuff(BuffID.Midas, 240);

			if (forbiddenTome)
			{
				if (target.life <= 0 && !target.SpawnedFromStatue)
					SpawnForbiddenTomeGhasts(proj.GetSource_OnHit(target), target, proj.damage, knockback);
			}

			if (geodeRanged && proj.IsRanged() && Main.rand.NextBool(24))
			{
				target.AddBuff(BuffID.Frostburn, 180);
				target.AddBuff(BuffID.OnFire, 180);
				target.AddBuff(BuffID.CursedInferno, 180);
			}

			if (bloodfireSet && proj.IsMagic())
			{
				if (Main.rand.NextBool(15))
					target.AddBuff(ModContent.BuffType<BloodCorrupt>(), 180);

				if (Main.rand.NextBool(30))
				{
					Player.statLife += 2;
					Player.HealEffect(2);
				}
			}

			if (wheezeScale && Main.rand.NextBool(9) && proj.IsMelee())
			{
				Vector2 vel = new Vector2(0, -1);
				float rand = Main.rand.NextFloat() * 6.283f;
				vel = vel.RotatedBy(rand);
				vel *= 8f;
				Projectile.NewProjectile(proj.GetSource_OnHit(target), target.Center, vel, ModContent.ProjectileType<Wheeze>(), Main.hardMode ? 40 : 20, 0, Player.whoAmI);
			}

			if (timScroll && proj.IsMagic())
			{
				switch (Main.rand.Next(12))
				{
					case 0:
						target.AddBuff(BuffID.OnFire, 120);
						break;
					case 1:
						target.AddBuff(BuffID.Confused, 120);
						break;
					case 2:
						target.AddBuff(BuffID.Frostburn, 120);
						break;
					default:
						break;
				}
			}

			if (winterbornCharmMage && Main.rand.NextBool(9))
				target.AddBuff(ModContent.BuffType<MageFreeze>(), 180);

			if (ToxicExtract && Main.rand.NextBool(5) && proj.IsMagic())
				target.AddBuff(BuffID.Venom, 240);

			if (infernalFlame && proj.IsMelee() && crit && Main.rand.NextBool(8))
				Projectile.NewProjectile(proj.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<PhoenixProjectile>(), 50, 4, Player.whoAmI);

			if (NebulaPearl && Main.rand.NextBool(8) && proj.IsMagic())
				Item.NewItem(proj.GetSource_OnHit(target), target.Hitbox, 3454);

			if (crystalFlower && target.life <= 0 && (Main.rand.NextBool(3) || proj.type == ModContent.ProjectileType<CrystalFlowerProjectile>()))
				CrystalFlowerItem.OnKillEffect(proj.GetSource_OnHit(target), Player, target, damage);
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
		{
			if (bubbleTimer > 0)
				return false;
			
			if (AnimeSword)
				return false;

			if (Player.GetModPlayer<DashPlayer>().ActiveDash == DashType.Shinigami)
				return false;

			int index = Player.FindBuffIndex(ModContent.BuffType<PhantomVeil>());
			if (index >= 0)
			{
				Player.DelBuff(index);
				Items.Glyphs.VeilGlyph.Block(Player);
				veilCounter = 0;

				return false;
			}
			return true;
		}

		public override void ModifyScreenPosition()
		{
			Main.screenPosition.Y += Main.rand.Next(-Shake, Shake) * ModContent.GetInstance<SpiritClientConfig>().ScreenShake;
			Main.screenPosition.X += Main.rand.Next(-Shake, Shake) * ModContent.GetInstance<SpiritClientConfig>().ScreenShake;
			if (Shake > 0) { Shake--; }
		}

		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			foreach (var effect in effects)
				effect.PlayerHurt(Player, pvp, quiet, damage, hitDirection, crit);

			veilCounter = 0;

			if (glyph == GlyphType.Daze && Main.rand.NextBool(2))
				Player.AddBuff(BuffID.Confused, 180);

			if (rogueSet && !Player.HasBuff<RogueCooldown>())
			{
				Player.AddBuff(BuffID.Invisibility, 260);
				Player.AddBuff(ModContent.BuffType<RogueCooldown>(), 1520);
			}

			if (leatherSet)
			{
				concentratedCooldown = 360;
				concentrated = false;
			}

			if (cryoSet)
			{
				cryoTimer = 0;
				SoundEngine.PlaySound(SoundID.Item50, Player.position);
			}

			if (ChaosCrystal && Main.rand.NextBool(4))
			{
				bool canSpawn = false;
				int teleportStartX = (int)(Main.LocalPlayer.position.X / 16) - 35;
				int teleportRangeX = 70;
				int teleportStartY = (int)(Main.LocalPlayer.position.Y / 16) - 35;
				int teleportRangeY = 70;
				Vector2 vector2 = TestTeleport(ref canSpawn, teleportStartX, teleportRangeX, teleportStartY, teleportRangeY);

				if (canSpawn)
				{
					Vector2 newPos = vector2;
					Main.LocalPlayer.Teleport(newPos, 2, 0);
					Main.LocalPlayer.velocity = Vector2.Zero;
					SoundEngine.PlaySound(SoundID.Item27, Player.Center);
					SoundEngine.PlaySound(SoundID.Item8, Player.Center);
					if (Main.netMode == NetmodeID.Server)
					{
						RemoteClient.CheckSection(Main.myPlayer, Main.LocalPlayer.position, 1);
						NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, Main.myPlayer, newPos.X, newPos.Y, 3, 0, 0);
					}
				}
			}

			if (infernalSet && Main.rand.NextBool(10))
				Projectile.NewProjectile(Player.GetSource_OnHurt(null), Player.position, new Vector2(0, -2), ModContent.ProjectileType<InfernalBlast>(), 50, 7, Main.myPlayer);

			if (starCharm)
			{
				int amount = Main.rand.Next(4, 6);
				for (int i = 0; i < amount; ++i)
				{
					Vector2 position = new Vector2(Player.position.X + Player.width * 0.5f + Main.rand.Next(-200, 201), Player.Center.Y - 600f);
					position.X = (position.X * 10f + Player.position.X) / 11f + Main.rand.Next(-100, 101);
					position.Y -= 150;

					float speedX = Player.position.X + Player.width * 0.5f + Main.rand.Next(-200, 201) - position.X;
					float speedY = Player.Center.Y - position.Y;

					if (speedY < 0f)
						speedY *= -1f;

					if (speedY < 20f)
						speedY = 20f;

					float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
					length = 12 / length;

					speedX *= length;
					speedY *= length;
					speedX += Main.rand.Next(-40, 41) * 0.03f;
					speedY += Main.rand.Next(-40, 41) * 0.03f;
					speedX *= Main.rand.Next(75, 150) * 0.01f;

					position.X += Main.rand.Next(-50, 51);
					Projectile.NewProjectile(Player.GetSource_OnHurt(null), position, new Vector2(speedX, speedY), ModContent.ProjectileType<Starshock1>(), 35, 1, Player.whoAmI);
				}
			}

			if (starMap && Main.rand.NextBool(2))
			{
				int amount = Main.rand.Next(2, 3);
				for (int i = 0; i < amount; ++i)
				{
					Vector2 position = new Vector2(Player.position.X + Player.width * 0.5f + Main.rand.Next(-300, 301), Player.Center.Y - 800f);
					position.X = (position.X * 10f + Player.position.X) / 11f + Main.rand.Next(-100, 101);
					position.Y -= 150;

					float speedX = Player.position.X + Player.width * 0.5f + Main.rand.Next(-200, 201) - position.X;
					float speedY = Player.Center.Y - position.Y;

					if (speedY < 0f)
						speedY *= -1f;

					if (speedY < 30f)
						speedY = 30f;

					float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
					length = 12 / length;

					speedX *= length;
					speedY *= length;
					speedX += Main.rand.Next(-40, 41) * 0.03f;
					speedY += Main.rand.Next(-40, 41) * 0.03f;
					speedX *= Main.rand.Next(75, 150) * 0.01f;

					position.X += Main.rand.Next(-10, 11);
					int p = Projectile.NewProjectile(Player.GetSource_OnHurt(null), position, new Vector2(speedX, speedY), ModContent.ProjectileType<Starshock1>(), 24, 1, Player.whoAmI);
					Main.projectile[p].timeLeft = 600;
				}
			}
		}

		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			if (soulPotion && Main.rand.NextBool(5))
				Projectile.NewProjectile(Player.GetSource_OnHurt(null), Player.Center, Vector2.Zero, ModContent.ProjectileType<SoulPotionWard>(), 0, 0f, Main.myPlayer);

			if (spiritBuff && Main.rand.NextBool(3))
				Projectile.NewProjectile(Player.GetSource_OnHurt(null), Player.Center, new Vector2(6, 6), ModContent.ProjectileType<StarSoul>(), 40, 0f, Main.myPlayer);
		}


		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (Player.HasBuff(ModContent.BuffType<FateBlessing>()))
			{
				Player.ClearBuff(ModContent.BuffType<FateBlessing>());

				Player.statLife = 500;
				Projectile.NewProjectile(Player.GetSource_OnHurt(null), Player.position.X, Player.position.Y, 0, 0, ModContent.ProjectileType<Shockwave>(), 0, 0, Player.whoAmI);

				return false;
			}

			if (illusionistEye && illusionistTimer <= 0)
			{
				for (int index3 = 0; index3 < 100; ++index3)
				{
					NPC npc = Main.npc[index3];
					if (!npc.boss)
					{
						SoundEngine.PlaySound(SoundID.Zombie54);
						illusionistTimer = 36000;
						Player.statLife += 20;

						for (int i = 0; i < 12; i++)
						{
							int num = Dust.NewDust(Player.position, Player.width, Player.width, DustID.UltraBrightTorch, 0f, -2f, 0, new Color(0, 255, 142), 2f);
							Main.dust[num].noGravity = true;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].scale *= .25f;
							if (Main.dust[num].position != Player.Center)
								Main.dust[num].velocity = Player.DirectionTo(Main.dust[num].position) * 6f;
						}

						Player.grappling[0] = -1;
						Player.grapCount = 0;

						for (int i = 0; i < Main.maxProjectiles; i++)
						{
							if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].aiStyle == 7)
								Main.projectile[i].Kill();
						}

						bool wasImmune = Player.immune;
						int immuneTime = Player.immuneTime;

						Player.Spawn(PlayerSpawnContext.ReviveFromDeath);
						Player.immune = wasImmune;
						Player.immuneTime = immuneTime;

						for (int i = 0; i < 6; i++)
						{
							int num = Dust.NewDust(Player.position, Player.width, Player.width, DustID.UltraBrightTorch, 0f, -2f, 0, new Color(0, 255, 142), 2f);
							Main.dust[num].noGravity = true;
							Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
							Main.dust[num].scale *= .25f;
							if (Main.dust[num].position != Player.Center)
								Main.dust[num].velocity = Player.DirectionTo(Main.dust[num].position) * 6f;
						}
					}
				}
			}

			if (clatterboneSet && clatterboneTimer <= 0)
			{
				Player.AddBuff(ModContent.BuffType<Sturdy>(), 21600);

				Rectangle textPos = new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 60, Main.LocalPlayer.width, Main.LocalPlayer.height);
				CombatText.NewText(textPos, new Color(100, 240, 0, 100), "Sturdy Activated!");

				Player.statLife += (int)damage;
				Projectile.NewProjectile(Player.GetSource_OnHurt(null), Player.position.X, Player.position.Y, 0, 0, ModContent.ProjectileType<Shockwave>(), 0, 0, Player.whoAmI);
				clatterboneTimer = 21600; // 6 minute timer.
				return false;
			}

			if (damageSource.SourceOtherIndex == 8)
				CustomDeath(ref damageSource);
			return true;
		}

		private void CustomDeath(ref PlayerDeathReason reason)
		{
			if (Player.FindBuffIndex(ModContent.BuffType<BurningRage>()) >= 0)
				reason = PlayerDeathReason.ByCustomReason(Player.name + " was consumed by Rage.");
		}

		int shroomtimer;
		int bloodTimer;
		int MinifishTimer = 0;

		/// <summary>Helper method that checks how far underwater the player is, continuously. If a tile above the player is not watered enough but is solid, it will still count as submerged.</summary>
		/// <param name="tileDepth">Depth in tiles for the player to be under.</param>
		public bool Submerged(int tileDepth, out int realDepth, bool countRealDepth = false)
		{
			realDepth = 0;

			if (!Collision.WetCollision(Player.position, Player.width, 8))
				return false;

			Point tPos = Player.Center.ToTileCoordinates();
			for (int i = 0; i < tileDepth; ++i)
			{
				realDepth = i;

				if (!WorldGen.InWorld(tPos.X, tPos.Y - i))
					return true;

				if (!countRealDepth && WorldGen.SolidTile(tPos.X, tPos.Y - i))
					return true; //Fully submerged to the point where the player should not be able to breathe
				else if (countRealDepth && WorldGen.SolidTile(tPos.X, tPos.Y - i))
					continue;

				if (Framing.GetTileSafely(tPos.X, tPos.Y - i).LiquidAmount < 255)
					return false;
			}
			return true;
		}

		public bool Submerged(int tileDepth) => Submerged(tileDepth, out int _);

		public override void PreUpdate()
		{
			int x1 = (int)Player.Center.X / 16;
			int y1 = (int)Player.Center.Y / 16;
			var config = ModContent.GetInstance<SpiritClientConfig>();

			if ((Player.ZoneSnow || Player.ZoneSkyHeight) && !Player.behindBackWall && Main.rand.NextBool(27))
			{
				Vector2 spawnPos = new Vector2(Player.Center.X + 8 * Player.direction, Player.Center.Y - 2f);
				if (Player.sleeping.isSleeping)
					spawnPos = new Vector2(Player.Center.X + ((Player.direction == -1) ? 20 : -14), Player.Center.Y - 6f);

				int d = Dust.NewDust(spawnPos, Player.width, 10, ModContent.DustType<FrostBreath>(), 1.5f * Player.direction, 0f, 100, default, Main.rand.NextFloat(.20f, 0.75f));
				Main.dust[d].velocity.Y *= 0f;
			}

			if (Player.ZoneDesert && Player.ZoneOverworldHeight && Main.rand.NextBool(33))
			{
				int index = Dust.NewDust(Player.position, Player.width + 4, Player.height + 2, DustID.Wet, 0.0f, 0.0f, 50, default, 0.8f);
				if (Main.rand.NextBool(2))
					Main.dust[index].alpha += 50;
				if (Main.rand.NextBool(2))
					Main.dust[index].alpha += 50;
				Main.dust[index].noLight = true;
				Main.dust[index].velocity *= .12f;
				Main.dust[index].velocity += Player.velocity;
			}

			if (!Player.ZoneOverworldHeight)
			{
				if (Framing.GetTileSafely(x1, y1 + 1).WallType == 62 && Framing.GetTileSafely(x1, y1).WallType == 62)
					ZoneSpider = true;
				else
					ZoneSpider = false;

				if (Framing.GetTileSafely(x1, y1 + 1).WallType == 178 && Framing.GetTileSafely(x1, y1).WallType == 178)
					inMarble = true;
				else
					inMarble = false;

				if (Framing.GetTileSafely(x1, y1 + 1).WallType == 180 && Framing.GetTileSafely(x1, y1).WallType == 180)
					inGranite = true;
				else
					inGranite = false;
			}

			if (mushroomPotion)
			{
				shroomtimer++;
				if (shroomtimer >= 20 && Player.velocity != Vector2.Zero)
				{
					shroomtimer = 0;
					int proj = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X, Player.Center.Y, 0, 0, ProjectileID.Mushroom, 10, 1, Main.myPlayer, 0.0f, 1);
					Main.projectile[proj].timeLeft = 120;
				}
			}

			if (bloodCourtHead)
			{
				bloodTimer++;
				if (bloodTimer >= 40)
				{
					bloodTimer = 0;
					Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X + Main.rand.Next(-30, 30), Player.Center.Y - Main.rand.Next(40, 50), Main.rand.Next(-1, 1), -1, ModContent.ProjectileType<BloodRuneEffect>(), 0, 0, Main.myPlayer, 0.0f, 1);
				}
			}

			if (MyWorld.meteorShowerWeather && Main.rand.NextBool(270) && Player.ZoneAsteroid())
			{
				float num12 = Main.rand.Next(-30, 30);
				float num14 = (float)Math.Sqrt(num12 * num12 + 100 * 100);
				float num15 = 10 / num14;
				float num16 = num12 * num15;
				float num17 = 100 * num15;
				float SpeedX = num16 + Main.rand.Next(-40, 41) * 0.02f;
				float SpeedY = num17 + Main.rand.Next(-40, 41) * 0.02f;
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X + Main.rand.Next(-1000, 1000), Player.Center.Y + Main.rand.Next(-1200, -900), SpeedX, SpeedY, ModContent.ProjectileType<Meteor>(), 30, 3, Main.myPlayer, 0.0f, 1);
			}

			if (!throwerGlove)
				throwerStacks = 0;

			if (shieldCore && Player.whoAmI == Main.myPlayer)
			{
				int shieldCount = 2;
				int type = ModContent.ProjectileType<InterstellarShield>();

				if (Player.ownedProjectileCounts[type] < shieldCount)
				{
					for (int i = 0; i < shieldCount; i++)
					{
						Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<InterstellarShield>(), 0, 0, Player.whoAmI, i * 360);
						proj.ai[1] = -(InterstellarShield.cooldownTime * InterstellarShield.rechargeRate);
						proj.netUpdate = true;
					}
				}
			}

			//Randomly spawn floating asteroid debris without disrupting NPC spawn weight
			if (Player.ZoneAsteroid() && Player.whoAmI == Main.myPlayer && Player.active && !Player.dead && Main.rand.NextBool(64))
			{
				Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

				Rectangle spawnRect = screenRect;
				int padding = 80 * 16; //40 tiles in each direction
				spawnRect.Inflate(padding, padding);

				for (int i = 0; i < 20; i++)
				{
					Vector2 spawnPos = new Vector2(spawnRect.X + Main.rand.Next(spawnRect.Width), spawnRect.Y + Main.rand.Next(spawnRect.Height));

					var tilePos = spawnPos / 16;
					bool inWorldBounds = tilePos.X < (Main.maxTilesX - 40) && tilePos.X > 40 && tilePos.Y < (Main.maxTilesY - 40) && tilePos.Y > 40;
					
					if (!screenRect.Contains(spawnPos.ToPoint()) && inWorldBounds && !Collision.SolidCollision(spawnPos, 10, 10))
					{
						if (NPC.CountNPCS(ModContent.NPCType<AsteroidDebris>()) < 30)
						{
							static int GetNPCIndex() => Main.rand.NextBool(GoldDebris.Chance) ? ModContent.NPCType<GoldDebris>() : ModContent.NPCType<AsteroidDebris>();

							if (Main.netMode == NetmodeID.SinglePlayer)
							{
								NPC.NewNPC(Terraria.Entity.GetSource_NaturalSpawn(), (int)spawnPos.X, (int)spawnPos.Y, GetNPCIndex());
							}
							if (Main.netMode == NetmodeID.MultiplayerClient)
							{
								ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.SpawnNPCFromClient, 3);
								packet.Write(GetNPCIndex());
								packet.Write((int)spawnPos.X);
								packet.Write((int)spawnPos.Y);
								packet.Send();
							}
						}
						return;
					}
				}
			}

			if (Player.ZoneAsteroid() && MyWorld.stardustWeather && Main.netMode != NetmodeID.Server)
			{
				int d = Main.rand.Next(new int[] { 180, 226, 206 });

				if (Main.rand.NextBool(10))
				{
					int num3 = Main.rand.Next(Main.screenWidth + 1000) - 500;
					int num4 = (int)Main.screenPosition.Y - Main.rand.Next(50);
					if (Main.LocalPlayer.velocity.Y > 0.0)
						num4 -= (int)Main.LocalPlayer.velocity.Y;
					if (Main.rand.NextBool(5))
						num3 = Main.rand.Next(500) - 500;
					else if (Main.rand.NextBool(5))
						num3 = Main.rand.Next(500) + Main.screenWidth;
					if (num3 < 0 || num3 > Main.screenWidth)
						num4 += Main.rand.Next((int)(Main.screenHeight * 0.8)) + (int)(Main.screenHeight * 0.1);
					int num5 = num3 + (int)Main.screenPosition.X;
					int x = num5 / 16;
					int y = num4 / 16;

					if (Main.tile[x, y] != null)
					{
						if (Main.tile[x, y].WallType == 0)
						{
							Dust dust = Main.dust[Dust.NewDust(new Vector2(num5, num4), 10, 10, d, 0.0f, 0.0f, 0, new Color(), 1f)];
							dust.scale += Main.cloudAlpha * 0.2f;
							dust.velocity.Y = (float)(3.0 + Main.rand.Next(30) * 0.100000001490116);
							dust.velocity.Y *= dust.scale;
							if (!Main.raining)
								dust.velocity.X = (Main.windSpeedCurrent + Main.rand.Next(-10, 10) * 0.1f) + (float)(Main.windSpeedCurrent * Main.cloudAlpha * 10.0);
							else
							{
								dust.velocity.X = (float)(Math.Sqrt(Math.Abs(Main.windSpeedCurrent)) * Math.Sign(Main.windSpeedCurrent) * (Main.cloudAlpha + 0.5) * 25.0 + Main.rand.NextFloat() * 0.200000002980232 - 0.100000001490116);
								dust.velocity.Y *= 0.5f;
							}
							dust.velocity.Y *= (float)(1.0 + 0.300000011920929 * Main.cloudAlpha);
							dust.scale += Main.cloudAlpha * 0.2f;
							dust.velocity *= (float)(1.0 + Main.cloudAlpha * 0.5);
						}
					}
				}
			}

			if (Player.ZoneSkyHeight) //Meteor projectiles in the space biome
			{
				float rand = Main.rand.Next(12, 18);
				float num14 = (float)Math.Sqrt(rand * rand + 100 * 100);
				float num15 = 10 / num14;
				float num16 = rand * num15;
				float num17 = 100 * num15;
				float SpeedX = num16 + Main.rand.Next(-40, 41) * Main.windSpeedCurrent + (.01f * Main.windSpeedCurrent);
				float SpeedY = num17 + Main.rand.Next(-40, 41) * 0.02f;

				string[] bigDebris = { "SpaceDebris3", "SpaceDebris4", "Meteor" };
				string[] smallDebris = { "SpaceDebris1", "SpaceDebris2" };

				if (Player.ZoneAsteroid() && MyWorld.spaceJunkWeather && Main.rand.NextBool(59))
				{
					if (Main.rand.NextBool(7))
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X + Main.rand.Next(-1000, 1000), Player.Center.Y + Main.rand.Next(-1200, -900), SpeedX, SpeedY, Mod.Find<ModProjectile>(Main.rand.Next(bigDebris)).Type, 16, 3, Main.myPlayer, 0.0f, 1);
					else
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X + Main.rand.Next(-1000, 1000), Player.Center.Y + Main.rand.Next(-1200, -900), SpeedX, SpeedY, Mod.Find<ModProjectile>(Main.rand.Next(smallDebris)).Type, 7, 3, Main.myPlayer, 0.0f, 1);
				}

				if (MyWorld.rareStarfallEvent && Main.rand.NextBool(65))
				{
					if (Player.ZoneAsteroid())
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X + Main.rand.Next(-800, 800), Player.Center.Y + Main.rand.Next(-1000, -900), Main.rand.Next(26, 33) * Main.windSpeedCurrent, 4, ModContent.ProjectileType<Comet>(), 0, 3, Main.myPlayer, 0.0f, 1);
					else
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X + Main.rand.Next(-800, 800), Player.Center.Y + Main.rand.Next(-1000, -900), Main.rand.Next(26, 33) * Main.windSpeedCurrent, 4, ModContent.ProjectileType<Comet>(), 0, 3, Main.myPlayer, 0.0f, 1);
				}
			}

			if (Player.ZoneBriar() && !Main.raining && !MyWorld.downedReachBoss)
			{
				Main.cloudAlpha += .007f;
				if (Main.cloudAlpha >= .4f)
					Main.cloudAlpha = .4f;
				Player.fishingSkill -= 20;
			}

			if (illusionistEye)
			{
				illusionistTimer--;
				if (illusionistTimer == 0)
				{
					SoundEngine.PlaySound(SoundID.MaxMana);
					for (int i = 0; i < 6; i++)
					{
						int num = Dust.NewDust(Player.position, Player.width, Player.height, DustID.UltraBrightTorch, 0f, -2f, 0, new Color(0, 255, 142), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if (Main.dust[num].position != Player.Center)
							Main.dust[num].velocity = Player.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
			}

			if (fierySet)
				fierySetTimer--;
			else
				fierySetTimer = 480;
			if (surferSet)
				surferTimer--;
			else
				surferTimer = 330;

			if (fierySetTimer == 0)
			{
				SoundEngine.PlaySound(SoundID.MaxMana);
				for (int i = 0; i < 2; i++)
				{
					int num = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if (Main.dust[num].position != Player.Center)
						Main.dust[num].velocity = Player.DirectionTo(Main.dust[num].position) * 6f;
				}
			}

			if (marbleSet)
			{
				marbleJump--;
				marbleJustJumped = true;
			}

			if (marbleJump == 0)
			{
				SoundEngine.PlaySound(SoundID.MaxMana);
				for (int i = 0; i < 2; i++)
				{
					int num = Dust.NewDust(Player.position, Player.width, Player.height, DustID.FireworkFountain_Yellow, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if (Main.dust[num].position != Player.Center)
						Main.dust[num].velocity = Player.DirectionTo(Main.dust[num].position) * 6f;
				}
			}

			if (!marbleSet)
				marbleJustJumped = false;

			if ((Player.velocity.Y == 0f || Player.sliding || (Player.autoJump && Player.justJumped)) && marbleJustJumped)
				marbleJustJumped = true;

			if (graniteSet && stompCooldown > 0)
				stompCooldown--;

			if (!Main.dayTime && MyWorld.dayTimeSwitched)
			{
				candyInBowl = 2;
				candyFromTown.Clear();
			}

			if (Player.ZoneAsteroid())
				Main.numCloudsTemp = 0;

			if (ChildSafety.Disabled && config.LeafFall)
			{
				if (Main.rand.NextBool(6) && (Player.ZoneBriar() || MyWorld.calmNight) && Player.ZoneOverworldHeight && !Player.ZoneBeach && !Player.ZoneCorrupt && !Player.ZoneCrimson && !Player.ZoneJungle && !Player.ZoneHallow && !Player.ZoneSnow)
				{
					float goreScale = 0.01f * Main.rand.Next(20, 70);
					int a = Gore.NewGore(Player.GetSource_FromThis(), new Vector2(Player.Center.X + Main.rand.Next(-1000, 1000), Player.Center.Y + Main.rand.Next(-1000, -100)), new Vector2(Main.windSpeedCurrent * 3f, 0f), 911, goreScale);
					Main.gore[a].timeLeft = 15;
					Main.gore[a].rotation = 0f;
					Main.gore[a].velocity = new Vector2(Main.windSpeedCurrent * 40f, Main.rand.NextFloat(0.2f, 2f));
				}
				if (Main.rand.NextBool(9) && Main.netMode != NetmodeID.Server && (Player.ZoneBriar() || MyWorld.calmNight) && Player.ZoneOverworldHeight && !Player.ZoneBeach && 
					!Player.ZoneCorrupt && !Player.ZoneCrimson && !Player.ZoneJungle && !Player.ZoneHallow && !Player.ZoneSnow)
				{
					float goreScale = Main.rand.NextFloat(0.5f, 0.9f);
					int x = (int)(Main.windSpeedCurrent > 0 ? Main.screenPosition.X - 100 : Main.screenPosition.X + Main.screenWidth + 100);
					int y = (int)Main.screenPosition.Y + Main.rand.Next(-100, Main.screenHeight);
					int gore = Gore.NewGore(Player.GetSource_FromThis(), new Vector2(x, y), Vector2.Zero, Mod.Find<ModGore>("GreenLeaf").Type, goreScale);
					Main.gore[gore].rotation = 0f;
					Main.gore[gore].velocity.Y = Main.rand.NextFloat(1f, 3f);
				}
			}

			if (windEffect)
			{
				if (Main.windSpeedCurrent <= -.01f)
					Main.windSpeedCurrent = -.8f;

				if (Main.windSpeedCurrent >= .01f)
					Main.windSpeedCurrent = .8f;
			}

			SpeedMPH = CalculateSpeed();

			if (Player.whoAmI == Main.myPlayer)
			{
				var temp = glyph; //Store the previous tick glyph type
				if (!Player.HeldItem.IsAir)
				{
					glyph = Player.HeldItem.GetGlobalItem<GItem>().Glyph;
					if (glyph == GlyphType.None && Player.nonTorch >= 0 && Player.nonTorch != Player.selectedItem && !Player.inventory[Player.nonTorch].IsAir)
						glyph = Player.inventory[Player.nonTorch].GetGlobalItem<Items.GItem>().Glyph;
				}
				else
					glyph = GlyphType.None;

				if (Main.netMode == NetmodeID.MultiplayerClient && temp != glyph) //If the glyph type has changed, sync
				{
					ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.PlayerGlyph, 2);
					packet.Write((byte)Main.myPlayer);
					packet.Write((byte)glyph);
					packet.Send();
				}
			}

			if (glyph == GlyphType.Bee)
				Player.AddBuff(BuffID.Honey, 2);
			else if (glyph == GlyphType.Phase)
				Player.GetModPlayer<DashPlayer>().UpdateShift();
			else if (glyph == GlyphType.Veil)
			{
				veilCounter++;
				if (veilCounter >= 8 * 60)
				{
					veilCounter = 0;
					Player.AddBuff(ModContent.BuffType<PhantomVeil>(), 2);
				}
			}
			else if (glyph == GlyphType.Void)
				Items.Glyphs.VoidGlyph.DevouringVoid(Player);
			else if (glyph == GlyphType.Radiant)
			{
				divineCounter++;
				if (divineCounter >= 90)
				{
					divineCounter = 0;
					Player.AddBuff(ModContent.BuffType<DivineStrike>(), 2);
				}
			}

			if (Player.HeldItem.type == Mod.Find<ModItem>("Minifish").Type)
				MinifishTimer--;
			else
				MinifishTimer = 120;

			if (Main.netMode != NetmodeID.MultiplayerClient && !Main.dedServ)
			{
				Vector2 zoom = Main.GameViewMatrix.Zoom;

				foreach (NPC npc in Main.npc)
				{
					if (!npc.active || npc.type != ModContent.NPCType<AuroraStag>())
						continue;

					var auroraStag = (AuroraStag)npc.ModNPC;
					if (auroraStag.Scared)
						continue;

					Rectangle npcBox = npc.getRect();
					npcBox.Inflate((int)zoom.X, (int)zoom.Y);

					if ((int)(Vector2.Distance(Player.Center, npc.Center) / 16) < 8 && npcBox.Contains(Main.MouseWorld.ToPoint()))
						hoveredStag = auroraStag;
				}

				if (hoveredStag != null)
				{
					Rectangle npcBox = hoveredStag.NPC.getRect();
					npcBox.Inflate((int)zoom.X, (int)zoom.Y);

					if ((int)(Vector2.Distance(Player.Center, hoveredStag.NPC.Center) / 16) > 8 || !npcBox.Contains(Main.MouseWorld.ToPoint()))
						hoveredStag = null;
				}
			}

			if (AnimeSword)
				Player.maxFallSpeed = 2000f;
		}

		private float CalculateSpeed()
		{
			//Mimics the Stopwatch accessory
			float slice = Player.velocity.Length();
			int count = (int)(1f + slice * 6f);
			if (count > phaseSlice.Length)
				count = phaseSlice.Length;

			for (int i = count - 1; i > 0; i--)
				phaseSlice[i] = phaseSlice[i - 1];

			phaseSlice[0] = slice;
			float inverse = 1f / count;
			float sum = 0f;
			for (int n = 0; n < phaseSlice.Length; n++)
			{
				if (n < count)
					sum += phaseSlice[n];
				else
					phaseSlice[n] = sum * inverse;
			}

			sum *= inverse;
			float boost = sum * (216000 / 42240f);
			if (!Player.merman && !Player.ignoreWater)
			{
				if (Player.honeyWet)
					boost *= .25f;
				else if (Player.wet)
					boost *= .5f;
			}

			return (float)Math.Round(boost);
		}

		public override void UpdateBadLifeRegen()
		{
			int before = Player.lifeRegen;
			bool drain = false;

			if (DoomDestiny)
			{
				drain = true;
				Player.lifeRegen -= 16;
			}

			if (blazeBurn)
			{
				drain = true;
				Player.lifeRegen -= 10;
			}

			if (drain && before > 0)
			{
				Player.lifeRegenTime = 0;
				Player.lifeRegen -= before;
			}
		}

		public override void UpdateLifeRegen()
		{
			if (glyph == GlyphType.Sanguine)
				Player.lifeRegen += 4;
		}

		public override void UpdateEquips()
		{
			Player.wingTimeMax = (int)(Player.wingTimeMax * WingTimeMaxMultiplier);
			if (Player.manaFlower)
				Player.manaFlower = !StarjinxSet;
		}

		public override void PostUpdateEquips()
		{
			Player.GetModPlayer<DashPlayer>().DashMovement();

			if (Player.ownedProjectileCounts[ModContent.ProjectileType<MiningHelmet>()] < 1 && Player.head == 11)
				Projectile.NewProjectile(Terraria.Entity.GetSource_NaturalSpawn(), Player.Center.X, Player.Center.Y, 0f, 0f, ModContent.ProjectileType<MiningHelmet>(), 0, 0, Player.whoAmI);

			if (graniteSet)
			{
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<EnergyStomp>()] > 0)
				{
					Player.noKnockback = true;
					Player.noFallDmg = true;
					Player.velocity.Y = 40f * Player.gravDir;
					Player.maxFallSpeed = 50f;

					Player.armorEffectDrawShadow = true;
				}
			}

			if (Main.mouseRight && magnifyingGlass && Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].damage <= 0)
				Player.scope = true;

			if (leatherSet)
			{
				if (concentratedCooldown == 0)
					SoundEngine.PlaySound(SoundID.MaxMana);

				concentratedCooldown -= (Player.velocity.X == 0f) ? 2 : 1;
			}
			else
			{
				concentrated = false;
				concentratedCooldown = 420;
			}

			if (concentratedCooldown <= 0)
				concentrated = true;

			if (concentrated && leatherSet)
				Yoraiz0rEye();

			if (bloodCourtHead)
				BloodCourtEye();

			if (clatterboneShield)
			{
				clatterStacks = 0;
				for (int i = 0; i < 200; i++)
				{
					if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy)
					{
						int distance = (int)Main.npc[i].Distance(Player.Center);
						if (distance < 480)
							clatterStacks++;

						for (int k = 0; k < clatterStacks; k++)
						{
							if (Main.rand.NextBool(4))
							{
								int d = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + Player.height - 4f), Player.width, 2, DustID.Dirt, 0f, 0f, 100, default, .64f);
								Main.dust[d].noGravity = true;
							}
						}
					}
				}

				if (clatterStacks >= 5)
					clatterStacks = 5;
			}
			else
				clatterStacks = 0;

			if (astralSet)
			{
				Player.GetAttackSpeed(DamageClass.Melee) += .06f * astralSetStacks;
				Player.manaCost -= .06f * astralSetStacks;
				Player.lifeRegen += 1 * astralSetStacks;
				Player.manaRegen += 1 * astralSetStacks;
				astralSetStacks = 0;
				for (int i = 0; i < 200; i++)
				{
					if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy)
					{
						int distance = (int)Main.npc[i].Distance(Player.Center);
						if (distance < 240)
							astralSetStacks++;
						Vector2 center = Player.Center;
						float num8 = Player.miscCounter / 40f;
						float num7 = 1.0471975512f * 2;
						for (int k = 0; k < astralSetStacks; k++)
						{
							int num6 = Dust.NewDust(center, 0, 0, DustID.UnusedWhiteBluePurple, 0f, 0f, 100, default, 1.3f);
							Main.dust[num6].noGravity = true;
							Main.dust[num6].velocity = Vector2.Zero;
							Main.dust[num6].noLight = true;
							Main.dust[num6].position = center + (num8 * MathHelper.TwoPi + num7 * i).ToRotationVector2() * 12f;
						}
					}
				}

				if (astralSetStacks >= 3)
					astralSetStacks = 3;
			}

			if (bismiteShield)
			{
				bismiteShieldStacks = 0;
				for (int i = 0; i < 200; i++)
				{
					if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].HasBuff(ModContent.BuffType<FesteringWounds>()) && Main.npc[i].type != NPCID.TargetDummy)
					{
						int distance = (int)Main.npc[i].Distance(Player.Center);
						if (distance < 480)
							bismiteShieldStacks++;

						for (int k = 0; k < bismiteShieldStacks; k++)
						{
							if (Main.rand.NextBool(6))
							{
								int d = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Plantera_Green, 0f, 0f, 100, default, .45f * bismiteShieldStacks);
								Main.dust[d].noGravity = true;
							}
						}
					}
				}

				Player.statDefense += (Player.GetSpiritPlayer().bismiteShieldStacks * 2);
				if (bismiteShieldStacks >= 5)
					bismiteShieldStacks = 5;
			}

			if (frigidGloves)
			{
				frigidGloveStacks = 0;
				for (int i = 0; i < 200; i++)
				{
					if (Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].type != NPCID.TargetDummy)
					{
						int distance = (int)Main.npc[i].Distance(Player.Center);
						if (distance < 320)
							frigidGloveStacks++;

						for (int k = 0; k < frigidGloveStacks; k++)
						{
							if (Main.rand.NextBool(6))
							{
								int d = Dust.NewDust(Player.position, Player.width, Player.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, .45f * bismiteShieldStacks);
								Main.dust[d].noGravity = true;
							}
						}
					}
				}

				if (frigidGloveStacks >= 5)
					frigidGloveStacks = 5;
			}

			if (bloodfireShield)
			{
				if (Player.lifeRegen >= 0)
					Player.lifeRegen = 0;
			};

			if (Player.controlUp && scarabCharm)
			{
				Player.noFallDmg = true;
				if (Player.gravDir == -1.0f)
				{
					Player.itemRotation = -Player.itemRotation;
					Player.itemLocation.Y = (Player.position.Y + Player.height + (Player.position.Y - Player.itemLocation.Y));
					if (Player.velocity.Y < -2.0f)
						Player.velocity.Y = -2f;
				}
				else if (Player.velocity.Y > 2.0f)
					Player.velocity.Y = 2f;
			}

			if (glyph == GlyphType.Void)
				Player.endurance += .08f;

			if (phaseShift)
			{
				Player.noKnockback = true;
				Player.buffImmune[BuffID.Slow] = true;
				Player.buffImmune[BuffID.Chilled] = true;
				Player.buffImmune[BuffID.Frozen] = true;
				Player.buffImmune[BuffID.Webbed] = true;
				Player.buffImmune[BuffID.Stoned] = true;
				Player.buffImmune[BuffID.OgreSpit] = true;
				Player.buffImmune[BuffID.Confused] = true;

				int dust;
				if (Player.velocity.Y == 0f)
					dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + Player.height - 4f), Player.width, 8, ModContent.DustType<TemporalDust>(), 0f, 0f, 100, default, 1.4f);
				else
					dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (Player.height >> 1) - 8f), Player.width, 16, ModContent.DustType<TemporalDust>(), 0f, 0f, 100, default, 1.4f);

				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f + Main.rand.Next(20) * 0.01f;
			}

			if (clatterboneSet)
				clatterboneTimer--;

			// Update armor sets.
			if (infernalSet)
			{
				float percentageLifeLeft = (float)Player.statLife / Player.statLifeMax2;
				if (percentageLifeLeft <= 0.25f)
				{
					Player.statDefense -= 4;
					Player.manaCost += 0.25F;
					Player.GetDamage(DamageClass.Magic) += 0.5F;

					bool spawnProj = true;
					for (int i = 0; i < 1000; ++i)
					{
						if (Main.projectile[i].type == ModContent.ProjectileType<InfernalGuard>() && Main.projectile[i].owner == Player.whoAmI)
						{
							spawnProj = false;
							break;
						}
					}

					if (spawnProj)
					{
						for (int i = 0; i < 3; ++i)
						{
							Projectile proj = Projectile.NewProjectileDirect(Terraria.Entity.GetSource_NaturalSpawn(), Player.Center, Vector2.Zero, ModContent.ProjectileType<InfernalGuard>(), 0, 0, Player.whoAmI, 90, 1);
							proj.localAI[1] = 2f * MathHelper.Pi / 3f * i;
						}
					}

					Player.AddBuff(ModContent.BuffType<InfernalRage>(), 2);
					infernalSetCooldown = 60;
				}
			}

			if (infernalSetCooldown > 0)
				infernalSetCooldown--;

			if (runicSet)
				RunicHood.SpawnRunicRunes(Player);

			if (darkfeatherVisage)
				DarkfeatherVisage.SpawnDarkfeatherBombs(Player);

			if (spiritSet)
			{
				if (Main.rand.NextBool(5))
				{
					int num = Dust.NewDust(Player.position, Player.width, Player.height, DustID.AncientLight, 0f, 0f, 0, default, 1f);
					Main.dust[num].noGravity = true;
				}

				if (Player.statLife >= 400)
				{
					Player.GetDamage(DamageClass.Melee) += 0.08f;
					Player.GetDamage(DamageClass.Magic) += 0.08f;
					Player.GetDamage(DamageClass.Summon) += 0.08f;
					Player.GetDamage(DamageClass.Ranged) += 0.08f;
				}
				else if (Player.statLife >= 200)
					Player.statDefense += 6;
				else if (Player.statLife >= 50)
					Player.lifeRegenTime += 5;
				else if (Player.statLife > 0)
					Player.noKnockback = true;
			}

			if (cryoSet && Player.ownedProjectileCounts[ModContent.ProjectileType<CryoProj>()] <= 1)
				Projectile.NewProjectile(Player.GetSource_NaturalSpawn(), Player.position, Vector2.Zero, ModContent.ProjectileType<CryoProj>(), 0, 0, Player.whoAmI);

			if (SoulStone && Player.ownedProjectileCounts[ModContent.ProjectileType<StoneSpirit>()] < 1 && Main.rand.NextBool(2))
				Projectile.NewProjectile(Player.GetSource_NaturalSpawn(), Player.position, Vector2.Zero, ModContent.ProjectileType<StoneSpirit>(), 35, 0, Player.whoAmI);

			if (duskSet && Player.ownedProjectileCounts[ModContent.ProjectileType<ShadowCircleRune1>()] <= 0)
				Projectile.NewProjectile(Player.GetSource_NaturalSpawn(), Player.position, Vector2.Zero, ModContent.ProjectileType<ShadowCircleRune1>(), 18, 0, Player.whoAmI);

			if (infernalDash > 0)
				infernalDash--;

			/*if (Player.dashDelay < 0)
			{
				for (int l = 0; l < 0; l++)
				{
					int num14;
					if (Player.velocity.Y == 0f)
						num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + Player.height - 4f), Player.width, 8, DustID.Smoke, 0f, 0f, 100, default, 1.4f);
					else
						num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (Player.height / 2) - 8f), Player.width, 16, DustID.Smoke, 0f, 0f, 100, default, 1.4f);
					Main.dust[num14].velocity *= 0.1f;
					Main.dust[num14].scale *= 1f + Main.rand.Next(20) * 0.01f;
					Main.dust[num14].shader = GameShaders.Armor.GetSecondaryShader(Player.shoe, Player);

					int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 0, default, 1f);
					Main.dust[dust].scale *= 10f;

					int dust2 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 0, default, 1f);
					Main.dust[dust2].scale *= 10f;

					int dust3 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 0, default, 1f);
					Main.dust[dust3].scale *= 10f;
				}

				for (int l = 0; l < 0; l++)
				{
					int dust;
					if (Player.velocity.Y == 0f)
						dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + Player.height - 4f), Player.width, 8, DustID.Smoke, 0f, 0f, 100, default, 1.4f);
					else
						dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (Player.height / 2) - 8f), Player.width, 16, DustID.Smoke, 0f, 0f, 100, default, 1.4f);
					Main.dust[dust].velocity *= 0.1f;
					Main.dust[dust].scale *= 1f + Main.rand.Next(20) * 0.01f;
					Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(Player.shoe, Player);
				}

				Player.vortexStealthActive = false;

				//float maxSpeed = Math.Max(Player.accRunSpeed, Player.maxRunSpeed);

				//if (Player.velocity.X > 12f || Player.velocity.X < -12f)
				//{
				//	Player.velocity.X = Player.velocity.X * 0.985f;
				//	return;
				//}

				//if (Player.velocity.X > maxSpeed || Player.velocity.X < -maxSpeed)
				//{
				//	Player.velocity.X = Player.velocity.X * 0.94f;
				//	return;
				//}

				//Player.dashDelay = 30;

				//if (Player.velocity.X < 0f)
				//{
				//	Player.velocity.X = -maxSpeed;
				//	return;
				//}

				//if (Player.velocity.X > 0f)
				//{
				//	Player.velocity.X = maxSpeed;
				//	return;
				//}
			}

			if (infernalDash > 0)
				infernalDash--;

			// Update accessories.
			if (infernalShield)
			{
				if (infernalDash > 0)
					infernalDash--;
				else
					infernalHit = -1;

				if (infernalDash > 0 && infernalHit < 0)
				{
					int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 0, default, 1f);
					Main.dust[dust].scale *= 2f;

					int dust2 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 0, default, 1f);
					Main.dust[dust2].scale *= 2f;

					int dust3 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 0, default, 1f);
					Main.dust[dust3].scale *= 2f;

					var rectangle = new Rectangle((int)(Player.position.X + Player.velocity.X * 0.5 - 4.0), (int)(Player.position.Y + Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
					for (int i = 0; i < 200; i++)
					{
						if (Main.npc[i].active && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
						{
							NPC npc = Main.npc[i];
							Rectangle rect = npc.getRect();
							if (rectangle.Intersects(rect) && (npc.noTileCollide || Collision.CanHit(Player.position, Player.width, Player.height, npc.position, npc.width, npc.height)))
							{
								float damage = Player.GetDamage(DamageClass.Melee).ApplyTo(70);

								float knockback = 12f;
								if (Player.kbGlove)
									knockback *= 2f;

								if (Player.kbBuff)
									knockback *= 1.5f;

								bool crit = false;
								if (Main.rand.Next(100) < Player.GetCritChance(DamageClass.Melee))
									crit = true;

								int hitDirection = Player.direction;
								if (Player.velocity.X < 0f)
									hitDirection = -1;

								if (Player.velocity.X > 0f)
									hitDirection = 1;

								if (Player.whoAmI == Main.myPlayer)
								{
									npc.AddBuff(ModContent.BuffType<StackingFireBuff>(), 600);
									npc.StrikeNPC((int)damage, knockback, hitDirection, crit, false, false);

									if (Main.netMode != NetmodeID.SinglePlayer)
										NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, i, damage, knockback, hitDirection, 0, 0, 0);
								}

								infernalDash = 10;
								Player.dashDelay = 30;
								Player.velocity.X = -hitDirection * 1f;
								Player.velocity.Y = -4f;
								Player.immune = true;
								Player.immuneTime = 2;
								infernalHit = i;
							}
						}
					}
				}

				if (Player.dash <= 0 && Player.dashDelay == 0 && !Player.mount.Active)
				{
					int num21 = 0;
					bool spawnDust = false;

					if (Player.dashTime > 0)
						Player.dashTime--;

					if (Player.dashTime < 0)
						Player.dashTime++;

					if (Player.controlRight && Player.releaseRight)
					{
						if (Player.dashTime > 0)
						{
							num21 = 1;
							spawnDust = true;
							Player.dashTime = 0;
						}
						else
							Player.dashTime = 15;
					}
					else if (Player.controlLeft && Player.releaseLeft)
					{
						if (Player.dashTime < 0)
						{
							num21 = -1;
							spawnDust = true;
							Player.dashTime = 0;
						}
						else
							Player.dashTime = -15;
					}

					if (spawnDust)
					{
						int dust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 0, default, 1f);
						Main.dust[dust].scale *= 2f;

						int dust2 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, 0f, 0, default, 1f);
						Main.dust[dust2].scale *= 2f;

						Player.velocity.X = 15.5f * num21;

						Point point3 = (Player.Center + new Vector2(num21 * Player.width / 2 + 2, Player.gravDir * -Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
						Point point4 = (Player.Center + new Vector2(num21 * Player.width / 2 + 2, 0f)).ToTileCoordinates();

						if (WorldGen.SolidOrSlopedTile(point3.X, point3.Y) || WorldGen.SolidOrSlopedTile(point4.X, point4.Y))
							Player.velocity.X = Player.velocity.X / 2f;

						Player.dashDelay = -1;
						infernalDash = 15;

						for (int num22 = 0; num22 < 0; num22++)
						{
							int num23 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
							Main.dust[num23].position.X = Main.dust[num23].position.X + Main.rand.Next(-5, 6);
							Main.dust[num23].position.Y = Main.dust[num23].position.Y + Main.rand.Next(-5, 6);
							Main.dust[num23].velocity *= 0.2f;
							Main.dust[num23].scale *= 1f + Main.rand.Next(20) * 0.01f;
							Main.dust[num23].shader = GameShaders.Armor.GetSecondaryShader(Player.shield, Player);
						}
					}
				}
			}*/

			if (bubbleTimer > 0)
				bubbleTimer--;

			if (soulSiphon > 0)
			{
				Player.lifeRegenTime += 2;

				int num = (5 + soulSiphon) / 2;
				Player.lifeRegenTime += num;
				Player.lifeRegen += num;

				soulSiphon = 0;
			}

			if (drakomireMount)
			{
				Player.statDefense += 40;
				Player.noKnockback = true;

				if (Player.dashDelay > 0)
					Player.dashDelay--;
				else
				{
					int num4 = 0;
					bool flag = false;

					if (Player.dashTime > 0)
						Player.dashTime--;
					else if (Player.dashTime < 0)
						Player.dashTime++;

					if (Player.controlRight && Player.releaseRight)
					{
						if (Player.dashTime > 0)
						{
							num4 = 1;
							flag = true;
							Player.dashTime = 0;
						}
						else
							Player.dashTime = 15;
					}
					else if (Player.controlLeft && Player.releaseLeft)
					{
						if (Player.dashTime < 0)
						{
							num4 = -1;
							flag = true;
							Player.dashTime = 0;
						}
						else
							Player.dashTime = -15;
					}

					if (flag)
					{
						Player.velocity.X = 16.9f * num4;
						Point point = Utils.ToTileCoordinates(Player.Center + new Vector2(num4 * Player.width / 2 + 2, Player.gravDir * -Player.height / 2f + Player.gravDir * 2f));
						Point point2 = Utils.ToTileCoordinates(Player.Center + new Vector2(num4 * Player.width / 2 + 2, 0f));

						if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
							Player.velocity.X = Player.velocity.X / 2f;

						Player.dashDelay = 600;
					}
				}

				if (Player.velocity.X != 0f && Player.velocity.Y == 0f)
				{
					drakomireFlameTimer += (int)Math.Abs(Player.velocity.X);
					if (drakomireFlameTimer >= 15)
					{
						Vector2 vector = Player.Center + new Vector2(26 * -(float)Player.direction, 26f * Player.gravDir);
						Projectile.NewProjectile(Player.GetSource_FromThis(), vector.X, vector.Y, 0f, 0f, ModContent.ProjectileType<DrakomireFlame>(), Player.statDefense / 2, 0f, Player.whoAmI, 0f, 0f);
						drakomireFlameTimer = 0;
					}
				}

				if (Main.rand.NextBool(10))
				{
					Vector2 vector2 = Player.Center + new Vector2(-48 * Player.direction, -6f * Player.gravDir);
					if (Player.direction == -1)
						vector2.X -= 20f;

					Dust.NewDust(vector2, 16, 16, DustID.Torch, 0f, 0f, 0, default, 1f);
				}
			}

			if (oakHeartStacks > 0)
				oakHeartStacks -= 0.025f;
		}

		public override void PostUpdateRunSpeeds()
		{
			//Adjust speed here to also affect mounted speed.
			float speed = 1f;
			float sprint = 1f;
			float accel = 1f;
			float slowdown = 1f;

			if (glyph == GlyphType.Frost)
				sprint += .05f;

			if (phaseShift)
			{
				speed += 0.55f;
				sprint += 0.55f;
				accel += 3f;
				slowdown += 3f;
			}

			if (stoneplate)
			{
				speed -= .15f;
				sprint -= .15f;
				accel -= .3f;
			}

			Player.maxRunSpeed *= speed;
			Player.accRunSpeed *= sprint;
			Player.runAcceleration *= accel;
			Player.runSlowdown *= slowdown;

			if (oldHelios)
			{
				Player.maxRunSpeed *= 1.2f;
				Player.runAcceleration *= 1.8f;
				Player.maxFallSpeed *= 20;
			}
		}

		public override void PostUpdate()
		{
			if (starjinxtimer > 0)
				starjinxtimer--;

			//kinda scuffed way of implementing stars on shoot but overriding the actual shoot hook does not work with vanilla weapons that fire projectiles in unique ways(ie most magic weapons), also has benefit of working with channel weapons
			if (Player.HeldItem.IsMagic() && Player.HasBuff(Mod.Find<ModBuff>("ManajinxBuff").Type) && starjinxtimer <= 0 && Player.HeldItem.damage > 0 && Player.itemTime > 0)
			{
				SoundEngine.PlaySound(SoundID.Item9, Player.Center);
				starjinxtimer = Player.HeldItem.useTime + 10; //nerf the effect on fast firing weapons since they're typically busted and also benefit more from ichor
				Vector2 toMouse = Vector2.Normalize(Main.MouseWorld - Player.Center) * 8f;
				toMouse = toMouse.RotatedByRandom(MathHelper.Pi / 8);
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, toMouse, ModContent.ProjectileType<ManajinxStar>(), Player.HeldItem.damage / 3, Player.HeldItem.knockBack, Player.whoAmI, 0, Main.rand.Next(10));
			}

			if (seaSnailVenom && Player.miscCounter % 5 == 0 && Player.velocity.X != 0f && Player.velocity.Y == 0f && !Player.mount.Active)
				Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X - 10 * Player.direction, Player.Center.Y + 5, 0f, 20f, ModContent.ProjectileType<Sea_Snail_Poison_Projectile>(), 5, 0f, Player.whoAmI, 0f, 0f);

			if (moonlightSack)
			{
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && Player.DistanceSQ(projectile.Center) <= 240 * 240 && projectile.owner == Player.whoAmI && projectile.minion && projectile.type != ModContent.ProjectileType<Moonlight_Sack_Lightning>())
					{
						if (Player.miscCounter % 5 == 0)
						{
							int pickedProjectile = ModContent.ProjectileType<Moonlight_Sack_Lightning>();
							Vector2 vector2_2 = Vector2.Normalize(Player.Center - projectile.Center) * 8f;
							int p = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X, Player.Center.Y, vector2_2.X, vector2_2.Y, pickedProjectile, (int)(Player.GetDamage(DamageClass.Summon).ApplyTo(12)), 1f, Player.whoAmI, 0.0f, 0.0f);
							Main.projectile[p].ai[0] = projectile.whoAmI;
						}
					}
				}
			}

			if (teslaCoil)
				TeslaStrike(Player);

			foreach (var effect in removedEffects)
				if (!effects.Contains(effect)) effect.EffectRemoved(Player);

			foreach (var effect in effects)
				effect.PlayerPostUpdate(Player);

			if (Player.ZoneBriar() && Player.wet && Main.expertMode && !MyWorld.downedReachBoss)
				Player.AddBuff(BuffID.Poisoned, 120);

			if (cryoSet)
			{
				cryoTimer += .5f;
				if (cryoTimer >= 450)
					cryoTimer = 450;
			}
			else
				cryoTimer = 0;

			if (surferSet && surferTimer == 0)
			{
				var textPos = new Rectangle((int)Player.position.X, (int)Player.position.Y - 20, Player.width, Player.height);
				CombatText.NewText(textPos, new Color(121, 195, 237, 100), "Water Spout Charged!");
			}
		}

		private void TeslaStrike(Player player)
		{
			attackTimer++;

			int npcsHit = 0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC npc = Main.npc[i];

				if (npc.active && player.DistanceSQ(npc.Center) <= 300f * 300f && npc.CanBeChasedBy())
				{
					if (attackTimer % 90 == 0)
					{
						int p = Projectile.NewProjectile(Player.GetSource_FromThis(), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<Items.Accessory.UnstableTeslaCoil.Unstable_Tesla_Coil_Projectile>(), 18, 0f, player.whoAmI, 0.0f, 0.0f);
						Main.projectile[p].ai[0] = npc.position.X;
						Main.projectile[p].ai[1] = npc.position.Y;
						Main.projectile[p].netUpdate = true;
					}

					if (npcsHit++ > 3)
						break;
				}
			}
		}

		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			foreach (var effect in effects)
				effect.PlayerModifyHitNPC(Player, item, target, ref damage, ref knockback, ref crit);

			if (CursedPendant && Main.rand.NextBool(5))
				target.AddBuff(BuffID.CursedInferno, 180);

			if (duskSet && item.IsMagic() && Main.rand.NextBool(4))
				target.AddBuff(BuffID.ShadowFlame, 300);

			if (primalSet && item.IsMelee() && Main.rand.NextBool(2))
				target.AddBuff(ModContent.BuffType<Afflicted>(), 120);

			if (moonGauntlet && item.IsMelee())
			{
				if (Main.rand.NextBool(4))
					target.AddBuff(BuffID.CursedInferno, 180);

				if (Main.rand.NextBool(4))
					target.AddBuff(BuffID.Ichor, 180);

				if (Main.rand.NextBool(4))
					target.AddBuff(BuffID.Daybreak, 180);

				if (Main.rand.NextBool(8))
					Player.AddBuff(ModContent.BuffType<OnyxWind>(), 120);
			}

			if (starBuff && crit && Main.rand.NextBool(10))
				for (int i = 0; i < 3; ++i)
					if (Main.myPlayer == Player.whoAmI)
						Projectile.NewProjectile(item.GetSource_OnHit(target), target.Center.X + Main.rand.Next(-140, 140), target.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), ProjectileID.HallowStar, 40, 3, Player.whoAmI);

			if (poisonPotion && crit)
				target.AddBuff(ModContent.BuffType<FesteringWounds>(), 180);

			if (runeBuff && item.IsMagic())
			{
				if (Main.rand.NextBool(10))
				{
					for (int h = 0; h < 3; h++)
					{
						float rand = Main.rand.NextFloat() * MathHelper.TwoPi;
						Vector2 vel = new Vector2(0, -1).RotatedBy(rand) * 8f;
						Projectile.NewProjectile(item.GetSource_OnHit(target), target.Center - new Vector2(10f, 10f), vel, ModContent.ProjectileType<Rune>(), 27, 1, Player.whoAmI);
					}
				}
			}

			if (concentrated)
			{
				for (int i = 0; i < 40; i++)
				{
					int dust = Dust.NewDust(target.Center, target.width, target.height, DustID.GoldCoin);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].noGravity = true;

					Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
					vector2_1.Normalize();

					Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();

					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = target.Center - vector2_3;
				}

				damage = (int)(damage * 1.2f);
				crit = true;
				concentrated = false;
				concentratedCooldown = 300;
			}
		}

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			AddBuffWithCondition(poisonPotion && crit, target, ModContent.BuffType<FesteringWounds>(), 180);
			AddBuffWithCondition(primalSet && Main.rand.NextBool(2) && (proj.IsMagic() || proj.IsMelee()), target, ModContent.BuffType<Afflicted>(), 120);
			AddBuffWithCondition(duskSet && proj.IsMagic() && Main.rand.NextBool(4), target, BuffID.ShadowFlame, 300);

			if (moonGauntlet && proj.IsMelee())
			{
				if (Main.rand.NextBool(4))
					target.AddBuff(BuffID.CursedInferno, 180);
				if (Main.rand.NextBool(4))
					target.AddBuff(BuffID.Ichor, 180);
				if (Main.rand.NextBool(4))
					target.AddBuff(BuffID.Daybreak, 180);
				if (Main.rand.NextBool(8))
					Player.AddBuff(ModContent.BuffType<OnyxWind>(), 120);
			}

			if (runeBuff && proj.IsMagic() && Main.rand.NextBool(10))
			{
				for (int h = 0; h < 3; h++)
				{
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 8f;
					Projectile.NewProjectile(proj.GetSource_OnHit(target), target.Center.X - 10, target.Center.Y - 10, vel.X, vel.Y, ModContent.ProjectileType<Projectiles.Magic.Rune>(), 27, 1, Player.whoAmI);
				}
			}

			if (starBuff && crit && Main.rand.NextBool(10) && Main.myPlayer == Player.whoAmI)
				for (int i = 0; i < 3; ++i)
					Projectile.NewProjectile(proj.GetSource_OnHit(target), target.Center.X + Main.rand.Next(-140, 140), target.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), ProjectileID.HallowStar, 40, 3, Player.whoAmI);

			if (concentrated)
			{
				for (int i = 0; i < 40; i++)
				{
					int dust = Dust.NewDust(target.Center, target.width, target.height, DustID.GoldCoin);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].noGravity = true;

					Vector2 velocity = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101))) * (Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = velocity;
					velocity.Normalize();

					Vector2 vector2_3 = velocity * 34f;
					Main.dust[dust].position = target.Center - vector2_3;
				}

				damage = (int)(damage * 1.2F);
				crit = true;
				concentrated = false;
				concentratedCooldown = 300;
			}
		}

		private static void AddBuffWithCondition(bool condition, NPC p, int id, int ticks) { if (condition) p.AddBuff(id, ticks); }

		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
		{
			if (npc.whoAmI == infernalHit)
				damage = 0;
		}

		public void Yoraiz0rEye()
		{
			int index = 0 + Player.bodyFrame.Y / 56;
			if (index >= Main.OffsetsPlayerHeadgear.Length)
				index = 0;

			Vector2 vector2_1 = Vector2.Zero;
			if (Player.mount.Active && Player.mount.Cart)
			{
				int sign = Math.Sign(Player.velocity.X);
				if (sign == 0)
					sign = Player.direction;

				vector2_1 = new Vector2(MathHelper.Lerp(0.0f, -8f, Player.fullRotation / 0.7853982f), MathHelper.Lerp(0.0f, 2f, Math.Abs(Player.fullRotation / 0.7853982f))).RotatedBy(Player.fullRotation, new Vector2());
				if (sign == Math.Sign(Player.fullRotation))
					vector2_1 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(Player.fullRotation / 0.7853982f));
			}

			Vector2 spinningpoint1 = new Vector2(3 * Player.direction - (Player.direction == 1 ? 1 : 0), -11.5f * Player.gravDir) + Vector2.UnitY * Player.gfxOffY + Player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
			Vector2 spinningpoint2 = new Vector2(3 * Player.shadowDirection[1] - (Player.direction == 1 ? 1 : 0), -11.5f * Player.gravDir) + Player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
			if (Player.fullRotation != 0.0)
			{
				spinningpoint1 = spinningpoint1.RotatedBy(Player.fullRotation, Player.fullRotationOrigin);
				spinningpoint2 = spinningpoint2.RotatedBy(Player.fullRotation, Player.fullRotationOrigin);
			}

			float offset = 0.0f;
			if (Player.mount.Active)
				offset = Player.mount.PlayerOffset;

			Vector2 vector2_2 = Player.position + spinningpoint1 + vector2_1;
			vector2_2.Y -= offset / 2f;

			Vector2 vector2_3 = Player.oldPosition + spinningpoint2 + vector2_1;
			vector2_3.Y -= offset / 2f;

			int num3 = (int)Vector2.Distance(vector2_2, vector2_3) / 3 + 1;
			if (Vector2.Distance(vector2_2, vector2_3) % 3.0 != 0.0)
				++num3;

			for (float num4 = 1f; num4 <= (double)num3; ++num4)
			{
				Dust dust = Main.dust[Dust.NewDust(Player.Center, 0, 0, DustID.GoldCoin, 0.0f, 0.0f, 0, new Color(), 1f)];
				dust.position = Vector2.Lerp(vector2_3, vector2_2, num4 / num3);
				dust.noGravity = true;
				dust.velocity = Vector2.Zero;
				dust.customData = Player;
				dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cYorai, Player);
			}
		}

		public void BloodCourtEye()
		{
			int index = 0 + Player.bodyFrame.Y / 56;
			if (index >= Main.OffsetsPlayerHeadgear.Length)
				index = 0;

			Vector2 vector2_1 = Vector2.Zero;
			if (Player.mount.Active && Player.mount.Cart)
			{
				int num = Math.Sign(Player.velocity.X);
				if (num == 0)
					num = Player.direction;

				vector2_1 = new Vector2(MathHelper.Lerp(0.0f, -8f, Player.fullRotation / 0.7853982f), MathHelper.Lerp(0.0f, 2f, Math.Abs(Player.fullRotation / 0.7853982f))).RotatedBy(Player.fullRotation, new Vector2());
				if (num == Math.Sign(Player.fullRotation))
					vector2_1 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(Player.fullRotation / 0.7853982f));
			}

			Vector2 spinningpoint1 = new Vector2(3 * Player.direction - (Player.direction == 1 ? 1 : 0), -11.5f * Player.gravDir) + Vector2.UnitY * Player.gfxOffY + Player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
			Vector2 spinningpoint2 = new Vector2(3 * Player.shadowDirection[1] - (Player.direction == 1 ? 1 : 0), -11.5f * Player.gravDir) + Player.Size / 2f + Main.OffsetsPlayerHeadgear[index];
			if (Player.fullRotation != 0.0)
			{
				spinningpoint1 = spinningpoint1.RotatedBy(Player.fullRotation, Player.fullRotationOrigin);
				spinningpoint2 = spinningpoint2.RotatedBy(Player.fullRotation, Player.fullRotationOrigin);
			}

			float offset = 0.0f;
			if (Player.mount.Active)
				offset = Player.mount.PlayerOffset;

			Vector2 vector2_2 = Player.position + spinningpoint1 + vector2_1;
			vector2_2.Y -= offset / 2f;

			Vector2 vector2_3 = Player.oldPosition + spinningpoint2 + vector2_1;
			vector2_3.Y -= offset / 2f;

			int num3 = (int)Vector2.Distance(vector2_2, vector2_3) / 4 + 1;
			if (Vector2.Distance(vector2_2, vector2_3) % 3.0 != 0.0)
				++num3;

			for (float num4 = 1f; num4 <= (double)num3; ++num4)
			{
				Dust dust = Main.dust[Dust.NewDust(Player.Center, 0, 0, ModContent.DustType<NightmareDust>(), 0.0f, 0.0f, 0, new Color(), 1f)];
				dust.position = Vector2.Lerp(vector2_3, vector2_2, num4 / num3);
				dust.noGravity = true;
				dust.velocity = Vector2.Zero;
				dust.customData = Player;
				dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cYorai, Player);
			}
		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			if (strikeshield)
			{
				npc.AddBuff(ModContent.BuffType<Buffs.SummonTag.SummonTag3>(), 300, true);
				int num = -1;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					if (Main.npc[i].CanBeChasedBy(Player, false) && Main.npc[i] == npc)
						num = i;
				}

				Player.MinionAttackTargetNPC = num;

				for (int i = 0; i < 10; i++)
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Dirt, 2.5f, -2.5f, 0, Color.Gray, 0.7f);
				npc.StrikeNPCNoInteraction(30, 4f, 0, false, false, false);

			}

			if (bismiteShield)
				npc.AddBuff(ModContent.BuffType<FesteringWounds>(), 300);

			if (basiliskMount)
			{
				int num = Player.statDefense / 2;
				npc.StrikeNPCNoInteraction(num, 0f, 0, false, false, false);
			}

		}

		internal bool CanTrickOrTreat(NPC npc)
		{
			if (!npc.townNPC)
				return false;

			string fullName;
			if (npc.ModNPC == null)
				fullName = "Terraria:" + npc.TypeName;
			else
				fullName = npc.ModNPC.Mod.Name + ":" + npc.TypeName;

			if (candyFromTown.Contains(fullName))
				return false;

			candyFromTown.Add(fullName);
			return true;
		}

		private static Vector2 TestTeleport(ref bool canSpawn, int teleportStartX, int teleportRangeX, int teleportStartY, int teleportRangeY)
		{
			Player player = Main.LocalPlayer;

			int repeats = 0;
			int num2 = 0;
			int num3 = 0;

			Vector2 Position = new Vector2(num2, num3) * 16f + new Vector2(-player.width / 2 + 8, -player.height);
			while (!canSpawn && repeats < 1000)
			{
				++repeats;

				int index1 = teleportStartX + Main.rand.Next(teleportRangeX);
				int index2 = teleportStartY + Main.rand.Next(teleportRangeY);
				Position = new Vector2(index1, index2) * 16f + new Vector2(-player.width / 2 + 8, -player.height);

				if (!Collision.SolidCollision(Position, player.width, player.height))
				{
					if ((Main.tile[index1, index2].WallType != 87 || index2 <= Main.worldSurface || NPC.downedPlantBoss) && (!Main.wallDungeon[Main.tile[index1, index2].WallType] || index2 <= Main.worldSurface || NPC.downedBoss3))
					{
						int num4 = 0;
						while (num4 < 100)
						{
							Tile tile = Main.tile[index1, index2 + num4];
							Position = new Vector2(index1, index2 + num4) * 16f + new Vector2(-player.width / 2 + 8, -player.height);
							Collision.SlopeCollision(Position, player.velocity, player.width, player.height, player.gravDir, false);

							bool flag = !Collision.SolidCollision(Position, player.width, player.height);

							if (flag)
								++num4;
							else if (!tile.HasTile || tile.IsActuated || !Main.tileSolid[tile.TileType])
								++num4;
							else
								break;
						}
						if (!Collision.LavaCollision(Position, player.width, player.height) && Collision.HurtTiles(Position, player.velocity, player.width, player.height, false).Y <= 0.0)
						{
							Collision.SlopeCollision(Position, player.velocity, player.width, player.height, player.gravDir, false);
							if (Collision.SolidCollision(Position, player.width, player.height) && num4 < 99)
							{
								Vector2 Velocity1 = Vector2.UnitX * 16f;
								if (!(Collision.TileCollision(Position - Velocity1, Velocity1, player.width, player.height, false, false, (int)player.gravDir) != Velocity1))
								{
									Vector2 Velocity2 = -Vector2.UnitX * 16f;
									if (!(Collision.TileCollision(Position - Velocity2, Velocity2, player.width, player.height, false, false, (int)player.gravDir) != Velocity2))
									{
										Vector2 Velocity3 = Vector2.UnitY * 16f;
										if (!(Collision.TileCollision(Position - Velocity3, Velocity3, player.width, player.height, false, false, (int)player.gravDir) != Velocity3))
										{
											Vector2 Velocity4 = -Vector2.UnitY * 16f;
											if (!(Collision.TileCollision(Position - Velocity4, Velocity4, player.width, player.height, false, false, (int)player.gravDir) != Velocity4))
											{
												canSpawn = true;
												break;
											}
										}
									}
								}
							}
						}
					}
				}
			}

			return Position;
		}

		public override void FrameEffects()
		{
			// Prevent potential bug with shot projectile detection.
			EndShotDetection();

			// Hide players wings, etc. when mounted
			if (Player.mount.Active)
			{
				int mount = Player.mount.Type;
				if (mount == ModContent.MountType<Drakomire>())
					Player.wings = -1;
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			foreach (var effect in effects)
				effect.PlayerDrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);

			if (HellGaze)
			{
				if (Main.rand.NextBool(4))
				{
					int dust = Dust.NewDust(Player.position, Player.width + 26, 30, DustID.Torch, Player.velocity.X, Player.velocity.Y, 100, default, 1f);
					drawInfo.DustCache.Add(dust);
				}

				r *= 0f;
				g *= 1f;
				b *= 0f;
				fullBright = true;
			}
		}

		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
		{
			if (stoneplate) //This causes the player to squint
			{
				Player.poisoned = true; //Workaround
				Player.eyeHelper.Update(Player);
				Player.poisoned = false;
			}

			if (camoCounter > 0)
			{
				float camo = 1 - .75f / CAMO_DELAY * camoCounter;
				drawInfo.colorArmorHead = Color.Multiply(drawInfo.colorArmorHead, camo);
				drawInfo.colorArmorBody = Color.Multiply(drawInfo.colorArmorBody, camo);
				drawInfo.colorArmorLegs = Color.Multiply(drawInfo.colorArmorLegs, camo);
				camo *= camo;
				drawInfo.colorHair = Color.Multiply(drawInfo.colorHair, camo);
				drawInfo.colorEyeWhites = Color.Multiply(drawInfo.colorEyeWhites, camo);
				drawInfo.colorEyes = Color.Multiply(drawInfo.colorEyes, camo);
				drawInfo.colorHead = Color.Multiply(drawInfo.colorHead, camo);
				drawInfo.colorBodySkin = Color.Multiply(drawInfo.colorBodySkin, camo);
				drawInfo.colorLegs = Color.Multiply(drawInfo.colorLegs, camo);
				drawInfo.colorShirt = Color.Multiply(drawInfo.colorShirt, camo);
				drawInfo.colorUnderShirt = Color.Multiply(drawInfo.colorUnderShirt, camo);
				drawInfo.colorPants = Color.Multiply(drawInfo.colorPants, camo);
				drawInfo.colorShoes = Color.Multiply(drawInfo.colorShoes, camo);
				//drawInfo.headGlowMask = Color.Multiply(drawInfo.headGlowMask, camo); //NEEDSUPDATING
				//drawInfo.bodyGlowMask = Color.Multiply(drawInfo.bodyGlowMask, camo);
				//drawInfo.armGlowMaskColor = Color.Multiply(drawInfo.armGlowMaskColor, camo);
				//drawInfo.legGlowMaskColor = Color.Multiply(drawInfo.legGlowMaskColor, camo);
			}
		}

		public void DoubleTapEffects(int keyDir)
		{
			if (keyDir == (Main.ReversedUpDownArmorSetBonuses ? 1 : 0))
			{
				if (jellynautHelm)
				{
					foreach (Projectile proj in Main.projectile)
					{
						if (proj.type == ModContent.ProjectileType<JellynautOrbiter>() && proj.owner == Player.whoAmI && Player.whoAmI == Main.myPlayer)
						{
							proj.timeLeft = Main.rand.Next(10, 30);
							proj.netUpdate = true;
						}
					}
				}
				
				if (starSet && !Player.HasBuff(ModContent.BuffType<StarCooldown>()))
				{
					Player.AddBuff(ModContent.BuffType<StarCooldown>(), StarMask.CooldownTime);
					SoundEngine.PlaySound(SoundID.Item92, Player.position);

					if (Player.whoAmI == Main.myPlayer)
					{
						int id = Projectile.NewProjectile(Player.GetSource_FromThis("DoubleTap"), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<EnergyFieldStarplate>(), 0, 0, Player.whoAmI);

						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.SyncProjectile, number: id);
					}
					for (int i = 0; i < 8; i++)
					{
						int num = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Electric, 0f, -2f, 0, default, .7f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if (Main.dust[num].position != Player.Center)
							Main.dust[num].velocity = Player.DirectionTo(Main.dust[num].position) * 6f;
					}
				}

				if (surferSet && surferTimer <= 0)
				{
					surferTimer = 420;
					SoundEngine.PlaySound(SoundID.Splash, Player.position);
					SoundEngine.PlaySound(SoundID.Item20, Player.position);
					Projectile.NewProjectile(Player.GetSource_FromThis("DoubleTap"), Player.Center - new Vector2(0, 30), Vector2.Zero, ModContent.ProjectileType<WaterSpout>(), (int)(Player.GetDamage(DamageClass.Magic).ApplyTo(30)), 8, Player.whoAmI);
				}

				int stompProj = ModContent.ProjectileType<EnergyStomp>();
				if (graniteSet && !Player.mount.Active && Player.velocity.Y != 0 && stompCooldown <= 0 && Player.ownedProjectileCounts[stompProj] < 1)
					Projectile.NewProjectile(Player.GetSource_FromThis("DoubleTap"), Player.Center, Vector2.Zero, stompProj, 10, 3, Player.whoAmI);

				if (fierySet && fierySetTimer <= 0)
				{
					SoundEngine.PlaySound(SoundID.Item74, Player.position);
					for (int i = 0; i < 8; i++)
					{
						int num = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Torch, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if (Main.dust[num].position != Player.Center)
							Main.dust[num].velocity = Player.DirectionTo(Main.dust[num].position) * 6f;
					}

					for (int projFinder = 0; projFinder < Main.maxProjectiles; ++projFinder)
					{
						if (Main.projectile[projFinder].sentry && Main.projectile[projFinder].active)
							Projectile.NewProjectile(Player.GetSource_FromThis("DoubleTap"), Main.projectile[projFinder].Center.X, Main.projectile[projFinder].Center.Y - 20, 0f, 0f, ModContent.ProjectileType<FierySetExplosion>(), Main.projectile[projFinder].damage, Main.projectile[projFinder].knockBack, Player.whoAmI);

						fierySetTimer = 480;
					}
				}

				if (bloodcourtSet && !Player.HasBuff(ModContent.BuffType<CourtCooldown>()) && Player.statLife > (int)(Player.statLifeMax * .08f))
					BloodCourtHead.DoubleTapEffect(Player);

				if (frigidSet && !Player.HasBuff(ModContent.BuffType<FrigidCooldown>()))
				{
					Vector2 mouse = Main.MouseScreen + Main.screenPosition;
					Projectile.NewProjectile(Player.GetSource_FromThis("DoubleTap"), mouse, Vector2.Zero, ModContent.ProjectileType<FrigidWall>(), 14, 8, Player.whoAmI);
					Player.AddBuff(ModContent.BuffType<FrigidCooldown>(), 500);
				}
			}
		}

		public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
		{
			if (StarjinxSet)
				healValue = 0;
		}

		public override void OnMissingMana(Item item, int neededMana)
		{
			if (StarjinxSet && Player.ownedProjectileCounts[ModContent.ProjectileType<Manajinx>()] == 0)
			{
				Vector2 spawnpos = Player.Center + Main.rand.NextVector2CircularEdge(Main.rand.Next(500, 600), Main.rand.Next(500, 600));
				int spawntries = 0;

				while ((WorldGen.SolidTile((int)spawnpos.X / 16, (int)spawnpos.Y / 16) || WorldGen.SolidTile3((int)spawnpos.X / 16, (int)spawnpos.Y / 16)) && spawntries < 50)
				{
					spawnpos = Player.Center + Main.rand.NextVector2CircularEdge(Main.rand.Next(500, 600), Main.rand.Next(500, 600));
					spawntries++;
				}

				if (spawntries < 50)
				{
					SoundEngine.PlaySound(SoundID.Item9, spawnpos);
					Projectile.NewProjectile(Player.GetSource_FromThis("DoubleTap"), spawnpos, Vector2.Zero, ModContent.ProjectileType<Manajinx>(), 0, 0, Player.whoAmI);
				}
			}
		}
	}
}