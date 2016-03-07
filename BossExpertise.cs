
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class BossExpertise : Mod
	{
		public override void SetModInfo(out string name, ref ModProperties properties)
		{
			name = "BossExpertise";
			properties.Autoload = true;
			properties.AutoloadGores = true;
			properties.AutoloadSounds = true;
		}
		
		public override void ChatInput(string text)
		{
			if(text.Equals("/expert", StringComparison.InvariantCultureIgnoreCase))
			{
				if(Main.expertMode)
				{
					Main.expertMode = false;
					Main.NewText("This world is now in Normal Mode");
				}
				else
				{
					Main.expertMode = true;
					Main.NewText("This world is now in Expert Mode!", 255, 50, 50);
				}
				return;
			}
			if(text.StartsWith("/expert", StringComparison.InvariantCultureIgnoreCase))
			{
				string[] words = text.Split(' ');
				if(words.Length != 2)
				{
					Main.NewText("Usage: /expert OR /expert <true|false>");
					return;
				}
				if(words[1].Equals("true", StringComparison.InvariantCultureIgnoreCase))
				{
					Main.expertMode = true;
					Main.NewText("This world is now in Expert Mode!", 255, 50, 50);
				}
				else if(words[1].Equals("false", StringComparison.InvariantCultureIgnoreCase))
				{
					Main.expertMode = false;
					Main.NewText("This world is now in Normal Mode");
				}
				else
					Main.NewText("Usage: /expert OR /expert <true|false>");
				return;
			}
		}
	}
}
