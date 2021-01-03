using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using SolemnLament;

namespace SolemnLament.Buffs
{
    public class RIPBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rest in Peace");
            DisplayName.AddTranslation(GameCulture.Chinese, "安息");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            npc.GetGlobalNPC<SolemnLamentNPC>().BrokenDice++;
            if (npc.GetGlobalNPC<SolemnLamentNPC>().BrokenDice > 3)
            {
                npc.GetGlobalNPC<SolemnLamentNPC>().BrokenDice = 3;
            }
            return false;
        }
    }
}