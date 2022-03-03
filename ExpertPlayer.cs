
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertPlayer : ModPlayer
	{
        public override void Load()
        {
          On.Terraria.Player.IsAValidEquipmentSlotForIteration += HookIsAValidEquipmentSlotForIteration;
        }

		public override void Unload()
		{
			On.Terraria.Player.IsAValidEquipmentSlotForIteration -= HookIsAValidEquipmentSlotForIteration;
		}

		private static bool HookIsAValidEquipmentSlotForIteration(On.Terraria.Player.orig_IsAValidEquipmentSlotForIteration orig, Player self, int slot)
		{
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
			bool cantUseExpertModeSlot = (!Main.expertMode && !(BossExpertise.CurrentDifficulty > 0 && ModContent.GetInstance<Config>().SlotsWorksInNormal)) && !Main.gameMenu;
			if (cantUseExpertModeSlot)
			{
				result = false;
			}
			return result;
		MasterModeSlot:
			result = true;
			bool cantUseMasterModeSlot = (!Main.masterMode && !(BossExpertise.CurrentDifficulty > 1 && ModContent.GetInstance<Config>().SlotsWorksInNormal)) && !Main.gameMenu;
			if (cantUseMasterModeSlot)
			{
				result = false;
			}
			return result;

			orig(self, slot);
		}

	}
}
