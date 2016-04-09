
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public static class Config
	{
		public static bool DropBags;
		
		static int ConfigVersion;
		const int LatestVersion = 2;
		static string ConfigFolderPath = Path.Combine(Main.SavePath, "Mod Configs", "BossExpertise");
		static string ConfigPath = Path.Combine(ConfigFolderPath, "config.txt");
		static string ConfigVersionFilePath = Path.Combine(ConfigFolderPath, "configVersion.txt");
		
		
		public static void Load()
		{
			if(!File.Exists(ConfigPath) || !File.Exists(ConfigVersionFilePath))
			{
				CreateConfig();
				WriteConfigVersion(LatestVersion);
				ErrorLogger.Log("Creating new config");
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
				ErrorLogger.Log("Recreating config");
			}
		}
		
		
		internal static bool ReadConfig()
		{
			var file = new StreamReader(ConfigPath);
			try
			{
				file.ReadLine();
				DropBags = Boolean.Parse(file.ReadLine().Split(':')[1]);
				BossExpertise.Log("Drop bags: {0}", DropBags);
				return true;
			}
			catch(Exception e)
			{
				BossExpertise.Log("Couldn't properly read config file! Using default values...");
				BossExpertise.Log(e.ToString());
			}
			finally
			{
				file.Dispose();
			}
			return false;
		}
		
		internal static int ReadConfigVersion()
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
		
		
		internal static void CreateConfig()
		{
			Directory.CreateDirectory(ConfigFolderPath);
			using(var file = File.CreateText(ConfigPath))
			{
				file.WriteLine("This value can either be True or False");
				file.WriteLine("Drop Treasure Bags: {0}", DropBags);
			}
		}
		
		internal static void WriteConfigVersion(int version)
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
