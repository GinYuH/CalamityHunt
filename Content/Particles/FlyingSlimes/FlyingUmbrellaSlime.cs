﻿using CalamityHunt.Common.Systems.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityHunt.Content.Particles.FlyingSlimes
{
    public class FlyingUmbrellaSlime : Particle
    {
        public int time;
        public float distanceFade;

        public override void OnSpawn()
        {
            scale *= Main.rand.NextFloat(0.9f, 1.1f);
        }

        public override void Update()
        {
            if (data is Vector2)
            {
                velocity = Vector2.Lerp(velocity, position.DirectionTo((Vector2)data).SafeNormalize(Vector2.Zero) * 7f, 0.2f);
                if (position.Distance((Vector2)data) < 10)
                    if (position.Distance((Vector2)data) < 10)
                    {
                        Active = false;
                        Gore.NewGore(Entity.GetSource_None(), position, Main.rand.NextVector2Circular(1, 1), 314);
                    }
                distanceFade = Utils.GetLerpValue(20, 80, position.Distance((Vector2)data), true);
            }
            else
                distanceFade = 1f;

            time++;

            if (time > 500)
                Active = false;

            velocity *= 0.98f;
            rotation = velocity.ToRotation();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            float fadeIn = Utils.GetLerpValue(0, 30, time, true) * distanceFade;

            for (int i = 0; i < 10; i++)
                spriteBatch.Draw(texture.Value, position - velocity * i * 0.5f - Main.screenPosition, null, color.MultiplyRGBA(Lighting.GetColor(position.ToTileCoordinates())) * fadeIn * ((10f - i) / 100f), rotation + MathHelper.PiOver2, texture.Size() * 0.5f, scale * distanceFade * 1.05f, 0, 0);

            spriteBatch.Draw(texture.Value, position - Main.screenPosition, null, color.MultiplyRGBA(Lighting.GetColor(position.ToTileCoordinates())) * fadeIn, rotation + MathHelper.PiOver2, texture.Size() * 0.5f, scale * distanceFade, 0, 0);
        }
    }
}
