
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
		public static bool FKtModSettingsLoaded;
		
		public override void Load()
		{
			Instance = this;
			FKtModSettingsLoaded = ModLoader.GetMod("FKTModSettings") != null;
			
			Config.Load();
			if(FKtModSettingsLoaded && !Main.dedServ)
				Config.LoadFKConfig();
			
			if(Config.AddExpertCommand)
				AddCommand("expert", new ExpertCommand());
		}
		
		public override void PostSetupContent()
		{
			if(Config.AddCheatSheetButton)
				CheatSheetIntegration.Load();
		}
		
		public override void PostUpdateInput()
		{
			if(FKtModSettingsLoaded && !Main.dedServ && !Main.gameMenu)
				Config.UpdateFKConfig();
		}
		
		public override void PreSaveAndQuit()
		{
			if(FKtModSettingsLoaded && !Main.dedServ)
				Config.SaveConfig();
			
			if(ExpertGlobalNPC.FakeExpert) //an extra check just in case
			{
				Main.expertMode = false;
				ExpertGlobalNPC.FakeExpert = false;
			}
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
		
		public void SyncExpertMode(bool expert, int ignoreClient = -1)
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
		
		public static void Log(object message, params object[] formatData)
		{
			ErrorLogger.Log("[Boss Expertise] " + string.Format(message.ToString(), formatData));
		}


		/* Fun stuff (does not work anymore) */

		//		float Rotation;
		//		float Speed;

		//		public override Matrix ModifyTransformMatrix(Matrix transform)
		//		{
		////			Main.NewText(Rotation.ToString());
		//			if(Config.TransformMatrix && !Main.gameMenu)
		//			{
		//				Rotation = MathHelper.WrapAngle(Rotation + Speed);
		//				Speed += 0.001f;

		//				float transX = Main.screenWidth / 2;
		//				float transY = Main.screenHeight / 2;
		//				return transform
		//					* Matrix.CreateTranslation(-transX, -transY, 0f)
		//					* Matrix.CreateRotationZ(Rotation)
		//					* Matrix.CreateTranslation(transX, transY, 0f);
		//			}
		//			Rotation = 0f;
		//			Speed = 0f;
		//			return transform;
		//		}

		#region Hamstar's Mod Helpers integration

		public static string GithubUserName { get { return "goldenapple3"; } }
		public static string GithubProjectName { get { return "BossExpertise"; } }

		public static string ConfigFileRelativePath { get { return "Mod Configs/Boss Expertise.json"; } }

		public static void ReloadConfigFromFile()
		{
			Config.Load();
		}

		public static void ResetConfigFromDefaults()
		{
			Config.SetDefaults();
			Config.SaveConfig();
		}

		#endregion
	}
}
