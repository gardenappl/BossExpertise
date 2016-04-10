
using System;
using System.IO;
using Terraria;
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
		
		public override void ChatInput(string text)
		{
			if(!Config.AddCommand)
				return;
			
			if(text.Equals("/expert"))
			{
				if(Main.expertMode)
				{
					Main.expertMode = false;
					Main.NewText("This world is now in Nomral Mode");
				}
				else
				{
					Main.expertMode = true;
					Main.NewText("This world is now in Expert Mode!", 255, 50, 50);
				}
				return;
			}
			if(text.StartsWith("/expert"))
			{
				string[] words = text.Split(' ');
				if(words.Length == 2)
				{
					if(words[1].Equals("true"))
					{
						Main.expertMode = true;
						Main.NewText("This world is now in Expert Mode!", 255, 50, 50);
					}
					else if(words[1].Equals("false"))
					{
						Main.expertMode = false;
						Main.NewText("This world is now in Normal Mode");
					}
					return;
				}
				Main.NewText("Usage: /expert OR /expert <true|false>");
				return;
			}
		}
		
		public override void Load()
		{
			Log("Loading...");
			Config.Load();
		}
		
		public override void PostSetupContent()
		{
			if(Config.AddCheatSheetButton)
				CheatSheetIntegration.Load(this);
		}
		
		public static void Log(string message, params object[] formatData)
		{
			ErrorLogger.Log("[BossExpertise] " + string.Format(message, formatData));
		}
	}
}
