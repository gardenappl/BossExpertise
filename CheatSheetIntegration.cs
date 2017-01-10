
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossExpertise
{
	public static class CheatSheetIntegration
	{
		static BossExpertise mod;
		
		public static void Load(BossExpertise bossExpertiseMod)
		{
			mod = bossExpertiseMod;
			var cheatSheetMod = ModLoader.GetMod("CheatSheet");
			if(cheatSheetMod != null && !Main.dedServ)
				cheatSheetMod.Call("AddButton_Test", mod.GetTexture("ExpertModeButton"), (Action)ExpertModeButtonPressed, (Func<string>)ExpertModeButtonTooltip);
		}
		
		public static string ExpertModeButtonTooltip()
		{
			return Main.expertMode ? "Switch to Normal Mode" : "Switch to Expert Mode";
		}
		
		public static void ExpertModeButtonPressed()
		{
//			Main.PlaySound(10);
			mod.SetExpertMode(!Main.expertMode);
		}
	}
}
