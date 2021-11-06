
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertCommand : ModCommand
	{
		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if(args.Length == 0)
			{
                ModContent.GetInstance<BossExpertise>().SetExpertMode(!Main.expertMode);
			}
			else if(args.Length == 1)
			{
				if(args[0].Equals("true", StringComparison.OrdinalIgnoreCase))
				{
                    ModContent.GetInstance<BossExpertise>().SetExpertMode(true);
				}
				else if(args[0].Equals("false", StringComparison.OrdinalIgnoreCase))
				{
                    ModContent.GetInstance<BossExpertise>().SetExpertMode(false);
				}
				else
				{
					Main.NewText(Language.GetTextValue("Mods.BossExpertise.ExpertCommandUsage"));
				}
			}
			else
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.ExpertCommandUsage"));
			}
			
		}

        public override string Command
		{
			get { return "expert"; }
		}

		public override CommandType Type
		{
			get { return CommandType.World; }
		}

		public void AddCommand()
        {
			Register();
		}


	}
}
