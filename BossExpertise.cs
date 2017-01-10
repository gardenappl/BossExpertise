
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class BossExpertise : Mod
	{	
		public BossExpertise()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
		
		public override void Load()
		{
			OldConfig.Load();
			Config.Load();
		}
		
		public override void PostSetupContent()
		{
			if(Config.AddCheatSheetButton)
				CheatSheetIntegration.Load(this);
		}
		
		public override void ChatInput(string text, ref bool broadcast)
		{
			//argument split code from ExampleMod
			if (text[0] != '/')
				return;
			int index = text.IndexOf(' ');
			string command;
			string[] args;
			if (index < 0)
			{
				command = text.Substring(1);
				args = new string[0];
			}
			else
			{
				command = text.Substring(1, index - 1);
				args = text.Substring(index).Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
			}
			
			switch(command)
			{
				case "expert":
					if(Config.AddExpertCommand)
					{
						if(args.Length == 0)
							SetExpertMode(!Main.expertMode);
						else if(args.Length == 1)
						{
							if(args[0].Equals("true", StringComparison.OrdinalIgnoreCase))
								SetExpertMode(true);
							else if(args[0].Equals("false", StringComparison.OrdinalIgnoreCase))
								SetExpertMode(false);
							else
								Main.NewText("Usage: /expert OR /expert <true|false>");
						}
						else
							Main.NewText("Usage: /expert OR /expert <true|false>");
						broadcast = false;
					}
					return;
			}
		}
		
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			var msgType = (ExpertMessageType)reader.ReadByte();
//			Log("Received message! Type: {0}, Net mode: {1}", msgType, Main.netMode);
			switch(msgType)
			{
				case ExpertMessageType.SyncExpert:
					Main.expertMode = reader.ReadBoolean();
					return;
				case ExpertMessageType.SyncDemonHeart:
					var player = Main.player[reader.ReadInt32()];
					player.extraAccessory = reader.ReadBoolean();
					return;
			}
		}
		
		public void SyncExpertMode(bool expert)
		{
			Main.expertMode = expert;
			if(Main.netMode == 1 || Main.netMode == 2) //Multiplayer
			{
				var msg = GetPacket();
				msg.Write((byte)ExpertMessageType.SyncExpert);
				msg.Write(expert);
				msg.Send();
			}
		}
		
		public void SetExpertMode(bool expert)
		{
			if(Main.expertMode && !expert)
			{
				SyncExpertMode(false);
				Main.NewText("This world is now in Normal Mode");
			}
			else if(!Main.expertMode && expert)
			{
				SyncExpertMode(true);
				Main.NewText("This world is now in Expert Mode!", 255, 50, 50);
			}
			else if(!Main.expertMode && !expert)
			{
				Main.NewText("This world is already in Normal Mode");
			}
			else
			{
				Main.NewText("This world is already in Expert Mode!", 255, 50, 50);
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
