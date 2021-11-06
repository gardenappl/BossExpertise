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

		public static bool? FakeExpert = false;
		public override void Load()
		{

			LegacyConfigV1.Load();
			LegacyConfigV2.Load();

			AddConfig("Config", new Config());

		}
		
		public override void PostSetupContent()
		{
			if(ModContent.GetInstance<Config>().AddCheatSheetButton)
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
				case ExpertMessageType.SyncExpert:
					bool expert = reader.ReadBoolean();
					HookExpertMode(expert);
					if (Main.netMode == NetmodeID.Server)
					{
						SyncExpertMode(expert, whoAmI);
					}
					return;
				case ExpertMessageType.SyncDemonHeart:
					var player = Main.player[reader.ReadInt32()];
					player.extraAccessory = reader.ReadBoolean();
					return;
			}
		}
		
		void SyncExpertMode(bool expert, int ignoreClient = -1)
		{
			HookExpertMode(expert);
			if(Main.netMode != NetmodeID.SinglePlayer)
			{
				var msg = GetPacket();
				msg.Write((byte)ExpertMessageType.SyncExpert);
				msg.Write(expert);
				msg.Send(ignoreClient: ignoreClient);
			}
		}
		
		public static void HookExpertMode(bool? givenValue)
        {
			FieldInfo expert = typeof(Main)
				.GetField("_overrideForExpertMode", BindingFlags.Static | BindingFlags.NonPublic);
				expert.SetValue(null, givenValue);
		}

		public void SetExpertMode(bool expert)
		{
			if(Main.expertMode && !expert)
			{
				FakeExpert = expert;
				SyncExpertMode(false);
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowNormalMode"));
			}
			else if(!Main.expertMode && expert)
			{
				FakeExpert = expert;
                Log(FakeExpert);
				SyncExpertMode(true);
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowExpertMode"), 255, 50, 50);
			}
			else if(!Main.expertMode && !expert)
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.AlreadyNormalMode"));
			}
			else
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.AlreadyExpertMode"), 255, 50, 50);
			}
		}

		public static void Log(object message)
		{
			ModContent.GetInstance<BossExpertise>().Logger.Info(message);
		}
	}
}
