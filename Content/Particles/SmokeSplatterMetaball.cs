﻿using System;
using CalamityHunt.Common.Graphics.RenderTargets;
using CalamityHunt.Common.Systems.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityHunt.Content.Particles;

public class SmokeSplatterMetaball : Particle
{
    private int time;

    public int maxTime;

    private int style;

    private int direction;

    private float rotationalVelocity;

    public Func<Vector2> anchor;

    public Vector2 gravity;

    public Color fadeColor;

    public override void OnSpawn()
    {
        style = Main.rand.Next(5);
        direction = Main.rand.NextBool().ToDirectionInt();
        scale *= Main.rand.NextFloat(0.9f, 1.1f);
        rotationalVelocity = Main.rand.NextFloat(0.2f);
        maxTime = (int)(maxTime * 0.66f);
    }

    public override void Update()
    {
        float progress = (float)time / maxTime;

        velocity *= 0.97f;
        velocity += gravity * (0.5f + progress);

        if (time++ > maxTime) {
            ShouldRemove = true;
        }

        if (anchor != null) {
            position += anchor.Invoke();
        }

        rotationalVelocity *= 0.96f;
        rotation += (1f - MathF.Cbrt(progress)) * rotationalVelocity * direction;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        float progress = (float)time / maxTime;

        Texture2D texture = AssetDirectory.Textures.Particle[Type].Value;
        Rectangle frame = texture.Frame(1, 5, 0, style);
        SpriteEffects flip = direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
        Color scaleProgress = new Color(scale, progress, 0, 1);
        float drawScale = scale * MathF.Sqrt(Utils.GetLerpValue(0f, 6f, time, true)) * (0.4f + progress * 0.5f);

        Vector2 squish = new Vector2(1f - progress * 0.1f, 1f + progress * 0.1f);
        spriteBatch.Draw(texture, position - Main.screenPosition, frame, scaleProgress, rotation, frame.Size() * 0.5f, squish * drawScale * 0.5f, flip, 0);
    }
}
