
using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace BossExpertise
{
	public static class LegacyConfigV2
	{	
		static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", "Boss Expertise.json");

		public static void Load()
		{
			if(File.Exists(ConfigPath))
			{
				BossExpertise.Instance.Logger.Warn("Found config file in old location. Moving config...");
				string newPath = Path.Combine(ConfigManager.ModConfigPath, "BossExpertise_Config.json");
				File.Move(ConfigPath, newPath);
			}
		}
	}
}
