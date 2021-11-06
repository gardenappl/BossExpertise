
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
			return Language.GetTextValue(Main.expertMode ? "Mods.BossExpertise.SwitchToNormal" : "Mods.BossExpertise.SwitchToExpert");
		}
		
		public static void OnButtonPressed()
		{
            ModContent.GetInstance<BossExpertise>().SetExpertMode(!Main.expertMode);
		}
		
		public static void OnPermissionChanged(bool hasPermission)
		{
			//do nothing
		}
	}
}
