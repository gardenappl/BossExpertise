
using System;
using System.IO;
using Terraria;

namespace BossExpertise
{
	public static class OldConfig
	{
		static string ConfigFolderPath = Path.Combine(Main.SavePath, "Mod Configs", "BossExpertise");
		static string ConfigPath = Path.Combine(ConfigFolderPath, "config.txt");
		static string ConfigVersionPath = Path.Combine(ConfigFolderPath, "configVersion.txt");
		
		public static void Load()
		{
			if(!Directory.Exists(ConfigFolderPath))
				return;
			
			if(File.Exists(ConfigPath))
			{
				BossExpertise.Log("Found config file with old format! Reading outdated config...");
				var file = new StreamReader(ConfigPath);
				try
				{
					file.ReadLine();
					bool.TryParse(file.ReadLine().Split(':')[1], out Config.DropBags);
					
					file.ReadLine();
					bool.TryParse(file.ReadLine().Split(':')[1], out Config.ChangeBossAI);
					
					file.ReadLine();
					bool.TryParse(file.ReadLine().Split(':')[1], out Config.AddCheatSheetButton);
					
					file.ReadLine();
					bool.TryParse(file.ReadLine().Split(':')[1], out Config.AddExpertCommand);
				}
				catch(Exception e)
				{
					BossExpertise.Log("Unable to read old config file!");
					BossExpertise.Log(e.ToString());
				}
				finally
				{
					file.Dispose();
				}
			}
			
			BossExpertise.Log("Deleting outdated config...");
			try
			{
				File.Delete(ConfigPath);
				File.Delete(ConfigVersionPath);
				if(Directory.GetFiles(ConfigFolderPath).Length == 0 && Directory.GetDirectories(ConfigFolderPath).Length == 0)
				{
					Directory.Delete(ConfigFolderPath);
				}
				else
				{
					BossExpertise.Log("Outdated config folder still cotains some files/directories. They will not get deleted.");
				}
			}
			catch(Exception e)
			{
				BossExpertise.Log("Unable to delete old config!");
				BossExpertise.Log(e.ToString());
			}
		}
	}
}
