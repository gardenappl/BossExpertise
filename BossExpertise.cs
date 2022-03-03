using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossExpertise
{
    public class BossExpertise : Mod
	{

		public static int CurrentDifficulty = 0;
		public override void Load()
		{

			LegacyConfigV1.Load();
			LegacyConfigV2.Load();
			AddConfig("Config", new Config());
			if (ModContent.GetInstance<Config>().CurrentFakedDifficulty == "Expert")
			{
				CurrentDifficulty = 1;
			}
			else if (ModContent.GetInstance<Config>().CurrentFakedDifficulty == "Master")
			{
				CurrentDifficulty = 2;
			}
			else
			{
				CurrentDifficulty = 0;
			}
		}
		
		/*public override void PostSetupContent()
		{
			if(ModContent.GetInstance<Config>().AddCheatSheetButton)
				CheatSheetIntegration.Load();
		}*/

		public override void Unload()
		{
			LegacyConfigV1.Unload();
			LegacyConfigV2.Unload();
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			var msgType = (ExpertMessageType)reader.ReadByte();
//			Log("Received message! Type: {0}, Net mode: {1}", msgType, Main.netMode);
			switch(msgType)
			{
				case ExpertMessageType.SyncDifficulty:
					int difficulty = reader.ReadInt32();
					HookDifficultyMode(difficulty);
					if (Main.netMode == NetmodeID.Server)
					{
						SyncDifficultyMode(difficulty, whoAmI);
					}
					return;
				case ExpertMessageType.SyncDemonHeart:
					var player = Main.player[reader.ReadInt32()];
					player.extraAccessory = reader.ReadBoolean();
					return;
			}
		}
		
		void SyncDifficultyMode(int difficulty, int ignoreClient = -1)
		{ 
			HookDifficultyMode(difficulty);
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				var msg = GetPacket();
				msg.Write((byte)ExpertMessageType.SyncDifficulty);
				msg.Write(difficulty);
				msg.Send(ignoreClient: ignoreClient);
			}
		}
		
		public static void HookDifficultyMode(int difficulty)
        {
			if (difficulty == 0 || difficulty > 2)
            {
				FieldInfo expert = typeof(Main)
				.GetField("_overrideForExpertMode", BindingFlags.Static | BindingFlags.NonPublic);
				expert.SetValue(null, false);
				FieldInfo master = typeof(Main)
				.GetField("_overrideForMasterMode", BindingFlags.Static | BindingFlags.NonPublic);
				master.SetValue(null, false);
			}
			else if (difficulty == 1)
			{
				FieldInfo expert = typeof(Main)
				.GetField("_overrideForExpertMode", BindingFlags.Static | BindingFlags.NonPublic);
				expert.SetValue(null, true);
			}
			else if (difficulty == 2)
			{
				FieldInfo expert = typeof(Main)
				.GetField("_overrideForExpertMode", BindingFlags.Static | BindingFlags.NonPublic);
				expert.SetValue(null, true);
				FieldInfo master = typeof(Main)
				.GetField("_overrideForMasterMode", BindingFlags.Static | BindingFlags.NonPublic);
				master.SetValue(null, true);
			}
		}

		public void SetDifficultyMode(int difficulty)
		{
			if (difficulty > 2)
			{
				difficulty = 0;
			}
			if (Main.expertMode && difficulty == 0)
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowNormalMode"));
			}
			else if (!Main.expertMode && difficulty == 1)
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowExpertMode"), 255, 50, 50);
			}
			else if (!Main.masterMode && difficulty == 2)
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowMasterMode"), 255, 50, 50);
			}
			else if (!Main.expertMode && difficulty == 0)
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.AlreadyNormalMode"));
			}
			else if (Main.expertMode && difficulty == 1)
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.AlreadyExpertMode"), 255, 50, 50);
			}
			else if (!Main.masterMode && difficulty == 2)
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.AlreadyMasterMode"), 255, 50, 50);
			}
			else
			{
				Log("Something went wrong in BossExpertise: " + CurrentDifficulty);
				return;
			}
			SyncDifficultyMode(difficulty);
			CurrentDifficulty = difficulty;
		}

		public static void Log(object message)
		{
			ModContent.GetInstance<BossExpertise>().Logger.Info(message);
		}
	}
}
