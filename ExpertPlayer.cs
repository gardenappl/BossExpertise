
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertPlayer : ModPlayer
	{
        public override void Load()
        {
          Terraria.On_Player.IsItemSlotUnlockedAndUsable += HookIsItemSlotUnlockedAndUsable;
        }

		public override void Unload()
		{
			Terraria.On_Player.IsItemSlotUnlockedAndUsable -= HookIsItemSlotUnlockedAndUsable;
		}

		private static bool HookIsItemSlotUnlockedAndUsable(Terraria.On_Player.orig_IsItemSlotUnlockedAndUsable orig, Player self, int slot)
		{
			bool slotWorksSetting = ModContent.GetInstance<Config>().SlotsWorksInNormal;
			if (slot < 10)
			{
				if (slot == 8)
				{
					goto DemonHeartSlot;
				}
				if (slot == 9)
				{
					goto MasterModeSlot;
				}
			}
			else
			{
				if (slot == 18)
				{
					goto DemonHeartSlot;
				}
				if (slot == 19)
				{
					goto MasterModeSlot;
				}
			}
			return true;
			DemonHeartSlot:
				bool result = self.extraAccessory;
				bool cantUseExpertModeSlot = (!Main.expertMode && !((BossExpertise.FakedBeneficialDifficulty.HasFlag(Difficulty.Expert) || BossExpertise.FakedBeneficialDifficulty.HasFlag(Difficulty.Master)) && slotWorksSetting)) && !Main.gameMenu;
				result = cantUseExpertModeSlot ? false : result;
				return result;
			MasterModeSlot:
				bool cantUseMasterModeSlot = (!Main.masterMode && !(BossExpertise.FakedBeneficialDifficulty.HasFlag(Difficulty.Master) && slotWorksSetting)) && !Main.gameMenu;
				result = cantUseMasterModeSlot ? false : true;
				return result;

				#pragma warning disable CS0162 // Unreachable code detected
				orig(self, slot); //This is a total overwrite dont worry about it being unreachable.
				#pragma warning restore CS0162 // Unreachable code detected
		}

	}
}
