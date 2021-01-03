using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using SolemnLament;

namespace SolemnLament.Buffs
{
    public class EternalRestBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Eternal Rest");
            DisplayName.AddTranslation(GameCulture.Chinese, "安息之所");
            Description.SetDefault("Significantly enhance Solemn Lament's attack and effects.");
            Description.AddTranslation(GameCulture.Chinese, "大幅强化圣宣的攻击和效果。");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
            canBeCleared = false;
            longerExpertDebuff = false;
        }

    }
}