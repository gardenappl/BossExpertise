
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class BossExpertise : Mod
	{
		public static BossExpertise Instance;
		
		public override void Load()
		{

			LegacyConfigV1.Load();
			LegacyConfigV2.Load();

			AddConfig("Config", new Config());
			
			if(ModContent.GetInstance<Config>().AddExpertCommand)
				AddCommand("expert", new ExpertCommand());
		}
		
		public override void PostSetupContent()
		{
			if(ModContent.GetInstance<Config>().AddCheatSheetButton)
				CheatSheetIntegration.Load();
		}
		
		public override void PreSaveAndQuit()
		{
			if(ExpertGlobalNPC.FakeExpert) //an extra check just in case
			{
				Main.expertMode = false;
				ExpertGlobalNPC.FakeExpert = false;
			}
		}

		public override void Unload()
		{

		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			var msgType = (ExpertMessageType)reader.ReadByte();
//			Log("Received message! Type: {0}, Net mode: {1}", msgType, Main.netMode);
			switch(msgType)
			{
				case ExpertMessageType.SyncExpert:
					bool expert = reader.ReadBoolean();
					Main.expertMode = expert;
					if(Main.netMode == NetmodeID.Server)
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
			Main.expertMode = expert;
			if(Main.netMode != NetmodeID.SinglePlayer)
			{
				var msg = GetPacket();
				msg.Write((byte)ExpertMessageType.SyncExpert);
				msg.Write(expert);
				msg.Send(ignoreClient: ignoreClient);
			}
		}
		
		public void SetExpertMode(bool expert)
		{
			if(Main.expertMode && !expert)
			{
				SyncExpertMode(false);
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.NowNormalMode"));
			}
			else if(!Main.expertMode && expert)
			{
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
	}
}
