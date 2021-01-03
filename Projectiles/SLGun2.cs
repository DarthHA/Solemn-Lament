using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolemnLament.Items;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace SolemnLament.Projectiles
{
    public class SLGun2 : ModProjectile
    {
        public static readonly Vector2 OffSet = new Vector2(20, 0);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solenm Lament");
            DisplayName.AddTranslation(GameCulture.Chinese, "圣宣");
            Main.projFrames[projectile.type] = 2;
        }
        public override void SetDefaults()          //85 78  scale=0.75
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.scale = 1f;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 50;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.penetrate = -1;
        }
        public override void AI()
        {
            projectile.frame = ((int)projectile.ai[0] + 1) % 2;
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (owner.HeldItem.type != ModContent.ItemType<SolemnLamentItem>())
            {
                projectile.Kill();
                return;
            }

            projectile.rotation = -MathHelper.Pi / 2;
            if (owner.gravDir < 0)
            {
                projectile.rotation = MathHelper.Pi / 2;
            }
            projectile.Center = owner.Center + OffSet.RotatedBy(projectile.rotation);
            if (owner.mount.Active)
            {
                projectile.Center = owner.MountedCenter + OffSet.RotatedBy(projectile.rotation);
            }
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player owner = Main.player[projectile.owner];

            Texture2D tex = Main.projectileTexture[projectile.type];
            Rectangle rectangle = new Rectangle(0, tex.Height / 2 * projectile.frame, tex.Width, tex.Height / 2);
            SpriteEffects SP = owner.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (owner.direction < 0)
            {
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, rectangle, Color.White, projectile.rotation + MathHelper.Pi, rectangle.Size() / 2, projectile.scale * 0.5f, SP, 0);
            }
            else
            {
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, rectangle, Color.White, projectile.rotation, rectangle.Size() / 2, projectile.scale * 0.5f, SP, 0);
            }
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}