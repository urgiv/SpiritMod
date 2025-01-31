using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class DistressJelly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Distressed Jelly");
			Main.npcFrameCount[NPC.type] = 6;
			Main.npcCatchable[NPC.type] = true;
			NPCID.Sets.CountsAsCritter[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 12;
			NPC.height = 20;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit25;
			NPC.DeathSound = SoundID.NPCDeath28;
            NPC.value = 0f;
			NPC.catchItem = (short)ModContent.ItemType<DistressJellyItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 64;
            NPC.scale = 1f;
			NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.rarity = 3;

            AIType = NPCID.Firefly;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
				new FlavorTextBestiaryInfoElement("A stray space rock has popped this Lunazoa's protective bubble. The stone prevents the bubble from reforming, causing them distress."),
			});
		}

		public override bool? CanBeHitByProjectile(Projectile projectile) => projectile.minion ? false : null;

		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 15; k++)
            {
                Dust d = Dust.NewDustPerfect(NPC.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(2), 0, default, 0.65f);
                d.noGravity = true;
            }

			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
				for (int i = 0; i < 5; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DistressJelly" + i).Type);
		}

		public override void AI()
        {
            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;
            Lighting.AddLight(NPC.Center, 0.075f * 2, 0.231f * 2, 0.255f * 2);
        }

        public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon(ItemID.Gel);

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			drawColor = NPC.GetNPCColorTintedByBuffs(drawColor);
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.spriteBatch.Draw(
                Mod.Assets.Request<Texture2D>("NPCs/MoonjellyEvent/DistressJelly_Glow").Value,
				NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY),
				NPC.frame,
				Color.White,
				NPC.rotation,
				NPC.frame.Size() / 2,
				NPC.scale,
				SpriteEffects.None,
				0
			);
            
        }
    }
}
