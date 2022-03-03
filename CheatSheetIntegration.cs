
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossExpertise
{
	public static class CheatSheetIntegration
	{
		public static void Load()
		{
			ModLoader.TryGetMod("CheatSheet", out Mod cheatSheetMod);
			if(cheatSheetMod != null && !Main.dedServ)
			{
				cheatSheetMod.Call("AddButton_Test", ModContent.Request<Texture2D>("BossExpertise/ExpertModeButton"), (Action)OnButtonPressed, (Func<string>)GetButtonTooltip);
			}
			ModLoader.TryGetMod("HEROsMod", out Mod herosMod);
			if(herosMod != null)
			{
				herosMod.Call("AddPermission", "ToggleExpertModePermission", Language.GetTextValue("Mods.BossExpertise.ToggleModePermission"));
				if(!Main.dedServ)
				{
					herosMod.Call("AddSimpleButton", "ToggleExpertMode", ModContent.Request<Texture2D>("BossExpertise/ExpertModeButton"), (Action)OnButtonPressed, (Action<bool>)OnPermissionChanged, (Func<string>)GetButtonTooltip);
				}
			}
		}
		
		public static string GetButtonTooltip()
		{
			switch(BossExpertise.CurrentDifficulty)
            {
				case 0:
					return Language.GetTextValue("Mods.BossExpertise.SwitchToExpert");
				case 1:
					return Language.GetTextValue("Mods.BossExpertise.SwitchToMaster");
				case 2:
					return Language.GetTextValue("Mods.BossExpertise.SwitchToNormal");
				default:
					return Language.GetTextValue("Mods.BossExpertise.SwitchToNormal");
			}
			
		}
		
		public static void OnButtonPressed()
		{
			ModContent.GetInstance<BossExpertise>().SetDifficultyMode(BossExpertise.CurrentDifficulty + 1);
		}
		
		public static void OnPermissionChanged(bool hasPermission)
		{
			//do nothing
		}
	}
}
