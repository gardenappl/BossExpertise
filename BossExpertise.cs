using System;
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
		public static Difficulty FakedDifficulty;
		public static Difficulty FakedBeneficialDifficulty;
		public override void Load()
		{

			LegacyConfigV1.Load();
			LegacyConfigV2.Load();
			AddConfig("Config", new Config());
			string FakedDifficultySetting = ModContent.GetInstance<Config>().CurrentFakedDifficulty;
			if (FakedDifficultySetting == "Expert")
			{
				FakedDifficulty |= Difficulty.Expert;
			}
			else if (FakedDifficultySetting == "Master")
			{
				FakedDifficulty |= Difficulty.Master;
			}
			else
			{
				FakedDifficulty |= Difficulty.Classic;
			}

			string FakedBeneficialDifficultySetting = ModContent.GetInstance<Config>().FakedBeneficialDifficulty;
			if (FakedBeneficialDifficultySetting == "Expert")
			{
				FakedBeneficialDifficulty |= Difficulty.Expert;
			}
			else if (FakedBeneficialDifficultySetting == "Master")
			{
				FakedBeneficialDifficulty |= Difficulty.Master;
			}
			else
			{
				FakedBeneficialDifficulty |= Difficulty.Classic;
			}
		}
		
		public override void PostSetupContent()
		{
			if (ModContent.GetInstance<Config>().AddCheatSheetButton)
				CheatSheetIntegration.Load();
		}

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
					Difficulty difficulty = (Difficulty)reader.ReadByte();
					HookDifficultyMode(difficulty);
					if (Main.netMode == NetmodeID.Server)
						SyncDifficultyMode(difficulty, whoAmI);
					return;
				case ExpertMessageType.SyncDemonHeart:
					var player = Main.player[reader.ReadInt32()];
					player.extraAccessory = reader.ReadBoolean();
					return;
			}
		}
		
		void SyncDifficultyMode(Difficulty difficulty, int ignoreClient = -1)
		{ 
			HookDifficultyMode(difficulty);
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				var msg = GetPacket();
				msg.Write((byte)ExpertMessageType.SyncDifficulty);
				msg.Write((byte)difficulty);
				msg.Send(ignoreClient: ignoreClient);
			}
		}
		
		public static void HookDifficultyMode(Difficulty difficulty)
        {
			if (difficulty.HasFlag(Difficulty.Expert))
			{
				FieldInfo expert = typeof(Main)
				.GetField("_overrideForExpertMode", BindingFlags.Static | BindingFlags.NonPublic);
				expert.SetValue(null, true);
				FieldInfo master = typeof(Main)
				.GetField("_overrideForMasterMode", BindingFlags.Static | BindingFlags.NonPublic);
				master.SetValue(null, false);

			}
			else if (difficulty.HasFlag(Difficulty.Master))
			{
				FieldInfo expert = typeof(Main)
				.GetField("_overrideForExpertMode", BindingFlags.Static | BindingFlags.NonPublic);
				expert.SetValue(null, true);
				FieldInfo master = typeof(Main)
				.GetField("_overrideForMasterMode", BindingFlags.Static | BindingFlags.NonPublic);
				master.SetValue(null, true);
			}
			else
			{
				FieldInfo expert = typeof(Main)
				.GetField("_overrideForExpertMode", BindingFlags.Static | BindingFlags.NonPublic);
				expert.SetValue(null, false);
				FieldInfo master = typeof(Main)
				.GetField("_overrideForMasterMode", BindingFlags.Static | BindingFlags.NonPublic);
				master.SetValue(null, false);
			}
			//Main.getGoodWorld = difficulty.HasFlag(Difficulty.ForTheWorthy);
		}

		public void SetDifficultyMode(Difficulty difficulty)
		{
			Log(FakedDifficulty);
			Log(difficulty);
			if (FakedDifficulty.HasFlag(Difficulty.Classic) && difficulty.HasFlag(Difficulty.Classic))
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.AlreadyNormalMode"));
			}
			else if (FakedDifficulty.HasFlag(Difficulty.Expert) && difficulty.HasFlag(Difficulty.Expert))
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.AlreadyExpertMode"), 255, 127, 50);
			}
			else if (FakedDifficulty.HasFlag(Difficulty.Master) && difficulty.HasFlag(Difficulty.Master))
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.AlreadyMasterMode"), 255, 50, 50);
			}
			else if (difficulty.HasFlag(Difficulty.Classic))
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowNormalMode"));
			}
			else if (difficulty.HasFlag(Difficulty.Expert))
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowExpertMode"), 255, 127, 50);
			}
			else if (difficulty.HasFlag(Difficulty.Master))
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowMasterMode"), 255, 50, 50);
			}
			else
			{
				Log("Something went wrong in BossExpertise: " + FakedDifficulty);
				return;
			}

			/*if (FakedDifficulty.HasFlag(Difficulty.ForTheWorthy) && difficulty.HasFlag(Difficulty.ForTheWorthy))
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.AlreadyForTheWorthy"), 255, 50, 50);
			}
			else if (difficulty.HasFlag(Difficulty.ForTheWorthy))
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowForTheWorthy"));
			}*/

			if ( FakedDifficulty != difficulty )
            {
				SyncDifficultyMode(difficulty);
				FakedDifficulty = difficulty;
			}
		}

		public static void Log(object message)
		{
			ModContent.GetInstance<BossExpertise>().Logger.Info(message);
		}
	}

	[Flags]
	public enum Difficulty : byte
    {
		None = 0,
		Classic = 1,
		Expert = 2,
		Master = 4,
		ForTheWorthy = 8
	}
	
}
