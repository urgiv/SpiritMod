﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	/// <summary>Used to check if a player is currently in the starjinx event, and if so, set up visual effects.</summary>
	class StarjinxPlayer : ModPlayer
	{
		public bool zoneStarjinxEvent = false;
		public bool oldZoneStarjinx = false;

		public Vector2 StarjinxPosition;

		public override void ResetEffects()
		{
			oldZoneStarjinx = zoneStarjinxEvent;
			zoneStarjinxEvent = false;
		}

		public override void PostUpdateMiscEffects()
		{
			if (zoneStarjinxEvent)
				Player.AddBuff(ModContent.BuffType<HighGravityBuff>(), 2);

			if (Main.netMode != NetmodeID.Server)
			{
				Player.ManageSpecialBiomeVisuals("SpiritMod:StarjinxSky", zoneStarjinxEvent);
				SpiritMod.starjinxBorderEffect.Parameters["Radius"].SetValue(StarjinxMeteorite.EVENT_RADIUS);
				SpiritMod.starjinxBorderEffect.Parameters["NoiseTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
				SpiritMod.starjinxBorderShader.UseColor(new Color(230, 55, 166).ToVector3());
				Player.ManageSpecialBiomeVisuals("SpiritMod:StarjinxBorder", zoneStarjinxEvent, StarjinxPosition);
				Player.ManageSpecialBiomeVisuals("SpiritMod:StarjinxBorderFade", zoneStarjinxEvent, StarjinxPosition);
			}
		}
	}
}
