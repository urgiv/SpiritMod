using Microsoft.Xna.Framework;
using SpiritMod.Buffs.Glyph;
using SpiritMod.GlobalClasses.Items;
using SpiritMod.Items.Glyphs;
using SpiritMod.Particles;
using SpiritMod.Projectiles.Glyph;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.GlobalClasses.Players
{
	public class GlyphPlayer : ModPlayer
	{
		public GlyphType Glyph { get; set; }

		private int frenzyDamage;
		private float genericCounter;
		public float veilCounter;
		public bool zephyrStrike;

		public override void ResetEffects() => zephyrStrike = false;

		public override void PreUpdate()
		{
			if (Player.whoAmI == Main.myPlayer)
			{
				var temp = Glyph; //Store the previous tick glyph type
				if (!Player.HeldItem.IsAir)
				{
					if (Player.HeldItem.GetGlobalItem<GlyphGlobalItem>().randomGlyph)
					{
						const int chaosRate = 60 * 7;

						if ((Player.miscCounterNormalized % chaosRate) == 0)
							Player.HeldItem.GetGlobalItem<GlyphGlobalItem>().SetGlyph(Player.HeldItem, ChaosGlyph.Randomize(Glyph));
					} //Chaos glyph effect

					Glyph = Player.HeldItem.GetGlobalItem<GlyphGlobalItem>().Glyph;
					if (Glyph == GlyphType.None && Player.nonTorch >= 0 && Player.nonTorch != Player.selectedItem && !Player.inventory[Player.nonTorch].IsAir)
						Glyph = Player.inventory[Player.nonTorch].GetGlobalItem<GlyphGlobalItem>().Glyph;
				}
				else Glyph = GlyphType.None;

				if (temp != Glyph)
				{
					if (Main.netMode == NetmodeID.MultiplayerClient) //If the glyph type has changed, sync
					{
						ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.PlayerGlyph, 2);
						packet.Write((byte)Main.myPlayer);
						packet.Write((byte)Glyph);
						packet.Send();
					}
					genericCounter = 0;
				}
			}

			if (Glyph == GlyphType.Blaze && Player.velocity.Length() > 1.5f && Main.rand.NextBool(2))
				Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Torch, 0, 0, 0, default, Main.rand.NextFloat(1f, 2f)).noGravity = true;
			if (Glyph == GlyphType.Phase)
				genericCounter = MathHelper.Max(genericCounter - .01f, 0);
			if (Glyph == GlyphType.Veil && veilCounter > 0)
			{
				int shieldType = ModContent.ProjectileType<PhaseShield>();
				if (Player.ownedProjectileCounts[shieldType] < 1) //Spawn a shield visual
					Projectile.NewProjectile(null, Player.Center, Vector2.Zero, shieldType, 0, 0, Player.whoAmI);
			}
			if (Glyph == GlyphType.Radiant)
			{
				if ((genericCounter = MathHelper.Min(genericCounter + (1 / (Player.HeldItem.useTime * 3f)), 1)) == 1)
				{
					int radiantType = ModContent.BuffType<DivineStrike>();

					if (!Player.HasBuff(radiantType))
					{
						ParticleHandler.SpawnParticle(new StarParticle(Player.Center + new Vector2(0, -10 * Player.gravDir), Vector2.Zero, Color.White, Color.Yellow, .2f, 10, 0));
						SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);
					}
					Player.AddBuff(radiantType, 2);
				}
			}

			veilCounter = MathHelper.Max(veilCounter - .001f, 0);
		}

		public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (zephyrStrike)
				velocity *= StormGlyph.VelocityBoost;
		}

		public override bool CanUseItem(Item item)
		{
			if (Glyph == GlyphType.Storm && Main.rand.NextBool((int)MathHelper.Clamp(30 - (Player.HeldItem.useTime / 2), 2, 10)))
			{
				zephyrStrike = true;

				Vector2 velocity = Player.DirectionTo(Main.MouseWorld) * ((item.shootSpeed > 1) ? (item.shootSpeed * StormGlyph.VelocityBoost) : 12f);
				Projectile.NewProjectile(Player.GetSource_ItemUse(item), Player.Center, velocity, ModContent.ProjectileType<SlicingGust>(), item.damage, 12f, Player.whoAmI);
			}
			return base.CanUseItem(item);
		}

		#region hit overrides
		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit) => ModifyHitAnything(target, item, null, ref damage, ref knockback, ref crit);

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => ModifyHitAnything(target, null, proj, ref damage, ref knockback, ref crit);

		public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
		{
			float knockback = 0;
			ModifyHitAnything(target, item, null, ref damage, ref knockback, ref crit);
		}

		public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
		{
			float knockback = 0;
			ModifyHitAnything(target, null, proj, ref damage, ref knockback, ref crit);
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) => OnHitAnything(target, item, null, damage, knockback, crit);

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit) => OnHitAnything(target, null, proj, damage, knockback, crit);

		public override void OnHitPvp(Item item, Player target, int damage, bool crit) => OnHitAnything(target, item, null, damage, 0, crit);

		public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit) => OnHitAnything(target, null, proj, damage, 0, crit);
		#endregion

		private void OnHitAnything(Entity target, Item item, Projectile proj, int damage, float knockback, bool crit)
		{
			int life = (target is NPC npc) ? npc.life : ((target is Player player) ? player.statLife : 0);

			if (Glyph == GlyphType.Frost && Main.rand.NextBool((int)MathHelper.Clamp(30 - (Player.HeldItem.useTime / 2f), 2, 12)))
				FrostGlyph.FreezeEffect(Player, target, proj);
			if (Glyph == GlyphType.Void && Main.rand.NextBool((int)MathHelper.Clamp(30 - (Player.HeldItem.useTime / 2f), 2, 12)))
				VoidGlyph.VoidCollapse(Player, target, proj, damage);
			if (Glyph == GlyphType.Radiant)
			{
				genericCounter = 0;

				if (Player.HasBuff(ModContent.BuffType<DivineStrike>()))
					RadiantGlyph.RadiantStrike(Player, target);
			}

			if (target is NPC && ((target as NPC).value <= 0 || (target as NPC).SpawnedFromStatue || (target as NPC).friendly)) //Don't let useless NPCs trigger widely beneficial effects
				return;

			if (Glyph == GlyphType.Unholy && life <= 0)
				UnholyGlyph.Erupt(Player, target, damage / 3);
			if (Glyph == GlyphType.Sanguine)
				SanguineGlyph.DrainEffect(Player, target);
			if (Glyph == GlyphType.Blaze)
				Player.AddBuff(ModContent.BuffType<BurningRage>(), 120);
			if (Glyph == GlyphType.Bee)
			{
				if ((genericCounter = MathHelper.Clamp(genericCounter + (Player.HeldItem.useTime / 60f), 0, 1)) == 1)
				{
					BeeGlyph.ReleaseBees(Player, target, (int)(damage * .4f));
					genericCounter = 0;
				}
				if (life <= 0)
					BeeGlyph.HoneyEffect(Player);
			}
			if (Glyph == GlyphType.Phase)
			{
				if ((genericCounter = MathHelper.Clamp(genericCounter + (Player.HeldItem.useTime / 60f), 0, 1)) == 1)
					Player.AddBuff(ModContent.BuffType<TemporalShift>(), (int)MathHelper.Clamp(Player.HeldItem.useTime * 2f, 30, 60));
			}
			if (Glyph == GlyphType.Rage && life <= 0)
				frenzyDamage = Math.Max(damage - life, 0);
			if (Glyph == GlyphType.Veil)
				veilCounter = MathHelper.Clamp(veilCounter + (Player.HeldItem.useTime / 300f), 0, 1);
		}

		private void ModifyHitAnything(Entity target, Item item, Projectile proj, ref int damage, ref float knockback, ref bool crit)
		{
			if (Glyph == GlyphType.Rage)
			{
				if (frenzyDamage > 0)
				{
					damage += frenzyDamage;
					RageGlyph.RageEffect(Player, target, proj);
				}
				frenzyDamage = 0;
			}
		}

		public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
		{
			if (Glyph == GlyphType.Phase)
			{
				float boost = MathHelper.Clamp(0.005f * Player.GetModPlayer<MyPlayer>().SpeedMPH, 0, .7f);
				damage *= .8f + boost;
			}
			if (Glyph == GlyphType.Radiant && Player.HasBuff(ModContent.BuffType<DivineStrike>()))
				damage *= 1.5f;
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
		{
			if (veilCounter > 0)
			{
				float resistance = .5f; //resist 50% damage at full charge
				damage -= (int)(damage * resistance * veilCounter);
			}
			return true;
		}

		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
		{
			if (veilCounter > 0)
			{
				veilCounter = 0;

				for (int i = 0; i < (int)(20 * veilCounter); i++)
					Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Clentaminator_Cyan, 0, 0, 100).velocity = Vector2.UnitY * -2;
			}
		}
	}
}