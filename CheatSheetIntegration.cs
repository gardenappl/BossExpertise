
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public static class CheatSheetIntegration
	{
		public static void Load(BossExpertise mod)
		{
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
			Main.expertMode = !Main.expertMode;
		}
	}
}
