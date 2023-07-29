
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
		private static ReLogic.Content.Asset<Texture2D> CheatSheetButton;
		public static void Load()
		{
			CheatSheetButton = ModContent.Request<Texture2D>("BossExpertise/ExpertModeButton", ReLogic.Content.AssetRequestMode.ImmediateLoad);
			ModLoader.TryGetMod("CheatSheet", out Mod cheatSheetMod);
			if(cheatSheetMod != null && !Main.dedServ)
			{
				cheatSheetMod.Call("AddButton_Test", CheatSheetButton, (Action)OnButtonPressed, (Func<string>)GetButtonTooltip);
			}
			ModLoader.TryGetMod("HEROsMod", out Mod herosMod);
			if(herosMod != null)
			{
				herosMod.Call("AddPermission", "ToggleExpertModePermission", Language.GetTextValue("Mods.BossExpertise.ToggleModePermission"));
				if(!Main.dedServ)
				{
					herosMod.Call("AddSimpleButton", "ToggleExpertMode", CheatSheetButton, (Action)OnButtonPressed, (Action<bool>)OnPermissionChanged, (Func<string>)GetButtonTooltip);
				}
			}
		}
		
		public static string GetButtonTooltip()
		{
				if (BossExpertise.FakedDifficulty.HasFlag(Difficulty.Classic))
					return Language.GetTextValue("Mods.BossExpertise.SwitchToExpert");
				else if (BossExpertise.FakedDifficulty.HasFlag(Difficulty.Expert))
					return Language.GetTextValue("Mods.BossExpertise.SwitchToMaster");
				else if (BossExpertise.FakedDifficulty.HasFlag(Difficulty.Master))
					return Language.GetTextValue("Mods.BossExpertise.SwitchToNormal");
				else
					return Language.GetTextValue("Mods.BossExpertise.SwitchToNormal");	
		}
		
		public static void OnButtonPressed()
		{
			Difficulty currentDifficulty = BossExpertise.FakedDifficulty;
			Difficulty newDifficulty;
			if (currentDifficulty.HasFlag(Difficulty.Classic))
				newDifficulty = Difficulty.Expert;
			else if (currentDifficulty.HasFlag(Difficulty.Expert))
				newDifficulty = Difficulty.Master;
			else
				newDifficulty = Difficulty.Classic;
			ModContent.GetInstance<BossExpertise>().SetDifficultyMode(newDifficulty);
		}
		
		public static void OnPermissionChanged(bool hasPermission)
		{
			//do nothing
		}

		public static void Unload()
		{ 
			CheatSheetButton = null;
		}
	}
}
