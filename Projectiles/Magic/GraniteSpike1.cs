using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class GraniteSpike1 : ModProjectile
	{
		public override string Texture => SpiritMod.EMPTY_TEXTURE;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Granite Spike");

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(10);
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 120;
			Projectile.alpha = 255;
		}

		public override bool PreAI()
		{
			for (int i = 0; i < 9; ++i)
			{
				float num1 = Projectile.velocity.X * 0.2f * (float)i;
				float num2 = Projectile.velocity.Y * -0.200000002980232f * i;
				int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0, 0, 100, default, .4f);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity = Vector2.Zero;
				Main.dust[index2].position.X -= num1;
				Main.dust[index2].position.Y -= num2;
			}

			Projectile.velocity.Y += Projectile.ai[0];
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;

			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue);
			
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Flare_Blue, Projectile.oldVelocity.X * 0.2f, Projectile.oldVelocity.Y * 0.2f);

			for (int i = 0; i < 4; i++)
			{
				float rotation = (float)(Main.rand.Next(180, 361) * (Math.PI / 180));
				Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				int proj = Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y,
					velocity.X, velocity.Y, ModContent.ProjectileType<GraniteShard1>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
				
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].velocity *= 4f;
			}
		}
	}
}