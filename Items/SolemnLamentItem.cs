using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using SolemnLament.Projectiles;
using Terraria.Localization;

namespace SolemnLament.Items
{
	public class SolemnLamentItem : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Solemn Lament");
			DisplayName.AddTranslation(GameCulture.Chinese, "圣宣");
			Tooltip.SetDefault("\"The somber design is a reminder that not a sliver of frivolity is allowed for the minds of those who mourn.\n" +
			"One handgun symbolizes grief for the dead, while the other symbolizes early lament for the living.\"\n" +
			"Take turns using black handgun and white handgun to attack.\n" +
			"White handgun weakens enemies' ability, while black handgun deals higher damage.\n" +
			"After dealing enough amount of damage, its attacks and effects will be significantly enhanced.\n" +
			"All effects will disappear after you switch weapons.\n" +
			"Enemies killed by Solemn Lament will turn into butterflies.");
			Tooltip.AddTranslation(GameCulture.Chinese, "“葬礼上需要的是严肃，不需要那些色彩斑斓的配饰。\n"+
			"黑色的手枪象征着逝者的悲痛，而白色的手枪象征着生者的哀悼。”\n" +
			"轮流使用黑枪和白枪进行攻击，\n" +
			"白枪削弱敌人能力，黑枪具有较高伤害，\n" +
			"当你累计造成足够伤害后会大幅提高圣宣的攻击和效果，\n" +
			"所有效果会在你切换武器后消失，\n" +
			"被圣宣杀死的敌人会变成蝴蝶。");
			
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(99999, 3));
		}
		
		public override void SetDefaults() 
		{
			item.damage = 85;
			item.ranged = true;
			item.width = 96;
			item.height = 52;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 1;
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.White;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/ButterflyAttack");             //SoundID.Item44;
			item.useTurn = false;
			item.noUseGraphic = true;
			//item.useAmmo = AmmoID.Bullet;
			item.shoot = ModContent.ProjectileType<SLGun1>();
			item.shootSpeed = 1;
		}
        public override bool CanUseItem(Player player)
        {
			item.noUseGraphic = true;
			item.autoReuse = false;
			return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SolemnLamentPlayer modplayer = player.GetModPlayer<SolemnLamentPlayer>();
			if (modplayer.AttackType < 0)
			{
				modplayer.AttackType = 0;
			}
			modplayer.AttackType = (modplayer.AttackType + 1) % 2;
			Projectile.NewProjectile(player.Center, new Vector2(speedX, speedY), ModContent.ProjectileType<SLGun1>(), damage, knockBack, player.whoAmI, modplayer.AttackType);
			Projectile.NewProjectile(player.Center, new Vector2(speedX, speedY), ModContent.ProjectileType<SLGun2>(), damage, knockBack, player.whoAmI, modplayer.AttackType);
			return false;
		}

    }
}