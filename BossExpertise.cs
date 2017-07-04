﻿
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
		public override void Load()
		{
			OldConfig.Load();
			Config.Load();
			if(Config.AddExpertCommand)
			{
				AddCommand("expert", new ExpertCommand());
			}
			/* Translation Start */
			var text = CreateTranslation("NowNormalMode");
			text.SetDefault("The world is now in Normal Mode.");
			text.AddTranslation(GameCulture.Russian, "Этот мир теперь в Нормальном режиме.");
			AddTranslation(text);
			text = CreateTranslation("NowExpertMode");
			text.SetDefault("The world is now in Expert Mode!");
			text.AddTranslation(GameCulture.Russian, "Этот мир теперь в Режиме эксперта!");
			AddTranslation(text);
			text = CreateTranslation("AlreadyNormalMode");
			text.SetDefault("The world is already in Normal Mode.");
			text.AddTranslation(GameCulture.Russian, "Этот мир уже в Нормальном режиме.");
			AddTranslation(text);
			text = CreateTranslation("AlreadyExpertMode");
			text.SetDefault("The world is already in Expert Mode!");
			text.AddTranslation(GameCulture.Russian, "Этот мир уже в Режиме эксперта!");
			AddTranslation(text);
			text = CreateTranslation("ToggleModePermission");
			text.SetDefault("Toggle Expert Mode");
			text.AddTranslation(GameCulture.Russian, "Включать/выключать Режим эксперта");
			AddTranslation(text);
			text = CreateTranslation("SwitchToNormal");
			text.SetDefault("Switch to Normal Mode");
			text.AddTranslation(GameCulture.Russian, "Перейти в Нормальный режим");
			AddTranslation(text);
			text = CreateTranslation("SwitchToExpert");
			text.SetDefault("Switch to Expert Mode");
			text.AddTranslation(GameCulture.Russian, "Перейти в Режим эксперта");
			AddTranslation(text);
			text = CreateTranslation("ExpertCommandUsage");
			text.SetDefault("Usage: /expert OR /expert <true|false>");
			text.AddTranslation(GameCulture.Russian, "Использование: /expert или /expert <true|false>");
			AddTranslation(text);
			text = CreateTranslation("RightClickToUse");
			text.SetDefault("<right> to use!");
			text.AddTranslation(GameCulture.Russian, "Нажмите <ПКМ>, чтобы использовать!");
			AddTranslation(text);
			/* Tranlsation End */
		}
		
		public override void PostSetupContent()
		{
			if(Config.AddCheatSheetButton)
			{
				ModIntegration.Load(this);
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
		
		public override void PreSaveAndQuit()
		{
			if(ExpertGlobalNPC.FakeExpert) //an extra check just in case
			{
				Main.expertMode = false;
				ExpertGlobalNPC.FakeExpert = false;
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
		
		
		//Fun stuff
		
		float prevAngle;
		float targetAngle;
		int tick;
		
		public override Matrix ModifyTransformMatrix(Matrix transform)
		{
			if(Config.TransformMatrix)
			{
				float angle = MathHelper.Lerp(prevAngle, targetAngle, 0.05f);
				prevAngle = angle;
//				Main.NewText(angle.ToString());
				
				if(Main.playerInventory)
				{
					tick++;
					if(tick >= 30)
					{
						tick = 0;
						angle %= 360f;
						targetAngle = angle + (Main.rand.NextFloat() * 180f - 90f);
					}
				}
				else
					targetAngle = 0;
				
				float transX = Main.screenWidth / 2;
				float transY = Main.screenHeight / 2;
				return transform
					* Matrix.CreateTranslation(-transX, -transY, 0f)
					* Matrix.CreateRotationZ(MathHelper.ToRadians(angle))
					* Matrix.CreateTranslation(transX, transY, 0f);
			}
			return transform;
		}
	}
}
