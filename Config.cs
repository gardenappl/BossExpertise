
using System;
using System.IO;
using Terraria;
using Terraria.IO;

namespace BossExpertise
{
	public static class Config
	{
		public static bool DropBags;
		public static bool AddCheatSheetButton = true;
		public static bool ChangeBossAI = true;
		public static bool AddExpertCommand = true;
		public static bool DemonHeartHack;
		public static bool TransformMatrix;
		
		static int ConfigVersion;
		const int LatestVersion = 1;
		static string ConfigFolderPath = Path.Combine(Main.SavePath, "Mod Configs", "Boss Expertise");
		static string ConfigPath = Path.Combine(ConfigFolderPath, "config.json");
		static string ConfigVersionPath = Path.Combine(ConfigFolderPath, "config.version");
		
		static Preferences Configuration = new Preferences(ConfigPath);
		
		public static void Load()
		{
			if(File.Exists(ConfigVersionPath))
			{
				try
				{
					int.TryParse(File.ReadAllText(ConfigVersionPath), out ConfigVersion);
				}
				catch(Exception e)
				{
					BossExpertise.Log("Unable to read config version!");
					BossExpertise.Log(e.ToString());
					ConfigVersion = 0;
				}
			}
			else
				ConfigVersion = 0;
			
			if(ConfigVersion < LatestVersion)
				BossExpertise.Log("Config is outdated! Current version: {0} Latest version: {1}", ConfigVersion, LatestVersion);
			if(ConfigVersion > LatestVersion)
				BossExpertise.Log("Config is from the future?! Current version: {0} Latest version: {1}", ConfigVersion, LatestVersion);
				
//			BossExpertise.Log("Reading config...");
			if(!ReadConfig())
			{
				BossExpertise.Log("Failed to read config file! Recreating config...");
				SaveConfig();
			}
			else if(ConfigVersion != LatestVersion)
			{
				BossExpertise.Log("Replacing config with newest version...");
				File.WriteAllText(ConfigVersionPath, LatestVersion.ToString());
				SaveConfig();
			}
		}
		
		static bool ReadConfig()
		{
			if(Configuration.Load())
			{
				Configuration.Get("DropTreasureBagsInNormal", ref DropBags);
				Configuration.Get("DemonHeartWorksInNormal", ref DemonHeartHack);
				Configuration.Get("ChangeBossAI", ref ChangeBossAI);
				Configuration.Get("AddCheatSheetButton", ref AddCheatSheetButton);
				Configuration.Get("AddExpertCommand", ref AddExpertCommand);
				Configuration.Get("Fun", ref TransformMatrix);
				return true;
			}
			return false;
		}
		
		static void SaveConfig()
		{
			Configuration.Clear();
			Configuration.Put("DropTreasureBagsInNormal", DropBags);
			Configuration.Put("DemonHeartWorksInNormal", DemonHeartHack);
			Configuration.Put("ChangeBossAI", ChangeBossAI);
			Configuration.Put("AddCheatSheetButton", AddCheatSheetButton);
			Configuration.Put("AddExpertCommand", AddExpertCommand);
			Configuration.Put("Fun", TransformMatrix);
			Configuration.Save();
		}
	}
}
