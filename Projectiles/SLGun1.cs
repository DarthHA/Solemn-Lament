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
    public class SLGun1 : ModProjectile
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
            projectile.frame = (int)projectile.ai[0];
            Player owner = Main.player[projectile.owner];
            //SolemnLamentPlayer modowner = owner.GetModPlayer<SolemnLamentPlayer>();
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
            owner.itemTime = 2;
            owner.itemAnimation = 2;
            owner.heldProj = projectile.whoAmI;
            int dir = Math.Sign(projectile.velocity.X);
            owner.direction = dir;
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.Center = owner.Center + OffSet.RotatedBy(projectile.rotation);
            if (owner.mount.Active)
            {
                projectile.Center = owner.MountedCenter + OffSet.RotatedBy(projectile.rotation);
            }
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
            if (projectile.ai[1] == 0)
            {
                projectile.ai[1] = 1;
                if (projectile.ai[0] == 0)
                {
                    if (owner.HasBuff(ModContent.BuffType<EternalRestBuff>()))
                    {
                        Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<BlackEGO>(), projectile.damage * 5, 0, owner.whoAmI);
                    }
                    else
                    {
                        Projectile.NewProjectile(projectile.Center + projectile.rotation.ToRotationVector2() * BlackButterfly.OffSet, projectile.velocity, ModContent.ProjectileType<BlackButterfly>(), projectile.damage, projectile.knockBack, owner.whoAmI);
                    }
                }
                else
                {
                    if (owner.HasBuff(ModContent.BuffType<EternalRestBuff>()))
                    {
                        Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<WhiteEGO>(), projectile.damage * 5, 0, owner.whoAmI);
                    }
                    else
                    {
                        Projectile.NewProjectile(projectile.Center + projectile.rotation.ToRotationVector2() * BlackButterfly.OffSet, projectile.velocity, ModContent.ProjectileType<WhiteButterfly>(), projectile.damage, projectile.knockBack, owner.whoAmI);
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player owner = Main.player[projectile.owner];

            Texture2D tex = Main.projectileTexture[projectile.type];
            Rectangle rectangle = new Rectangle(0, tex.Height / 2 * projectile.frame, tex.Width, tex.Height / 2);
            SpriteEffects SP = owner.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (owner.direction < 0)
            {
                float r = projectile.rotation + MathHelper.Pi;
                if (owner.gravDir < 0)
                {
                    SP = SpriteEffects.None;
                    r -= MathHelper.Pi;
                }
                spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, rectangle, Color.White, r, rectangle.Size() / 2, projectile.scale * 0.5f, SP, 0);
            }
            else
            {
                if (owner.gravDir < 0)
                {
                    SP = SpriteEffects.FlipVertically;
                }
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