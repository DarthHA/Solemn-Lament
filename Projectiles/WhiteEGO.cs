using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolemnLament.Buffs;
using SolemnLament.Items;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace SolemnLament.Projectiles
{
    public class WhiteEGO : ModProjectile
    {
        public static readonly int OffSet = 350;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("White Butterfly");
            DisplayName.AddTranslation(GameCulture.Chinese, "白蝶");
        }
        public override void SetDefaults()          //512 164  scale = 1
        {
            projectile.width = 1;                     //560 160
            projectile.height = 1;
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
            projectile.localNPCHitCooldown = 10;
            projectile.ownerHitCheck = true;
        }
        public override void AI()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 99999;
            projectile.position = Main.screenPosition;
            projectile.width = Main.screenWidth;
            projectile.height = Main.screenHeight;
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
            spriteBatch.Draw(tex, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * projectile.Opacity);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanCutTiles()
        {
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.buffImmune[ModContent.BuffType<RIPBuff>()] = false;
            target.AddBuff(ModContent.BuffType<RIPBuff>(), 300);
        }

    }
}