
using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace BossExpertise
{
	public static class Config
	{
		public static bool DropBags;
		const string DropBagsKey = "DropTreasureBagsInNormal";
		
		public static bool AddCheatSheetButton;
		const string AddCheatSheetButtonKey = "AddCheatSheetButton";
		
		public static bool ChangeBossAI;
		const string ChangeBossAIKey = "ChangeBossAI";
		
		public static bool AddExpertCommand;
		const string AddExpertCommandKey = "AddExpertCommand";
		
		public static bool DemonHeartHack;
		const string DemonHeartHackKey = "DemonHeartWorksInNormal";
		
		public static bool TransformMatrix;
		const string TransformMatrixKey = "Fun";
		
		static string ConfigPath = Path.Combine(Main.SavePath, "Mod Configs", "Boss Expertise.json");
		static Preferences Configuration = new Preferences(ConfigPath);

		public static void SetDefaults()
		{
			DropBags = false;
			AddCheatSheetButton = true;
			ChangeBossAI = true;
			AddExpertCommand = true;
			DemonHeartHack = false;
			TransformMatrix = false;
		}

		public static void Load()
		{
			if(!ConfigLegacy.Load())
			{
				SetDefaults();
			}

			if(!ReadConfig())
			{
				BossExpertise.Instance.Logger.Warn("Failed to read config file! Creating config...");
				SaveConfig();
			}
		}
		
		public static bool ReadConfig()
		{
			if(Configuration.Load())
			{
				Configuration.Get(DropBagsKey, ref DropBags);
				Configuration.Get(DemonHeartHackKey, ref DemonHeartHack);
				Configuration.Get(ChangeBossAIKey, ref ChangeBossAI);
				Configuration.Get(AddCheatSheetButtonKey, ref AddCheatSheetButton);
				Configuration.Get(AddExpertCommandKey, ref AddExpertCommand);
//				Configuration.Get(TransformMatrixKey, ref TransformMatrix);
				return true;
			}
			return false;
		}
		
		public static void SaveConfig()
		{
			Configuration.Clear();
			Configuration.Put(DropBagsKey, DropBags);
			Configuration.Put(DemonHeartHackKey, DemonHeartHack);
			Configuration.Put(ChangeBossAIKey, ChangeBossAI);
			Configuration.Put(AddCheatSheetButtonKey, AddCheatSheetButton);
			Configuration.Put(AddExpertCommandKey, AddExpertCommand);
//			Configuration.Put(TransformMatrixKey, TransformMatrix);
			Configuration.Save();
		}
		
		class MultiplayerSyncWorld : ModWorld
		{
			public override void NetSend(BinaryWriter writer)
			{
				var data = new BitsByte();
				data[0] = ChangeBossAI;
				data[1] = DropBags;
				data[2] = DemonHeartHack;
				writer.Write((byte)data);
			}
			
			public override void NetReceive(BinaryReader reader)
			{
				SaveConfig();
				var data = (BitsByte)reader.ReadByte();
				ChangeBossAI = data[0];
				DropBags = data[1];
				DemonHeartHack = data[2];
			}
		}
		
		class MultiplayerSyncPlayer : ModPlayer
		{
			public override void PlayerDisconnect(Player player)
			{
				ReadConfig();
			}
		}
	}
}
