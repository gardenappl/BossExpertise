
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public static class Config
	{
		public static bool DropBags;
		public static bool AddCheatSheetButton = true;
		public static bool ChangeBossAI = true;
		public static bool AddCommand = true;
		
		static int ConfigVersion;
		const int LatestVersion = 3;
		static string ConfigFolderPath = Path.Combine(Main.SavePath, "Mod Configs", "BossExpertise");
		static string ConfigPath = Path.Combine(ConfigFolderPath, "config.txt");
		static string ConfigVersionFilePath = Path.Combine(ConfigFolderPath, "configVersion.txt");
		
		
		public static void Load()
		{
			if(!File.Exists(ConfigPath) || !File.Exists(ConfigVersionFilePath))
			{
				CreateConfig();
				WriteConfigVersion(LatestVersion);
				ErrorLogger.Log("Creating new config...");
			}
			
			ConfigVersion = ReadConfigVersion();
			if(ConfigVersion == LatestVersion)
				BossExpertise.Log("Config is up to date! Config version: {0}", ConfigVersion);
			else if(ConfigVersion < LatestVersion)
				BossExpertise.Log("Config is outdated! Config version: {0}; Latest version: {1}", ConfigVersion, LatestVersion);
			else if(ConfigVersion > LatestVersion)
				BossExpertise.Log("Config is from the future?! Config version: {0}; Latest version: {1}", ConfigVersion, LatestVersion);
			
			if(!ReadConfig() || ConfigVersion < LatestVersion)
			{
				CreateConfig();
				WriteConfigVersion(LatestVersion);
				ErrorLogger.Log("Recreating config...");
			}
			
			//Debug
			BossExpertise.Log("Drop bags: {0}", DropBags);
			BossExpertise.Log("Change boss AI: {0}", ChangeBossAI);
			BossExpertise.Log("Add Cheat Sheet button: {0}", AddCheatSheetButton);
			BossExpertise.Log("Add /expert command: {0}", AddCommand);
		}
		
		
		static bool ReadConfig()
		{
			var file = new StreamReader(ConfigPath);
			try
			{
				file.ReadLine(); //Skip the comment
				DropBags = Boolean.Parse(file.ReadLine().Split(':')[1]); //Read the actual value
				
				file.ReadLine();
				ChangeBossAI = Boolean.Parse(file.ReadLine().Split(':')[1]);
				
				file.ReadLine();
				AddCheatSheetButton = Boolean.Parse(file.ReadLine().Split(':')[1]);
				
				file.ReadLine();
				AddCommand = Boolean.Parse(file.ReadLine().Split(':')[1]);
				
				return true;
			}
			catch(Exception e)
			{
				if(ConfigVersion == LatestVersion)
				{
					BossExpertise.Log("Couldn't properly read config file! Using default values...");
					BossExpertise.Log(e.ToString());
				}
			}
			finally
			{
				file.Dispose();
			}
			return false;
		}
		
		static int ReadConfigVersion()
		{
			string[] versionFile = File.ReadAllLines(ConfigVersionFilePath);
			try
			{
				return Int32.Parse(versionFile[1]);
			}
			catch(Exception e)
			{
				BossExpertise.Log("Couldn't properly read config version file!");
				BossExpertise.Log(e.ToString());
				return LatestVersion;
			}
		}
		
		
		static void CreateConfig()
		{
			Directory.CreateDirectory(ConfigFolderPath);
			using(var file = File.CreateText(ConfigPath))
			{
				file.WriteLine("This value can either be True or False");
				file.WriteLine("Drop Treasure Bags: {0}", DropBags);
				
				file.WriteLine("This value can either be True or False");
				file.WriteLine("Change boss AI: {0}", ChangeBossAI);
				
				file.WriteLine("This value can either be True or False (requires the Cheat Sheet mod)");
				file.WriteLine("Add Cheat Sheet button: {0}", AddCheatSheetButton);
				
				file.WriteLine("This value can either be True or False");
				file.WriteLine("Add /expert command: {0}", AddCommand);
			}
		}
		
		static void WriteConfigVersion(int version)
		{
			using(var file = File.CreateText(ConfigVersionFilePath))
			{
				file.WriteLine("DON'T CHANGE THE NUMBER BELOW");
				file.WriteLine(version);
				file.WriteLine("Seriously though, if you screw it up your config file might get deleted and the mod might not load.");
			}
		}
	}
}
