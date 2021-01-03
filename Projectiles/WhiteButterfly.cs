using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolemnLament.Buffs;
using SolemnLament.Items;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
namespace SolemnLament.Projectiles
{
    public class WhiteButterfly : ModProjectile
    {
        public static readonly int OffSet = 350;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("White Butterfly");
            DisplayName.AddTranslation(GameCulture.Chinese, "白蝶");
        }
        public override void SetDefaults()          //512 164  scale = 1
        {
            projectile.width = 160;                     //560 160
            projectile.height = 160;
            projectile.scale = 1f;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.timeLeft = 6000;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.damage = 100;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 120;
        }
        public override void AI()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 99999;
            projectile.rotation = projectile.velocity.ToRotation();
            if (projectile.ai[0] == 0)
            {
                projectile.alpha -= 50;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                    projectile.ai[0] = 1;
                }
            }
            else
            {
                projectile.alpha += 7;
                if (projectile.alpha > 250)
                {
                    projectile.Kill();
                    return;
                }
            }
        }
        
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            SpriteEffects SP = projectile.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (projectile.velocity.X < 0)
            {
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation + MathHelper.Pi, tex.Size() / 2, projectile.scale, SP, 0);
            }
            else
            {
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation, tex.Size() / 2, projectile.scale, SP, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center - projectile.rotation.ToRotationVector2() * 280, projectile.Center + projectile.rotation.ToRotationVector2() * 280, 160, ref point);
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= 5;
            crit = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.buffImmune[ModContent.BuffType<RIPBuff>()] = false;
            target.AddBuff(ModContent.BuffType<RIPBuff>(), 200);
        }
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center - projectile.rotation.ToRotationVector2() * 280, projectile.Center + projectile.rotation.ToRotationVector2() * 280, (160 + 16) * projectile.scale, DelegateMethods.CutTiles);
        }
    }
}