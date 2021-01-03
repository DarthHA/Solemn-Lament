using SolemnLament.Items;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using SolemnLament.Buffs;
using Microsoft.Xna.Framework;
using SolemnLament.Gores;
using Terraria.Graphics.Effects;
using SolemnLament.Sky;
using SolemnLament.Projectiles;

namespace SolemnLament
{
	public class SolemnLament : Mod
	{
        public static SolemnLament Instance;

        public override void Load()
        {
            Instance = this;
            Filters.Scene["SolemnLament:SolemnLamentSky"] = new Filter(new SolemnLamentSkyScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.9f, 0.9f).UseOpacity(0.2f), EffectPriority.VeryHigh);
            SkyManager.Instance["SolemnLament:SolemnLamentSky"] = new SolemnLamentSky();
            On.Terraria.NPC.HitEffect += new On.Terraria.NPC.hook_HitEffect(HitEffectHook);
            On.Terraria.NPC.VanillaHitEffect += new On.Terraria.NPC.hook_VanillaHitEffect(HitEffectHook2);
            On.Terraria.Projectile.Kill += new On.Terraria.Projectile.hook_Kill(KillHook);
        }
        public override void Unload()
        {
            SkyManager.Instance["SolemnLament:SolemnLamentSky"].Deactivate();
            Instance = null;
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<EternalRestBuff>()))
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Angela");
                priority = MusicPriority.BossHigh;
            }
        }


        public static void HitEffectHook(On.Terraria.NPC.orig_HitEffect orig, NPC self, int direction, double dmg)
        {
            orig.Invoke(self, direction, dmg);
            if (self.HasBuff(ModContent.BuffType<RIPBuff>()))
            {
                if (self.life <= 0 && self.lifeMax > 10)
                {
                    foreach (Gore gore in Main.gore)
                    {
                        if (gore.active && self.Hitbox.Contains(new Point((int)gore.position.X, (int)gore.position.Y))) 
                        {
                            gore.active = false;
                        }
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 GorePos = new Vector2(self.position.X + Main.rand.Next(self.width), self.position.Y + Main.rand.Next(self.height));
                        Gore.NewGore(GorePos, Vector2.Zero, Instance.GetGoreSlot("Gores/ButterflyGore"), 0.5f);
                    }
                }
            }
        }


        public static void HitEffectHook2(On.Terraria.NPC.orig_VanillaHitEffect orig, NPC self, int direction, double dmg)
        {
            orig.Invoke(self, direction, dmg);
            if (self.HasBuff(ModContent.BuffType<RIPBuff>()))
            {
                if (self.life <= 0 && self.lifeMax > 10)
                {
                    foreach (Gore gore in Main.gore)
                    {
                        if (gore.active && self.Hitbox.Contains(new Point((int)gore.position.X, (int)gore.position.Y)))
                        {
                            gore.active = false;
                        }
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 GorePos = new Vector2(self.position.X + Main.rand.Next(self.width), self.position.Y + Main.rand.Next(self.height));
                        Gore.NewGore(GorePos, Vector2.Zero, Instance.GetGoreSlot("Gores/ButterflyGore"), 0.5f);
                    }
                }
            }

        }


        public static void KillHook(On.Terraria.Projectile.orig_Kill orig, Projectile self)
        {
            if (self.type == ModContent.ProjectileType<BlackEGO>() || self.type == ModContent.ProjectileType<WhiteEGO>() ||
                self.type == ModContent.ProjectileType<BlackButterfly>() || self.type == ModContent.ProjectileType<WhiteButterfly>()) 
            {
                if (self.alpha < 220)
                {
                    self.timeLeft = 99999;
                    self.penetrate = -1;
                    return;
                }
            }
            orig.Invoke(self);
        }
    }

	public class SolemnLamentPlayer : ModPlayer
    {
		public int AttackType = -1;
        public int DmgDealt = 0;
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (target.HasBuff(ModContent.BuffType<RIPBuff>()))
            {
                if (!player.HasBuff(ModContent.BuffType<EternalRestBuff>()))
                {
                    if (!target.immortal)
                    {
                        DmgDealt += damage;
                    }
                }
                if (DmgDealt >= 60000 * Main.LocalPlayer.rangedDamage)
                {
                    DmgDealt = 0;
                    player.AddBuff(ModContent.BuffType<EternalRestBuff>(), 60 * 60 * 4);
                }
            }
        }
        public override void PreUpdate()
        {
            Main.itemAnimations[ModContent.ItemType<SolemnLamentItem>()].FrameCounter = 0;
            if (player.HeldItem.type == ModContent.ItemType<SolemnLamentItem>())
            {
                switch (AttackType)
                {
                    case -1:
                        Main.itemAnimations[ModContent.ItemType<SolemnLamentItem>()].Frame = 0;
                        break;
                    case 0:
                        Main.itemAnimations[ModContent.ItemType<SolemnLamentItem>()].Frame = 1;
                        break;
                    case 1:
                        Main.itemAnimations[ModContent.ItemType<SolemnLamentItem>()].Frame = 2;
                        break;
                    default:
                        break;
                }

            }
            else
            {
                AttackType = -1;
                Main.itemAnimations[ModContent.ItemType<SolemnLamentItem>()].Frame = 0;
            }
        }
        public override void UpdateBiomeVisuals()
        {
            player.ManageSpecialBiomeVisuals("SolemnLament:SolemnLamentSky", player.HasBuff(ModContent.BuffType<EternalRestBuff>()), default);
        }

        public override void PostUpdateMiscEffects()
        {
            if (player.HeldItem.type != ModContent.ItemType<SolemnLamentItem>())
            {
                DmgDealt = 0;
                player.ClearBuff(ModContent.BuffType<EternalRestBuff>());
            }
        }
    }

    public class SolemnLamentNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int BrokenDice = 0;
        public override void PostAI(NPC npc)
        {
            if (!npc.HasBuff(ModContent.BuffType<RIPBuff>()))
            {
                BrokenDice = 0;
            }
        }
        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (npc.HasBuff(ModContent.BuffType<RIPBuff>()))
            {
                if (BrokenDice > 2)
                {
                    damage += defense / 2;
                    if (Main.LocalPlayer.HasBuff(ModContent.BuffType<EternalRestBuff>()))
                    {
                        damage += npc.lifeMax * 0.0003f;
                    }
                }
            }

            return true;
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (npc.HasBuff(ModContent.BuffType<RIPBuff>()))
            {
                if (BrokenDice > 0)
                {
                    if (Main.LocalPlayer.HasBuff(ModContent.BuffType<EternalRestBuff>()))
                    {
                        npc.position -= npc.velocity * 0.4f;
                    }
                    else
                    {
                        npc.position -= npc.velocity * 0.2f;
                    }
                }

            }
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            if (npc.HasBuff(ModContent.BuffType<RIPBuff>()))
            {
                if (BrokenDice > 1)
                {
                    if (Main.LocalPlayer.HasBuff(ModContent.BuffType<EternalRestBuff>()))
                    {
                        damage = (int)(damage * 0.1f);
                    }
                    else
                    {
                        damage = (int)(damage * 0.6f);
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC npc, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (npc.HasBuff(ModContent.BuffType<RIPBuff>()))
            {
                if (BrokenDice > 1)
                {
                    if (Main.LocalPlayer.HasBuff(ModContent.BuffType<EternalRestBuff>()))
                    {
                        damage = (int)(damage * 0.1f);
                    }
                    else
                    {
                        damage = (int)(damage * 0.6f);
                    }
                }
            }
        }


        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (SolemnLamentWorld.TombCount >= 4 * 6 && NPC.downedMoonlord)
            {
                if (type == NPCID.ArmsDealer && !Main.LocalPlayer.HasItem(ModContent.ItemType<SolemnLamentItem>()))
                {
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<SolemnLamentItem>());
                    shop.item[nextSlot].value = Item.sellPrice(5, 0, 0, 0);
                    nextSlot++;
                }
            }
        }

    }

    public class SolemnLamentWorld : ModWorld
    {
        public static int TombCount = 0;
        public override void TileCountsAvailable(int[] tileCounts)
        {
            TombCount = tileCounts[TileID.Tombstones];   
        }
    }

}