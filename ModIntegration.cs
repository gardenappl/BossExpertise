
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossExpertise
{
	public static class ModIntegration
	{
		static BossExpertise mod;
		
		public static void Load(BossExpertise bossExpertiseMod)
		{
			mod = bossExpertiseMod;
			var cheatSheetMod = ModLoader.GetMod("CheatSheet");
			if(cheatSheetMod != null && !Main.dedServ)
			{
				cheatSheetMod.Call("AddButton_Test", Main.buffTexture[BuffID.Horrified], (Action)OnButtonPressed, (Func<string>)GetButtonTooltip);
			}
			var herosMod = ModLoader.GetMod("HEROsMod");
			if(herosMod != null)
			{
				herosMod.Call("AddPermission", "ToggleExpertMode", Language.GetTextValue("Mods.BossExpertise.ToggleModePermission"));
				if(!Main.dedServ)
				{
					herosMod.Call("AddSimpleButton", "ToggleExpertMode", Main.buffTexture[BuffID.Horrified], (Action)OnButtonPressed, (Action<bool>)OnPermissionChanged, (Func<string>)GetButtonTooltip);
				}
			}
		}
		
		public static string GetButtonTooltip()
		{
			return Language.GetTextValue(Main.expertMode ? "Mods.BossExpertise.SwitchToNormal" : "Mods.BossExpertise.SwitchToExpert");
		}
		
		public static void OnButtonPressed()
		{
			mod.SetExpertMode(!Main.expertMode);
		}
		
		public static void OnPermissionChanged(bool hasPermission)
		{
			//do nothing
		}
	}
}
