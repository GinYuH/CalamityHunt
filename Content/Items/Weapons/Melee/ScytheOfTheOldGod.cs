﻿using CalamityHunt.Common.Systems;
using CalamityHunt.Content.Bosses.Goozma;
using CalamityHunt.Content.Items.Rarities;
using CalamityHunt.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityHunt.Content.Items.Weapons.Melee
{
    public class ScytheOfTheOldGod : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.HasAProjectileThatHasAUsabilityCheck[Type] = true;
            ItemID.Sets.gunProj[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 128;
            Item.height = 128;
            Item.damage = 6200;
            Item.DamageType = DamageClass.Melee;
            Item.rare = ModContent.RarityType<VioletRarity>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.sellPrice(gold: 20);
            Item.shoot = ModContent.ProjectileType<ScytheOfTheOldGodHeld>();
            Item.shootSpeed = 5f;
            if (ModLoader.HasMod("CalamityMod"))
            {
                ModRarity r;
                Mod calamity = ModLoader.GetMod("CalamityMod");
                calamity.TryFind<ModRarity>("Violet", out r);
                Item.rare = r.Type;
            }
            swingStyle = 0;

        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "Glow").Value;
            Color glowColor = new GradientColor(SlimeUtils.GoozOilColors, 0.33f, 0.33f).Value;
            glowColor.A = 0;
            spriteBatch.Draw(glow, position, frame, glowColor, 0, origin, scale, 0, 0);
            spriteBatch.Draw(glow, position, frame, glowColor * 0.5f, 0, origin, scale * 1.02f, 0, 0);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "Glow").Value;
            Color glowColor = new GradientColor(SlimeUtils.GoozOilColors, 0.33f, 0.33f).Value;
            glowColor.A = 0;
            spriteBatch.Draw(glow, Item.Center - Main.screenPosition, glow.Frame(), glowColor, rotation, Item.Size * 0.5f, scale, 0, 0);
            spriteBatch.Draw(glow, Item.Center - Main.screenPosition, glow.Frame(), glowColor * 0.5f, rotation, Item.Size * 0.5f, scale * 1.02f, 0, 0);
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<ScytheOfTheOldGodHeld>()] <= 0;

        public override bool AltFunctionUse(Player player) => false;

        public int swingStyle = 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ScytheOfTheOldGodHeld>()] <= 0)
            {
                Projectile.NewProjectileDirect(source, position, velocity, type, damage, 0, player.whoAmI, ai0: -1, ai1: swingStyle);
                swingStyle = (swingStyle + 1) % 3;
            }

            return false;
        }

        public override bool MeleePrefix() => true;
    }
}