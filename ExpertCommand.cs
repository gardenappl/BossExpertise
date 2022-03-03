
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossExpertise
{
	/*public class ExpertCommand : ModCommand
	{
		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if(args.Length == 0)
			{
                ModContent.GetInstance<BossExpertise>().SetDifficultyMode(BossExpertise.CurrentDifficulty + 1);
			}
			else if(args.Length == 1)
			{
				if (args[0].Equals("2", StringComparison.OrdinalIgnoreCase))
				{
					ModContent.GetInstance<BossExpertise>().SetDifficultyMode(2);
				}
				if (args[0].Equals("1", StringComparison.OrdinalIgnoreCase))
				{
                    ModContent.GetInstance<BossExpertise>().SetDifficultyMode(1);
				}
				else if(args[0].Equals("0", StringComparison.OrdinalIgnoreCase))
				{
                    ModContent.GetInstance<BossExpertise>().SetDifficultyMode(0);
				}
				else
				{
					Main.NewText(Language.GetTextValue("Mods.BossExpertise.DifficultyCommandUsage"));
				}
			}
			else
			{
				Main.NewText(Language.GetTextValue("Mods.BossExpertise.DifficultyCommandUsage"));
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


	}*/
}
