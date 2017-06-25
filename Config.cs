
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
		
		static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", "Boss Expertise.json");
		
		static string OldConfigFolderPath = Path.Combine(Main.SavePath, "Mod Configs", "Boss Expertise");
		static string OldConfigPath = Path.Combine(OldConfigFolderPath, "config.json");
		static string OldConfigVersionPath = Path.Combine(OldConfigFolderPath, "config.version");
		
		static Preferences Configuration = new Preferences(ConfigPath);
		
		public static void Load()
		{
			if(Directory.Exists(OldConfigFolderPath))
			{
				if(File.Exists(OldConfigPath))
				{
					BossExpertise.Log("Found config file in old folder! Moving config...");
					File.Move(OldConfigPath, ConfigPath);
				}
				if(File.Exists(OldConfigVersionPath))
				{
					File.Delete(OldConfigVersionPath);
				}
				if(Directory.GetFiles(OldConfigFolderPath).Length == 0 && Directory.GetDirectories(OldConfigFolderPath).Length == 0)
				{
					Directory.Delete(OldConfigFolderPath);
				}
				else
				{
					BossExpertise.Log("Old config folder still cotains some files/directories. They will not get deleted.");
				}
			}
			if(!ReadConfig())
			{
				BossExpertise.Log("Failed to read config file! Recreating config...");
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
