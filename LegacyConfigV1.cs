
using Newtonsoft.Json;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader.Config;
using Terraria.ModLoader;

namespace BossExpertise
{
	public static class LegacyConfigV1
	{
		static string ConfigFolderPath = Path.Combine(Main.SavePath, "Mod Configs", "BossExpertise");
		static string ConfigPath = Path.Combine(ConfigFolderPath, "config.txt");
		static string ConfigVersionPath = Path.Combine(ConfigFolderPath, "configVersion.txt");

		static bool DropBags = false;
		static bool ChangeBossAI = true;
		static bool AddCheatSheetButton = true;
		static bool AddExpertCommand = false;

		public static void Load()
		{
			if (!Directory.Exists(ConfigFolderPath))
				return;


            ModContent.GetInstance<BossExpertise>().Logger.Warn("Found config file in old format! Reading config.txt...");
			bool success = ReadConfig();


			if (success)
			{
                ModContent.GetInstance<BossExpertise>().Logger.Warn("Saving outdated config as JSON...");
				SaveAsJson();
			}


            ModContent.GetInstance<BossExpertise>().Logger.Warn("Deleting outdated config...");
			try
			{
				File.Delete(ConfigPath);
				File.Delete(ConfigVersionPath);
				if (Directory.GetFiles(ConfigFolderPath).Length == 0 && Directory.GetDirectories(ConfigFolderPath).Length == 0)
				{
					Directory.Delete(ConfigFolderPath);
				}
				else
				{
                    ModContent.GetInstance<BossExpertise>().Logger.Warn("Outdated config folder still cotains some files/directories. They will not get deleted.");
				}
			}
			catch (Exception e)
			{
                ModContent.GetInstance<BossExpertise>().Logger.Error("Unable to delete old config!", e);
			}
		}

		static bool ReadConfig()
		{
			bool success = false;

			if (File.Exists(ConfigPath))
			{
                ModContent.GetInstance<BossExpertise>().Logger.Warn("Found config file with old format! Reading outdated config...");

				using (var file = new StreamReader(ConfigPath))
				{
					try
					{
						file.ReadLine();
						bool.TryParse(file.ReadLine().Split(':')[1], out DropBags);

						file.ReadLine();
						bool.TryParse(file.ReadLine().Split(':')[1], out ChangeBossAI);

						file.ReadLine();
						bool.TryParse(file.ReadLine().Split(':')[1], out AddCheatSheetButton);

						file.ReadLine();
						bool.TryParse(file.ReadLine().Split(':')[1], out AddExpertCommand);

						success = true;
					}
					catch (Exception e)
					{
                        ModContent.GetInstance<BossExpertise>().Logger.Error("Unable to read old config file!", e);
					}
				}
			}
			return success;
		}

		static void SaveAsJson()
		{
			var configObject = new
			{
				DropTreasureBagsInNormal = DropBags,
				AddCheatSheetButton = AddCheatSheetButton,
				AddExpertCommand = AddExpertCommand,
				ChangeBossAI = ChangeBossAI
			};

			string json = JsonConvert.SerializeObject(configObject, ConfigManager.serializerSettings);
			File.WriteAllText(Path.Combine(ConfigManager.ModConfigPath, "BossExpertise_Config.json"), json);
		}
	}
}
